namespace LMS.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Models;
    using System;
    using System.Collections.Generic;
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
            string userId = "679a290d-8b3b-4488-8ffb-7dea7a44efca";


            #region From DefaultUserAndRoleStartupHelper
            //if and only if role is empty add roll
            if ( context.Roles.Count( n => n.Name == Helpers.Constants.TeacherRole ) == 0 ) {
                context.Roles.Add( new IdentityRole( Helpers.Constants.TeacherRole ) );
                context.SaveChanges();
            }
            var userManager = new UserManager<ApplicationUser>( new UserStore<ApplicationUser>( context ) );
        
            var defaultTeacher = new ApplicationUser() {
                Email = "Larar@lararson.se",
                UserName = "Larar@lararson.se",
                Id = userId
            };
            //if and only if default user is missing add user
            if ( context.Users.SingleOrDefault( n => n.Id == defaultTeacher.Id ) == null ) {
                var result = userManager.Create( defaultTeacher, "@ackN0w" );
                if ( defaultTeacher?.Roles.Count == 0 && result.Succeeded == true ) {
                    userManager.AddToRole( defaultTeacher.Id, Helpers.Constants.TeacherRole );
                }
            }
            #endregion


            //:Todo Figure out later where to put the Role seeding as startup.cs does not seam to have access to the context class...
            Guid newCourseId1 = new Guid( "679a290d-8b3b-4488-8ffb-7dea7a44ef01" );
            Guid newModuleId1 = new Guid( "679a290d-8b3b-4488-8ffb-7dea7a44ef02" );
            Guid newModuleId2 = new Guid( "679a290d-8b3b-4488-8ffb-7dea7a44ef03" );
            Guid newActivityId1 = new Guid( "679a290d-8b3b-4488-8ffb-7dea7a44ef04" );
            Guid newActivityId2 = new Guid( "679a290d-8b3b-4488-8ffb-7dea7a44ef05" );
            Guid newActivityId3 = new Guid( "679a290d-8b3b-4488-8ffb-7dea7a44ef06" );
            Guid newActivityId4 = new Guid( "679a290d-8b3b-4488-8ffb-7dea7a44ef07" );

            Course course10 = null;
            Module module10 = null;
            Module module20 = null;

            context.Courses.AddOrUpdate( c => c.Id,
                course10 = new Course {
                    Id = newCourseId1,
                    Name = "Test-kurs1",
                    Description = "Beskrivning av test-kurs 1...",
                    Start = new DateTime( 2016, 6, 1 ),
                    End = new DateTime( 2016, 9, 30 ),
                    DayStart = new DateTime( 2016, 6, 1, 8, 30, 0 ),
                    DayEnd = new DateTime( 2016, 6, 1, 17, 0, 0 ),
                    LunchStart = new DateTime( 2016, 6, 1, 12, 0, 0 ),
                    LunchEnd = new DateTime( 2016, 6, 1, 13, 0, 0 ),
                }
            );


        context.Modules.AddOrUpdate( m => m.Name,
            module10 = new Module {
                Id = newModuleId1,
                Name = "Testmodul1",
                Description = "Beskrivning av testmodul 1...",
                Start = new DateTime( 2016, 6, 1 ),
                End = new DateTime( 2016, 6, 30 ),
                //Course = course10,
                CourseId = newCourseId1
            },
            module20 = new Module {
                Id = newModuleId2,
                Name = "Testmodul2",
                Description = "Beskrivning av testmodul 2...",
                Start = new DateTime( 2016, 7, 1 ),
                End = new DateTime( 2016, 7, 31 ),
                //Course = course10,
                CourseId = newCourseId1
            }
        );


        Activity act1 = null;
        Activity act2 = null;
        Activity act3 = null;
        Activity act4 = null;
        context.Activies.AddOrUpdate( a => a.Id,
            act1 = new Activity {
                Id = newActivityId1,
                Name = "Test-aktivitet1",
                Description = "Beskrivning av test-aktivitet 1...",
                Start = new DateTime( 2016, 6, 1, 8, 30, 0 ),
                End = new DateTime( 2016, 6, 1, 17, 0, 0 ),
                //Module = module10,
                ModuleId = newModuleId1
            },
            act2 = new Activity {
                Id = newActivityId2,
                Name = "Test-aktivitet2",
                Description = "Beskrivning av test-aktivitet 2...",
                Start = new DateTime( 2016, 6, 2, 8, 30, 0 ),
                End = new DateTime( 2016, 6, 2, 12, 0, 0 ),
                //Module = module10,
                ModuleId = newModuleId1
            },
            act3 = new Activity {
                Id = newActivityId3,
                Name = "Test-aktivitet3",
                Description = "Beskrivning av test-aktivitet 3...",
                Start = new DateTime( 2016, 6, 2, 13, 0, 0 ),
                End = new DateTime( 2016, 6, 2, 17, 0, 0 ),
                //Module = module10,
                ModuleId = newModuleId1
            },
            act4 = new Activity {
                Id = newActivityId4,
                Name = "Test-aktivitet4",
                Description = "Beskrivning av test-aktivitet 4...",
                Start = new DateTime( 2016, 7, 1, 8, 30, 0 ),
                End = new DateTime( 2016, 7, 1, 17, 0, 0 ),
                //Module = module20,
                ModuleId = newModuleId2
            }
        );


        Guid newDocId1 = new Guid( "679a290d-8b3b-4488-8ffb-7dea7a44bfb4" );
        Guid newDocId2 = new Guid( "679a290d-8b3b-4488-8ffb-7dea7a44bfb5" );
        Guid newDocId3 = new Guid( "679a290d-8b3b-4488-8ffb-7dea7a44bfb6" );
        Guid newDocId4 = new Guid( "679a290d-8b3b-4488-8ffb-7dea7a44bfb7" );

        //user = context.Users.FirstOrDefault( n => n.Id == "679a290d-8b3b-4488-8ffb-7dea7a44efca" );

            context.Documents.AddOrUpdate( d => d.Id,
                new PlainDocument {
                    Id = newDocId1,
                    Type = DocumentType.Upgift,
                    //Activity = act1,
                    ActivityId = newActivityId1,
                    PublishDate = DateTime.Now - new TimeSpan(1,0,0,0),
                    Url="http://www.filelocation.com/somefile.pdf",
                    UploadDate = new DateTime( 2016, 1, 1 ),
                    IsLocal = false,
                    UserId = userId,
                    //User = user
                },
                new TimeSensetiveDocument {
                    Id = newDocId2,
                    Type = DocumentType.Upgift,
                    //Activity = act2,
                    ActivityId = newActivityId1,
                    Url = "http://www.filelocation.com/SomeTrainingExcersize.pdf",
                    PublishDate = DateTime.Now + new TimeSpan( 1, 0, 0, 0 ),
                    DeadLine = (DateTime.Now + new TimeSpan( 1, 0, 0, 0 )).Date + new TimeSpan( 5, 0, 0, 0 ),
                    UploadDate = new DateTime( 2016, 1, 1 ),
                    IsLocal = false,
                    UserId = userId,
                    //User = user
                },
                new AssignmentSubmission {
                    Id = newDocId3,
                    Type = DocumentType.Upgift,
                    //Activity = act3,
                    Url = "http://www.filelocation.com/PersonX_Answer.zip",
                    ActivityId = newActivityId1,
                    PublishDate = DateTime.Now - new TimeSpan( 1, 0, 0, 0 ),
                    assignmentId=newDocId2,
                    UploadDate = new DateTime( 2016, 1, 1 ),
                    IsLocal = false,
                    UserId = userId,
                     
                    //User = user
                },
                new PlainDocument {
                    Id = newDocId4,
                    Type = DocumentType.Upgift,
                    //Activity = act4,
                    ActivityId = newActivityId1,
                    PublishDate = DateTime.Now - new TimeSpan( 1, 0, 0, 0 ),
                   
                    UploadDate = new DateTime( 2016, 1, 1 ),
                    IsLocal = false,
                    UserId = userId,
                    //User = user
                }
            );

           
            /********************************************/

            var courseGuid = new Guid( "91838d6c-ec99-4b97-b930-ea99d3e52967" );

            if ( context.Courses.Count( n => n.Id == courseGuid ) == 0 ) {
                var y = context.Courses.ToList();
                var course1 = new Course {
                    Id = courseGuid,
                    Name = "Modern burklax produktion",
                    Description = "conservering och lax",
                    Start = new DateTime( 2016, 11, 3 ),
                    End = new DateTime( 2017, 4, 16 )
                };

                var module1 = new Module {
                    Id = new Guid( "cb817e0c-4df2-490a-a188-4140c9b07ca5" ),
                    Name = "Conservering 101",
                    Description = "Inläggning och konservering",
                    Start = new DateTime( 2016, 11, 4 ),
                    End = new DateTime( 2017, 2, 25 ),
                };

                var activity1 = new Activity {
                    Id = new Guid( "c186df12-1220-4c30-90e2-5bd095be3ac3" ),
                    Name = "förpackningar och burkar A",
                    Start = new DateTime( 2016, 11, 3, 8, 30, 0 ),
                    End = new DateTime( 2016, 11, 3, 17, 0, 0 ),

                };


                var activity2 = new Activity {
                    Id = new Guid( "40de38aa-ff3f-4d45-b8df-b7c1cb81c147" ),
                    Name = "förpackningar och burkar B",
                    Start = new DateTime( 2016, 11, 4, 8, 30, 0 ),
                    End = new DateTime( 2016, 11, 4, 17, 0, 0 ),

                };

                var activity3 = new Activity {
                    Id = new Guid( "08952d1c-f8ea-43db-a2b1-bff489acd9a7" ),
                    Name = "Konservering ämnen",
                    Start = new DateTime( 2016, 11, 5, 8, 30, 0 ),
                    End = new DateTime( 2016, 11, 5, 17, 0, 0 ),

                };
                var activity4 = new Activity {
                    Id = new Guid( "d6760b11-13e0-40c2-b27f-9ab4bfda3242" ),
                    Name = "Konservering ämnen och matsäkerhet",
                    Start = new DateTime( 2016, 11, 6, 8, 30, 0 ),
                    End = new DateTime( 2016, 11, 6, 17, 0, 0 ),

                };

                //    context.Activies.AddOrUpdate(n => n.Id, activity1, activity2, activity3, activity4);
                //    context.SaveChanges();
                module1.Activities = new List<Activity> { activity1, activity2, activity3, activity4 };

                var module2 = new Module {
                    Id = new Guid( "63e5cc14-3fff-48b5-b640-a0f5f792512b" ),
                    Name = "Fiskrensning och Hygien",
                    Description = "Fiskrensning och Hygien",
                    Start = new DateTime( 2017, 2, 26 ),
                    End = new DateTime( 2017, 3, 11 ),

                };
                var module3 = new Module {
                    Id = new Guid( "2bccc925-b668-40b5-bad4-66676edcf2f2" ),
                    Name = "Automations system Grund",
                    Description = "Grundläggande kurs i hur man jobbar med automation system för konsevburkar.",
                    Start = new DateTime( 2017, 3, 12 ),
                    End = new DateTime( 2017, 6, 26 ),

                };
                var module4 = new Module {
                    Id = new Guid( "6bc0323b-a2f1-44eb-8ef3-e56605c3c743" ),
                    Name = "Burklax 101",
                    Description = "Burlax ABs förtags specifika kurs.",
                    Start = new DateTime( 2017, 6, 27 ),
                    End = new DateTime( 2017, 7, 13 ),

                };

                // context.Modules.AddOrUpdate(p => p.Id, module1, module2, module3, module4);
                //context.SaveChanges();
                course1.Modules = new List<Module> { module1, module2, module3, module4 };

                //user = context.Users.FirstOrDefault( n => n.Id == "679a290d-8b3b-4488-8ffb-7dea7a44efca" );


                LMS.Helpers.DefaultUserAndRoleStartupHelper.Create();



                var doc1 = new PlainDocument {
                    Id = new Guid( "f6128c63-d52c-4b70-a1a1-f7ca9040a044" ),
                    Type = DocumentType.Info,
                    IsLocal = false,
                    Url = "www.burklaxAB.se/kursInfo/intro.pdf",
                    PublishDate = DateTime.Now,
                    UploadDate = DateTime.Now,
                    UserId = userId,
                    //User = user,
                    //Activity = activity1
                    ActivityId = activity1.Id
                };


                //   context.Documents.AddOrUpdate(n => n.Id, doc1);
                //  context.SaveChanges();
                //if ( user != null ) user.Documents.Add( doc1 );
                //activity1.Documents.Add( doc1 );


                //:Todo Figure out later where to put the Role seeding as startup.cs does not seam to have access to the context class...


                context.Courses.AddOrUpdate( p => p.Id, course1 );
                context.SaveChanges();

            }


            /***************************************************************/


        }
    }
}
