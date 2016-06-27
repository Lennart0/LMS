using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using LMS.Models;
using System.Data.Entity.Migrations;

namespace LMS.Controllers
{
    public class HomeController : Controller
    {

        private void test() {
     

        }

        [Authorize]
        public ActionResult Index()
        {
            test();



            if (User.IsInRole(Helpers.Constants.TeacherRole)) {
                return RedirectToAction("Index", "Courses");
            } else {
                return View();
            }      
        }

    }
}