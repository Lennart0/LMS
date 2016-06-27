using LMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin;


namespace LMS.Helpers {
    public class DefaultUserAndRoleStartupHelper {


        public static void Create() {

            using (var db = new DataAccessLayer.ApplicationDbContext()) {
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

                //if and only if role is empty add roll
                if (db.Roles.Count(n => n.Name == Helpers.Constants.TeacherRole) == 0) {
                    db.Roles.Add(new IdentityRole(Helpers.Constants.TeacherRole));
                    db.SaveChanges();
                }



                var defaultTeacher = new ApplicationUser() {
                    Email = "Larar@lararson.se",
                    UserName = "Larar@lararson.se",
                    Id = "679a290d-8b3b-4488-8ffb-7dea7a44efca"
                };

                //if and only if default user is missing add user
                if (db.Users.SingleOrDefault(n => n.Id == defaultTeacher.Id) == null) {
                    var result = UserManager.Create(defaultTeacher, "@ackN0w");
                    if (defaultTeacher?.Roles.Count == 0 && result.Succeeded == true) {
                        UserManager.AddToRole(defaultTeacher.Id, Helpers.Constants.TeacherRole);
                    }
                }

              
            }

        }
    }
}