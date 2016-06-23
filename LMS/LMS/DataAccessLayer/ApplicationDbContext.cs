using LMS.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace LMS.DataAccessLayer {

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false) {
        }

        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Course>().HasKey(n => n.Id);
            modelBuilder.Entity<Course>().HasMany(n => n.Students).WithOptional(n => n.Course);//.HasForeignKey(n=> n.CourseId)
 


        }

        public static ApplicationDbContext Create() {
            return new ApplicationDbContext();
        }
    }

}