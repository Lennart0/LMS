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
     

        [Authorize]
        public ActionResult Index()
        {

        


            if (User.IsInRole(Helpers.Constants.TeacherRole)) {
                return RedirectToAction("Index", "Courses");
            } else {
                return View();
            }      
        }

    }
}