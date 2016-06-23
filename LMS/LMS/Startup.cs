using LMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System.Linq;

[assembly: OwinStartupAttribute(typeof(LMS.Startup))]
namespace LMS {
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);

            //Registers the Default User and role

        
            using (DataAccessLayer.ApplicationDbContext db = new DataAccessLayer.ApplicationDbContext()) {

                var store = new UserStore<ApplicationUser>(db);
                var manager = new UserManager<ApplicationUser>(store);
                var roleName = Helpers.Constants.TeacherRole;

                //if and only if role is empty add roll
                if (db.Roles.SingleOrDefault(n => n.Name == roleName) == null) {
                    db.Roles.Add(new IdentityRole(roleName));
                    db.SaveChanges();
                }
                var defaultTeacher = new ApplicationUser() {
                    Email = "Lärar@lärarson.se",
                    UserName = "StandardLarae",
                    Id = "679a290d-8b3b-4488-8ffb-7dea7a44efca",
                    EmailConfirmed=true
                };
             

                //if and only if default user is missing add user
                if (db.Users.SingleOrDefault(n => n.Id == defaultTeacher.Id) == null) {
                var x =    manager.Create(defaultTeacher, "123AbC___");
                
                }


                if (defaultTeacher?.Roles.Count == 0) {
                    manager.AddToRole(defaultTeacher.Id, "Lärare");
                }
            }
        }
    }
}
