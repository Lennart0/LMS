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
        [Authorize]
        public ActionResult Index()
        {

            if (User.IsInRole(Helpers.Constants.TeacherRole)) {
                return RedirectToAction("Index", "Courses/index");
            } else {
                return RedirectToAction( "Index", "Schedule" );
            }      
        }

    }
}