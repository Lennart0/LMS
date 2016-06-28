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
            Guid newCourseId1 = new Guid( "679a290d-8b3b-4488-8ffb-7dea7a44ef01" );
            Guid newModuleId1 = new Guid( "679a290d-8b3b-4488-8ffb-7dea7a44ef02" );
            Guid newModuleId2 = new Guid( "679a290d-8b3b-4488-8ffb-7dea7a44ef03" );
            Guid newActivityId1 = new Guid( "679a290d-8b3b-4488-8ffb-7dea7a44ef04" );
            Guid newActivityId2 = new Guid( "679a290d-8b3b-4488-8ffb-7dea7a44ef05" );
            Guid newActivityId3 = new Guid( "679a290d-8b3b-4488-8ffb-7dea7a44ef06" );
            Guid newActivityId4 = new Guid( "679a290d-8b3b-4488-8ffb-7dea7a44ef07" );

            Course course1 = null;
            Module module1 = null;
            Module module2 = null;

            context.Courses.AddOrUpdate( c => c.Id,
                course1 = new Course {
                    Id = newCourseId1,
                    Name = "Test-kurs1",
                    Description = "Beskrivning av test-kurs 1...",
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

            context.Modules.AddOrUpdate( m => m.Id,
                module1 = new Module {
                    Id = newModuleId1,
                    Name = "Test-modul1",
                    Description = "Beskrivning av test-modul 1...",
                    Start = new DateTime( 2016, 6, 1 ),
                    End = new DateTime( 2016, 6, 30 ),
                    Course = course1,
                },
                module2 = new Module {
                    Id = newModuleId2,
                    Name = "Test-modul2",
                    Description = "Beskrivning av test-modul 2...",
                    Start = new DateTime( 2016, 7, 1 ),
                    End = new DateTime( 2016, 7, 31 ),
                    Course = course1,
                }
            );

            context.Activies.AddOrUpdate( a => a.Id,
                new Activity {
                    Id = newActivityId1,
                    Name = "Test-aktivitet1",
                    Description = "Beskrivning av test-aktivitet 1...",
                    Start = new DateTime( 2016, 6, 1, 8, 30, 0 ),
                    End = new DateTime( 2016, 6, 1, 17, 0, 0 ),
                    Module = module1,
                },
                new Activity {
                    Id = newActivityId2,
                    Name = "Test-aktivitet2",
                    Description = "Beskrivning av test-aktivitet 2...",
                    Start = new DateTime( 2016, 6, 2, 8, 30, 0 ),
                    End = new DateTime( 2016, 6, 2, 12, 0, 0 ),
                    Module = module1,
                },
                new Activity {
                    Id = newActivityId3,
                    Name = "Test-aktivitet3",
                    Description = "Beskrivning av test-aktivitet 3...",
                    Start = new DateTime( 2016, 6, 2, 13, 0, 0 ),
                    End = new DateTime( 2016, 6, 2, 17, 0, 0 ),
                    Module = module1,
                },
                new Activity {
                    Id = newActivityId4,
                    Name = "Test-aktivitet4",
                    Description = "Beskrivning av test-aktivitet 4...",
                    Start = new DateTime( 2016, 7, 1, 8, 30, 0 ),
                    End = new DateTime( 2016, 7, 1, 17, 0, 0 ),
                    Module = module2,
                }
            );
        }
    }
}
