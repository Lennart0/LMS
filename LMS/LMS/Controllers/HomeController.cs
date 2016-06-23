using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace LMS.Controllers
{
    public class HomeController : Controller
    {

       // DataAccessLayer.ApplicationDbContext db = new DataAccessLayer.ApplicationDbContext();
        public ActionResult Index()
        {
       //    var x = db.Courses.Include( n => n.Students ).Where()

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}