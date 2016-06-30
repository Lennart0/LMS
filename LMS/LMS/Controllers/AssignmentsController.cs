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

namespace LMS.Controllers
{
    [Authorize]
    public class AssignmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Student
        public ActionResult Index()
        {
            ApplicationUser student = null;
            if ( !User.IsInRole( Helpers.Constants.TeacherRole ) )
                student = db.Users.SingleOrDefault( u => u.UserName == User.Identity.Name );
            if ( student == null || student.Course == null )
                return Redirect( "/" );

            DateTime today = DateTime.Now;

            var assignmentDocs = student.Course.Modules
                .SelectMany( m => m.Activities )
                .SelectMany( a => a.Documents )
                .Where( d => d.PublishDate <= today && d.Type == DocumentType.Upgift && d is TimeSensetiveDocument )
                .Select(d => d as TimeSensetiveDocument)
                .ToList();

            return View( assignmentDocs );
        }

        //// GET: Student/Details/5
        //public ActionResult Details(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Document document = db.Documents.Find(id);
        //    if (document == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(document);
        //}

        //// GET: Student/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Student/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Type,Url,IsLocal,UploadDate,PublishDate")] Document document)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        document.Id = Guid.NewGuid();
        //        db.Documents.Add(document);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(document);
        //}

        //// GET: Student/Edit/5
        //public ActionResult Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Document document = db.Documents.Find(id);
        //    if (document == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(document);
        //}

        //// POST: Student/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Type,Url,IsLocal,UploadDate,PublishDate")] Document document)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(document).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(document);
        //}

        //// GET: Student/Delete/5
        //public ActionResult Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Document document = db.Documents.Find(id);
        //    if (document == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(document);
        //}

        //// POST: Student/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(Guid id)
        //{
        //    Document document = db.Documents.Find(id);
        //    db.Documents.Remove(document);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
