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
            ViewBag.CourseName = course.Name;
            ViewBag.CourseId   = course.Id;

            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Start,End")] Course course)
        {
            // This is for avoiding duplicates in DB. Check the Name and Start date of course.
            if (db.Courses.Any(c => c.Name == course.Name && c.DayStart == course.DayStart))
            {
                ModelState.AddModelError("Name", "This course has already registered!");

            }

            if (ModelState.IsValid)
            {
                course.Id = Guid.NewGuid();
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(Guid? id)
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
            ViewBag.CourseName = course.Name;
            ViewBag.CourseDescription = course.Description;

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
            ViewBag.CourseName = course.Name;
            ViewBag.CourseDescription = course.Description;

            var studentList = course?.Students.ToList();

            return View(studentList);
        }
    }
}

