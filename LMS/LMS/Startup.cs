using LMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Owin;
using System.Linq;
using System.Web;

[assembly: OwinStartupAttribute(typeof(LMS.Startup))]
namespace LMS {
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
            Helpers.DefaultUserAndRoleStartupHelper.Create();        
        }
    } 
}
