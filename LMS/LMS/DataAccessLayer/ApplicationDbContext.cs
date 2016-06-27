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
        public DbSet<Module> Modules { get; set; }
        public DbSet<Activity> Activies { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Course>().HasKey(n => n.Id);


            modelBuilder.Entity<Course>().HasMany(n => n.Students).WithOptional(n => n.Course);//.HasForeignKey(n=> n.CourseId)
            modelBuilder.Entity<Course>().HasMany(n => n.Modules).WithOptional(n => n.Course);//.HasForeignKey(n=> n.CourseId)
            modelBuilder.Entity<Course>().HasMany(n => n.Documents).WithOptional();


            modelBuilder.Entity<Module>().HasKey(n => n.Id);
            modelBuilder.Entity<Module>().HasMany(n => n.Activities).WithOptional(n => n.Module);//.HasForeignKey(n=> n.CourseId)
            modelBuilder.Entity<Module>().HasMany(n => n.Documents).WithOptional();

            modelBuilder.Entity<Activity>().HasKey(n => n.Id);
            modelBuilder.Entity<Activity>().HasMany(n => n.Documents).WithOptional(n=> n.Activity);


            modelBuilder.Entity<Document>().HasKey(n => n.Id);
            modelBuilder.Entity<Document>().HasRequired(n => n.User).WithMany(n => n.Documents);

        



        }

        public static ApplicationDbContext Create() {
            return new ApplicationDbContext();
        }
    }

}