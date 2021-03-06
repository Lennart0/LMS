﻿using LMS.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace LMS.DataAccessLayer
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base( "DefaultConnection", throwIfV1Schema: false )
        {
            //   Database.SetInitializer<ApplicationDbContext>(null); // should probably uncomment this later since it probablly a bit of a dirty fix.... to avoid a minor issue and let me test what the underlying problem is...

        }



        public DbSet<Course> Courses { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Activity> Activies { get; set; }

        public DbSet<PlainDocument> PlainDocuments { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<TimeSensetiveDocument> TimeSensetiveDocuments { get; set; }
        public DbSet<AssignmentSubmission> AssignmentSubmissions { get; set; }

        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating( DbModelBuilder modelBuilder )
        {
            base.OnModelCreating( modelBuilder );
            modelBuilder.Entity<Course>().HasKey( n => n.Id );


            modelBuilder.Entity<Document>().ToTable( "Document" );
            modelBuilder.Entity<TimeSensetiveDocument>().ToTable( "TimeSensetiveDocument" );
            modelBuilder.Entity<AssignmentSubmission>().ToTable( "AssignmentSubmission" );
            modelBuilder.Entity<PlainDocument>().ToTable("PlainDocuments");



            modelBuilder.Entity<Course>().HasMany(n => n.Students).WithOptional(n => n.Course);//.HasForeignKey(n=> n.CourseId)
            modelBuilder.Entity<Course>().HasMany(n => n.Modules).WithOptional(n => n.Course);//.HasForeignKey(n=> n.CourseId)
            modelBuilder.Entity<Course>().HasMany(n => n.Documents).WithOptional(n => n.Course);//.HasForeignKey(n=> n.CourseId);


            modelBuilder.Entity<Module>().HasKey(n => n.Id);
            modelBuilder.Entity<Module>().HasMany(n => n.Activities).WithOptional(n => n.Module);//.HasForeignKey(n=> n.CourseId)
            modelBuilder.Entity<Module>().HasMany(n => n.Documents).WithOptional(n => n.Module);//.HasForeignKey(n=> n.ModuleId);

            modelBuilder.Entity<Activity>().HasKey(n => n.Id);
            modelBuilder.Entity<Activity>().HasMany(n => n.Documents).WithOptional(n => n.Activity);//.HasForeignKey(n=> n.ActivityId);


            modelBuilder.Entity<Document>().HasKey( n => n.Id );
            modelBuilder.Entity<Document>().HasRequired( n => n.User ).WithMany( n => n.Documents );



        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

}