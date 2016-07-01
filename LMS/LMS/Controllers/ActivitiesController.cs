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
    public class ActivitiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Activities
        public ActionResult Index()
        {
            return View(db.Activies.ToList());
        }

        // GET: Activities/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activies.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // GET: Activities/Create
        private const string ModuleIdKey = "moduleid";
        public ActionResult Create( Guid? id = null /* ModuleId*/)
        {
            if ( id != null )
                HttpContext.Session.Contents[ModuleIdKey] = id;

            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,Name,Start,End")] Activity activity)
        {
            Guid? moduleId = (Guid?)HttpContext.Session.Contents[ModuleIdKey];

            if ( ModelState.IsValid && moduleId != null )
            {
                //activity.Module = db.Modules.SingleOrDefault( m => m.Id == moduleId.Value );
                activity.ModuleId = moduleId;

                activity.Id = Guid.NewGuid();
                db.Activies.Add(activity);
                db.SaveChanges();

                return RedirectToAction( "Details", "Modules", new { id = moduleId.Value } );
                //return RedirectToAction("Index");
            }

            return View(activity);
        }

        // GET: Activities/Edit/5
        private const string ActivityEditReturnUrlKey = "actedtreturnurl";
        public ActionResult Edit(Guid? id, string returnUrl )
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activies.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }

            if ( returnUrl != null )
                HttpContext.Session.Contents[ActivityEditReturnUrlKey] = returnUrl;

            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,Name,Start,End")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activity).State = EntityState.Modified;
                db.SaveChanges();

                string returnUrl = (string)HttpContext.Session.Contents[ActivityEditReturnUrlKey];
                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect( returnUrl );

                return RedirectToAction("Index");
            }
            return View(activity);
        }

        // GET: Activities/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activies.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Activity activity = db.Activies.Find(id);
            db.Activies.Remove(activity);
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
    }
}
