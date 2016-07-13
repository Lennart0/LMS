using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LMS.DataAccessLayer;
using LMS.Models;
using Microsoft.AspNet.Identity;
using LMS.Helpers;

namespace LMS.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Courses
        public ActionResult Index()
        {
            return View(db.Courses.ToList());
        }

        // GET: Courses/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);

            if (course == null)
            {
                return HttpNotFound();
            }

            ViewBag.CourseName = course.Name;
            ViewBag.CourseId = course.Id;
            DocHelper.AssocDocsToViewBag( course.Documents.Where(n=> n.PublishDate != null), ViewBag );

            return View(course);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            var templateList = db.Courses.Select( c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() } ).ToList();
            templateList.Insert( 0, new SelectListItem { Text = "Ingen mall-kurs", Value = null } );
            ViewBag.TemplateList = templateList;

            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Start,End")] Course course, string template)
        {
            // This is for avoiding duplicates in DB. Check the Name and Start date of course.
            if (db.Courses.Any(c => c.Name == course.Name && c.DayStart == course.DayStart))
            {
                ModelState.AddModelError("Name", "This course is already registered!");
            }

            if (ModelState.IsValid)
            {
                course.Id = Guid.NewGuid();
                db.Courses.Add(course);

                #region Copy content from Template course
                Course tplCourse = null;
                if ( !string.IsNullOrWhiteSpace( template ) )
                    tplCourse = db.Courses.Include("Modules").SingleOrDefault( c => c.Id == new Guid( template ) );
                if ( tplCourse != null ) {
                    Helpers.CourseDays courseDays = new CourseDays( course.Start );
                    DateTime activityDateNew = course.Start.Date - new TimeSpan( 1, 0, 0, 0 );

                    foreach ( var moduleOld in tplCourse.Modules ) {
                        Module module = new Module {
                            Id = Guid.NewGuid(),
                            Start = DateTime.MinValue,
                            Name = moduleOld.Name,
                            Description = moduleOld.Description,
                            CourseId = moduleOld.CourseId                            
                        };
                        CopyDocuments( moduleOld.Documents, null, module.Id, null, module.Start );

                        //db.Detach( module );
                        //db.Entry( module ).State = EntityState.Detached;
                        DateTime activityDateOld = DateTime.MinValue;
                        foreach ( var activityOld in moduleOld.Activities.OrderBy(a => a.Start) ) {
                            Activity activity = new Activity {
                                Id = Guid.NewGuid(),
                                Name = activityOld.Name,
                                Description = activityOld.Description,
                                ModuleId = module.Id
                            };

                            //db.Entry( activity ).State = EntityState.Detached;
                            //activity.Id = Guid.NewGuid();
                            //activity.ModuleId = module.Id;
                            if ( activityOld.Start.Date > activityDateOld ) {
                                activityDateOld = activityOld.Start.Date;
                                activityDateNew = courseDays.NextDay( activityDateNew );
                            }
                            activity.Start = activityDateNew + activityOld.Start.TimeOfDay;
                            activity.End = activityDateNew + (activityOld.End - activityDateOld);

                            CopyDocuments( activityOld.Documents, null, null, activity.Id, activityOld.Start );

                            if ( module.Start == DateTime.MinValue )
                                module.Start = activityDateNew;

                            db.Activies.Add( activity );
                        }
                        if ( module.Start == DateTime.MinValue )
                            module.Start = activityDateNew==DateTime.MinValue ? course.Start : activityDateNew;
                        module.End = activityDateNew == DateTime.MinValue ? course.Start : activityDateNew;

                        course.Modules.Add( module );
                    }
                    if ( course.End < activityDateNew )
                        course.End = activityDateNew;
                    CopyDocuments( tplCourse.Documents, course.Id, null, null, course.Start );

                }
                #endregion

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(course);
        }

        private void CopyDocuments( ICollection<Document> srcDocs, Guid? courseId, Guid? moduleId, Guid? activityId, DateTime newStartRefTime ) {
            if ( srcDocs == null || srcDocs.Count == 0 ) {
                return; // null;
            }
            //var docs = new List<Document>();
            foreach (var sd in srcDocs) {
                Document doc = null;
                if (sd is PlainDocument) {
                    doc = new PlainDocument();
                }
                else if (sd is TimeSensetiveDocument) {
                    var tsd = new TimeSensetiveDocument {
                        DeadLine = CalcNewRelativeTime(
                            GetDocRefTime( sd ),
                            ( (TimeSensetiveDocument)sd).DeadLine,
                            newStartRefTime ),
                    };
                    doc = tsd;
                }

                if ( doc != null ) {
                    doc.Id = Guid.NewGuid();
                    doc.Type = sd.Type;
                    doc.Url = sd.Url;
                    doc.IsLocal = sd.IsLocal;
                    doc.UserId = sd.UserId;
                    doc.UploadDate = sd.UploadDate;

                    doc.CourseId = courseId;
                    doc.ModuleId = moduleId;
                    doc.ActivityId = activityId;
                    if ( sd.PublishDate != null )
                        doc.PublishDate = CalcNewRelativeTime( GetDocRefTime( sd ), sd.PublishDate.Value, newStartRefTime );

                    //docs.Add( doc );
                }
            }
            //return docs;
        }


        private DateTime GetDocRefTime( Document doc ) {
            if ( doc.ActivityId != null )
                return doc.Activity.Start;
            if ( doc.ModuleId != null )
                return doc.Module.Start;
            if ( doc.CourseId != null )
                return doc.Course.Start;
            return doc.PublishDate!= null ? doc.PublishDate.Value : doc.UploadDate;
        }
        private DateTime CalcNewRelativeTime( DateTime oldTime1, DateTime oldTime2, DateTime newTime1 ) {
            CourseDays cd = new CourseDays();
            //int oldDayDiff = (oldTime2 - oldTime1).Days;
            int oldCourseDayDiff = cd.NrCourseDays( oldTime1, oldTime2 );
            DateTime newDate2 = cd.NthDayAfter( newTime1, oldCourseDayDiff );
            TimeSpan timeDiff = oldTime2.TimeOfDay - oldTime1.TimeOfDay;
            DateTime newTime2 = newDate2 + (oldTime1.TimeOfDay + timeDiff);
            return newTime2;
        }
        //DateTime CalcNewDeadLine( TimeSensetiveDocument oldDoc, DateTime newActivityStart ) {
        //    if ( oldDoc.Activity == null )
        //        return newActivityStart + new TimeSpan( 1, 0, 0, 0 );
        //    CourseDays cd = new CourseDays();
        //    int dayDiff = (oldDoc.DeadLine.Date - oldDoc.Activity.Start.Date).Days;
        //    int courseDayDiff = cd.NrDays( oldDoc.Activity.Start.Date, oldDoc.DeadLine );
        //    DateTime newDeadLine = cd.NthDayAfter( newActivityStart, courseDayDiff ) + oldDoc.DeadLine.TimeOfDay;
        //    return newDeadLine;
        //}


        // GET: Courses/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            ViewBag.CourseName = course.Name;

            if (course == null)
            {
                return HttpNotFound();
            }

            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Start,End")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            ViewBag.CourseName = course.Name;

            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// GET: Display all students in a course
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CourseStudentList(Guid? id)
        {
            Course course = id != null ? db.Courses.Find(id.Value) : null;
            ViewBag.CourseName = course != null ? course.Name : "";
            ViewBag.CourseDescription = course != null ? course.Description : "";

            var studentList = course?.Students.ToList();

            return View(studentList);
        }

        // GET: Courses
        //[Authorize(Users = "student1@test.se")]
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult CourseInfoAndStudents()
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);

            Course course = currentUser.CourseId != null ? db.Courses.Find(currentUser.CourseId.Value) : null;

            if ( course == null ) {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }


            ViewBag.CourseName = course.Name;
            ViewBag.CourseDescription = course.Description;

            var studentList = course?.Students.ToList();

            List<Document> docs =
                course.Documents.Where( n => n.PublishDate != null && n.PublishDate.Value.Date <= DateTime.Now.Date ).ToList();

            docs.AddRange( course.Modules.SelectMany( m => m.Documents ).Where( n => n.PublishDate != null && n.PublishDate.Value.Date <= DateTime.Now.Date ).ToList() );

            docs.AddRange( course.Modules.SelectMany( m => m.Activities ).SelectMany( a => a.Documents ).Where( n => n.PublishDate != null && n.PublishDate.Value.Date <= DateTime.Now.Date ).ToList() );

            if ( !User.IsInRole( Helpers.Constants.TeacherRole ) ) {
                docs.AddRange( db.Documents.Where( d => d.UserId == currentUserId ).ToList() );
            }


            DocHelper.AssocDocsToViewBag( docs, ViewBag );

            return View(studentList);
        }
    }
}

