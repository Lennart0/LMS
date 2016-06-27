namespace LMS.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LMS.DataAccessLayer.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed( LMS.DataAccessLayer.ApplicationDbContext context )
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //


            //:Todo Figure out later where to put the Role seeding as startup.cs does not seam to have access to the context class...
            Guid newCourseId1 = Guid.NewGuid();
            Guid newModuleId1 = Guid.NewGuid();
            Guid newModuleId2 = Guid.NewGuid();
            Guid newActivityId1 = Guid.NewGuid();
            Guid newActivityId2 = Guid.NewGuid();
            Guid newActivityId3 = Guid.NewGuid();
            Guid newActivityId4 = Guid.NewGuid();

            Course course1 = null;
            Module module1 = null;
            Module module2 = null;

            context.Courses.AddOrUpdate( c => c.Name,
                course1 = new Course {
                    Id = newCourseId1,
                    Name = "Testkurs1",
                    Description = "Beskrivning av testkurs 1...",
                    Start = new DateTime( 2016, 6, 1 ),
                    End = new DateTime( 2016, 9, 30 ),
                    DayStart = new DateTime( 2016, 6, 1, 8,30,0 ),
                    DayEnd = new DateTime( 2016, 6, 1, 17, 0, 0 ),
                    LunchStart = new DateTime( 2016, 6, 1, 12, 0, 0 ),
                    LunchEnd = new DateTime( 2016, 6, 1, 13, 0, 0 ),
                }
            );

            //context.SaveChanges();
            //context.Dispose();
            //context = new DataAccessLayer.ApplicationDbContext();
            //course1 = context.Courses.Single( c => c.Id == newCourseId1 );

            context.Modules.AddOrUpdate( m => m.Name,
                module1 = new Module {
                    Id = newModuleId1,
                    Name = "Testmodul1",
                    Description = "Beskrivning av testmodul 1...",
                    Start = new DateTime( 2016, 6, 1 ),
                    End = new DateTime( 2016, 6, 30 ),
                    Course = course1,
                },
                module2 = new Module {
                    Id = newModuleId2,
                    Name = "Testmodul2",
                    Description = "Beskrivning av testmodul 2...",
                    Start = new DateTime( 2016, 7, 1 ),
                    End = new DateTime( 2016, 7, 31 ),
                    Course = course1,
                }
            );

            context.Activies.AddOrUpdate( a => a.Name,
                new Activity {
                    Id = newActivityId1,
                    Name = "Testmodul1",
                    Description = "Beskrivning av testaktivitet 1...",
                    Start = new DateTime( 2016, 6, 1, 8, 30, 0 ),
                    End = new DateTime( 2016, 6, 1, 17, 0, 0 ),
                    Module = module1,
                },
                new Activity {
                    Id = newActivityId2,
                    Name = "Testaktivitet2",
                    Description = "Beskrivning av testaktivitet 2...",
                    Start = new DateTime( 2016, 6, 2, 8, 30, 0 ),
                    End = new DateTime( 2016, 6, 2, 12, 0, 0 ),
                    Module = module1,
                },
                new Activity {
                    Id = newActivityId3,
                    Name = "Testaktivitet3",
                    Description = "Beskrivning av testaktivitet 3...",
                    Start = new DateTime( 2016, 6, 2, 13, 0, 0 ),
                    End = new DateTime( 2016, 6, 2, 17, 0, 0 ),
                    Module = module1,
                },
                new Activity {
                    Id = newActivityId4,
                    Name = "Testaktivitet4",
                    Description = "Beskrivning av testaktivitet 4...",
                    Start = new DateTime( 2016, 7, 1, 8, 30, 0 ),
                    End = new DateTime( 2016, 7, 1, 17, 0, 0 ),
                    Module = module2,
                }
            );
        }
    }
}
