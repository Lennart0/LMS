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
using LMS.Helpers;

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
        public ActionResult Details(Guid? id, string returnUrl)
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

            DocHelper.AssocDocsToViewBag( activity.Documents, ViewBag );

            return View(activity);
        }

        // GET: Activities/Create
        private const string ModuleIdKey = "moduleid";
        private const string ActivityCreateReturnUrlKey = "actcrereturnurl";
        public ActionResult Create( Guid? id = null /* ModuleId*/, string returnUrl = null )
        {
            //if ( id != null )
              //  HttpContext.Session.Contents[ModuleIdKey] = id;

            HttpContext.Session.Contents[ActivityCreateReturnUrlKey] = returnUrl;
            var now = DateTime.Now;
            return View(new Activity() { ModuleId = id, Documents= new List<Document>(), Start = new DateTime(now.Year, now.Month, now.Day, 9,0,0) , End= new DateTime(now.Year, now.Month, now.Day, 17, 0, 0) });
        }

        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,Name,Start,End,ModuleId")] Activity activity)
        {
           // Guid? moduleId = (Guid?)HttpContext.Session.Contents[ModuleIdKey];

            if ( ModelState.IsValid && activity.ModuleId != null) //moduleId != null )
            {
                //activity.Module = db.Modules.SingleOrDefault( m => m.Id == moduleId.Value );
             //   activity.ModuleId = moduleId;

                activity.Id = Guid.NewGuid();
                db.Activies.Add(activity);
                db.SaveChanges();

                string returnUrl = (string)HttpContext.Session.Contents[ActivityCreateReturnUrlKey];
                if ( !string.IsNullOrEmpty( returnUrl ) )
                    return Redirect( returnUrl );

                if (Request.QueryString["mode"] == "schedule") {
                    return Content(""); //dont flash anything stupid in schedule mode
                }

                    return RedirectToAction( "Details", "Modules", new { id = activity.ModuleId }); // moduleId.Value } );
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

            //if ( returnUrl != null )
                HttpContext.Session.Contents[ActivityEditReturnUrlKey] = returnUrl;

            DocHelper.AssocDocsToViewBag( activity.Documents, ViewBag );

            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,Name,Start,End,ModuleId")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activity).State = EntityState.Modified;
                db.SaveChanges();

                string returnUrl = (string)HttpContext.Session.Contents[ActivityEditReturnUrlKey];

                if (Request.QueryString["mode"] == "schedule") {
                    return Content(""); //dont flash anything stupid in schedule mode
                }

                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect( returnUrl );

                return RedirectToAction("Index");
            }
            return View(activity);
        }

        // GET: Activities/Delete/5
        private const string ActivityDeleteReturnUrlKey = "actdelreturnurl";
        public ActionResult Delete(Guid? id, string returnUrl)
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

            HttpContext.Session.Contents[ActivityDeleteReturnUrlKey] = returnUrl;

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

            string returnUrl = (string)HttpContext.Session.Contents[ActivityDeleteReturnUrlKey];
            if ( !string.IsNullOrEmpty( returnUrl ) )
                return Redirect( returnUrl );

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
