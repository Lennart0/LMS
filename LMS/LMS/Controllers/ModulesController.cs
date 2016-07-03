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
    public class ModulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Modules
        public ActionResult Index()
        {
            return View(db.Modules.ToList());
        }

        // GET: Modules/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // GET: Modules/Create 
        private const string ModuleEditReturnUrlKey = "modulereturnurl";
        public ActionResult Create(Guid id, string returnUrl) 
        {
            if (returnUrl != null)
                HttpContext.Session.Contents[ModuleEditReturnUrlKey] = returnUrl;

            if ( id == null ) {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }

            return View( new Module() { CourseId = id /*Course = db.Courses.FirstOrDefault(n=> n.Id== id)*/ } );
        }

        // POST: Modules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description,Start,End,CourseId")] Module module)
        { 
            if (ModelState.IsValid)
            {
                //module.Course = db.Courses.FirstOrDefault(n => n.Id == module.Course.Id);
                module.Id = Guid.NewGuid();
                db.Modules.Add(module);
                db.SaveChanges();

                string returnUrl = (string)HttpContext.Session.Contents[ModuleEditReturnUrlKey];
                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index");
            }
            return View(module);
        }


        // GET: Modules/Edit/5
        
        public ActionResult Edit(Guid? id, string returnUrl)
        {
            if (returnUrl != null)
                HttpContext.Session.Contents[ModuleEditReturnUrlKey] = returnUrl;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Module module = db.Modules.Find(id);
            ViewBag.EditModule = module.Name;

            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Start,End")] Module module)
        {
            if (ModelState.IsValid)
            {
                db.Entry(module).State = EntityState.Modified;
                db.SaveChanges();

                string returnUrl = (string)HttpContext.Session.Contents[ModuleEditReturnUrlKey];
                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index");
            }
            return View(module);
        }

        public ActionResult GoPreviousView()
        {
            return Redirect(Request.QueryString["r"]);
        }

        // GET: Modules/Delete/5
        public ActionResult Delete(Guid? id, string returnUrl)
        {
            if (returnUrl != null)
            {
                HttpContext.Session.Contents[ModuleEditReturnUrlKey] = returnUrl;
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Module module = db.Modules.Find(id);
            ViewBag.DeleteModule = module.Name;

            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Module module = db.Modules.Find(id);
            db.Modules.Remove(module);
            db.SaveChanges();

            string returnUrl = (string)HttpContext.Session.Contents[ModuleEditReturnUrlKey];
            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);

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
