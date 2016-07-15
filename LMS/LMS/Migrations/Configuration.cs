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
        public Configuration() {
            AutomaticMigrationsEnabled = true;
        }
        void AddUser( LMS.DataAccessLayer.ApplicationDbContext context, UserManager<ApplicationUser> userMgr, string fullname, string email, string pw, string id, Guid? courseId ) {
            //if and only if default user is missing add user
            if ( context.Users.SingleOrDefault( n => n.Id == id ) == null ) {

                var defaultTeacher = new ApplicationUser() {
                    Email = email,
                    UserName = email,
                    Id = id,
                    FullName = fullname,
                    CourseId = courseId
                };


                var result = userMgr.Create( defaultTeacher, pw );
                if ( defaultTeacher?.Roles.Count == 0 && result.Succeeded == true ) {
                    if ( courseId == null )
                        userMgr.AddToRole( defaultTeacher.Id, Helpers.Constants.TeacherRole );
                }
            }

        }
        protected override void Seed( LMS.DataAccessLayer.ApplicationDbContext context ) {
            #region From DefaultUserAndRoleStartupHelper
            string userId = "679a290d-8b3b-4488-8ffb-7dea7a44efca";

            //if and only if role is empty add roll
            if ( context.Roles.Count( n => n.Name == Helpers.Constants.TeacherRole ) == 0 ) {
                context.Roles.Add( new IdentityRole( Helpers.Constants.TeacherRole ) );
                context.SaveChanges();
            }
            var userManager = new UserManager<ApplicationUser>( new UserStore<ApplicationUser>( context ) );

            AddUser( context, userManager, "Kalle Lärare", "Larar@lararson.se", "@ackN0w", userId, null );
            //var defaultTeacher = new ApplicationUser() {
            //    Email = "Larar@lararson.se",
            //    UserName = "Larar@lararson.se",
            //    Id = userId
            //};
            ////if and only if default user is missing add user
            //if ( context.Users.SingleOrDefault( n => n.Id == defaultTeacher.Id ) == null ) {
            //    var result = userManager.Create( defaultTeacher, "@ackN0w" );
            //    if ( defaultTeacher?.Roles.Count == 0 && result.Succeeded == true ) {
            //        userManager.AddToRole( defaultTeacher.Id, Helpers.Constants.TeacherRole );
            //    }
            //}
            #endregion
            AddUser( context, userManager, "Anders Svennson", "anders@demo.se", "1Password!", "679a290e-8b3b-4488-8ffb-7dea7a44efcb", null );

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

            AddUser( context, userManager, "Peter Andersson", "peter@demo.se", "1Password!", "679a290f-8b3b-4488-8ffb-7dea7a44efcc", newCourseId1 );
            AddUser( context, userManager, "Lennart", "lennart@demo.se", "1Password!", "679a290f-8b3b-4489-8ffb-7dea7a44efcf", newCourseId1 );


            ///////////////// Under uppbyggnad, tack! /////////////////////////////
            #region
            //var event10 = new List<Event>
            //{
            //new Event { Id = 1,
            //    Descriptipn = "Demo Sprint",
            //    StartDate   = new DateTime(2016, 7, 6, 8, 30, 0),
            //    EndDate     = new DateTime(2016, 7, 6, 12, 0, 0),
            //},
            //new Event{Id = 5,
            //    Descriptipn = "Lunch, yeah, yeah!!!",
            //    StartDate   = new DateTime(2016, 7, 6, 12, 0, 0),
            //    EndDate     = new DateTime(2016, 7, 6, 13, 0, 0),
            //    },
            //new Event{Id = 6,
            //    Descriptipn = "Slut Projekt",
            //    StartDate   = new DateTime(2016, 7, 6, 13, 0, 0),
            //    EndDate     = new DateTime(2016, 7, 6, 17, 0, 0),
            //    }
            //};
            //event10.ForEach( e => context.Events.AddOrUpdate( e ) );
            //context.SaveChanges();
            ////////////////////////////////////////////////////////////
            #endregion


            context.Modules.AddOrUpdate( m => m.Id,
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
            //Activity act4 = null;
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
                new Activity {
                    Id = newActivityId4,
                    Name = "Test-aktivitet4",
                    Description = "Beskrivning av test-aktivitet 4...",
                    Start = new DateTime( 2016, 7, 1, 8, 30, 0 ),
                    End = new DateTime( 2016, 7, 1, 17, 0, 0 ),
                //Module = module20,
                ModuleId = newModuleId2
                }
            );

            Guid demoCourseId = SeedDemoCourse( context, userId );

            #region Users Elever
            AddUser( context, userManager, "Andreas Carsbring", "andreasc@demo.se", "1Password!", "679a290f-8b3b-4488-8ffb-7dea7a44efcc", demoCourseId );
            AddUser( context, userManager, "Andreas Thyrhaug", "andreast@demo.se", "1Password!", "679a290f-8b3b-4489-8ffb-7dea7a44efcf", demoCourseId );
            AddUser( context, userManager, "Anette Tillbom", "anette@demo.se", "1Password!", "679a290f-8b3b-4488-8ffb-7dea7a44efab", demoCourseId );
            AddUser( context, userManager, "Ari Kylmänen", "ari@demo.se", "1Password!", "679a290f-8b3b-4489-8ffb-7dea7a44efsd", demoCourseId );
            AddUser( context, userManager, "Aryo Pehlewan", "aryo@demo.se", "1Password!", "679a290f-8b3b-4488-8ffb-7dea7a44effa", demoCourseId );
            AddUser( context, userManager, "Axel Räntilä", "axel@demo.se", "1Password!", "679a290f-8b3b-4489-8ffb-7dea7a44effe", demoCourseId );
            AddUser( context, userManager, "Bo Edström", "bo@demo.se", "1Password!", "679a290f-8b3b-4488-8ffb-7dea7a44efca", demoCourseId );
            AddUser( context, userManager, "Fernando Nilsson", "fernando@demo.se", "1Password!", "679a290f-8b3b-4489-8ffb-7dea7a44efba", demoCourseId );
            AddUser( context, userManager, "Fredrik Lindroth", "fredrik@demo.se", "1Password!", "679a290f-8b3b-4488-8ffb-7dea7a44efsa", demoCourseId );
            AddUser( context, userManager, "George Caspersson", "george@demo.se", "1Password!", "679a290f-8b3b-4489-8ffb-7dea7a44efas", demoCourseId );
            AddUser( context, userManager, "Helen Magnusson", "helen@demo.se", "1Password!", "679a290f-8b3b-4488-8ffb-7dea7a44easd", demoCourseId );
            AddUser( context, userManager, "Johan Haak", "johan@demo.se", "1Password!", "679a290f-8b3b-4489-8ffb-7dea7a44efwe", demoCourseId );
            AddUser( context, userManager, "John Castell", "john@demo.se", "1Password!", "679a290f-8b3b-4488-8ffb-7dea7a44edsa", demoCourseId );
            AddUser( context, userManager, "Karl Lindström", "karl@demo.se", "1Password!", "679a290f-8b3b-4489-8ffb-7dea7a44ebds", demoCourseId );
            AddUser( context, userManager, "Marie Hansson", "marie@demo.se", "1Password!", "679a290f-8b3b-4488-8ffb-7dea7a44esdf", demoCourseId );
            AddUser( context, userManager, "Michael Novak", "michael@demo.se", "1Password!", "679a290f-8b3b-4489-8ffb-7dea7a44efdb", demoCourseId );
            AddUser( context, userManager, "Thomas J Ekman", "thomase@demo.se", "1Password!", "679a290f-8b3b-4488-8ffb-7dea7a44eaaf", demoCourseId );
            AddUser( context, userManager, "Thomas Sundblom", "thomass@demo.se", "1Password!", "679a290f-8b3b-4489-8ffb-7dea7a44esdj", demoCourseId );
            AddUser( context, userManager, "Tomas Santana", "tomas@demo.se", "1Password!", "679a290f-8b3b-4488-8ffb-7dea7a44efff", demoCourseId );
            AddUser( context, userManager, "Wasim Randhawa", "wasin@demo.se", "1Password!", "679a290f-8b3b-4489-8ffb-7dea7a44eebn", demoCourseId );
            AddUser( context, userManager, "Yaser Mosavi", "yeser@demo.se", "1Password!", "679a290f-8b3b-4489-8ffb-7dfa7a44easb", demoCourseId );

            #endregion

            string guidBase = "679a290d-8b3b-4488-8ffb-7dcd7a44ef0";

            for ( int i = 0; i < 0x10; i++ ) {
                context.Activies.AddOrUpdate( a => a.Id,
                    new Activity {
                        Id = new Guid( guidBase + i.ToString( "x" ) ),
                        Name = "Test-aktivitet" + i,
                        Description = "Beskrivning av test-aktivitet " + i,
                        Start = new DateTime( 2016, 7, i + 2, 8, 30, 0 ),
                        End = new DateTime( 2016, 7, i + 2, 17, 0, 0 ),
                        ModuleId = newModuleId2
                    }
                );
            }

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
                    PublishDate = DateTime.Now - new TimeSpan( 1, 0, 0, 0 ),
                    Url = "http://www.filelocation.com/somefile.pdf",
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
                    assignmentId = newDocId2,
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

        Guid SeedDemoCourse( LMS.DataAccessLayer.ApplicationDbContext context, string uploaderId ) {
            Guid demoCourse1Id = new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a44ef01" );

            Guid[] guid = {
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440000" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440001" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440002" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440003" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440004" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440005" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440006" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440007" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440008" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440009" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440010" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440011" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440012" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440013" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440014" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440015" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440016" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440017" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440018" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440019" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440020" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440021" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440022" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440023" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440024" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440025" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440026" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440027" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440028" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440029" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440030" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440031" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440032" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440033" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440034" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440035" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440036" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440037" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440038" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7a440039" ),
            };

            Guid[] docGuid = {
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7b450000" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7b450001" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7b450002" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7b450003" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7b450004" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7b450005" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7b450006" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7b450007" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7b450008" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7b450009" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7b450010" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7b450011" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7b450012" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7b450013" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7b450014" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7b450015" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7b450016" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7b450017" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7b450018" ),
                new Guid( "679bcd0d-8b3b-4488-8ffb-7dea7b450019" ),
            };


            var days = new Helpers.CourseDays( new DateTime( 2016, 7, 1 ), new DateTime( 2016, 7, 29 ) ).Days;
            Course demoCourse1 = null;
            context.Courses.AddOrUpdate( c => c.Id,
                demoCourse1 = new Course {
                    Id = demoCourse1Id,
                    Name = ".Net Webbutveckling MVC",
                    Description = ".Net systemutvecklingskurs. Lorem ipsum dolor sit amet, consectetur adipiscing elit.Nulla ultricies justo eu suscipit dapibus.Curabitur pharetra ex in enim laoreet vestibulum.Aliquam sollicitudin id dui sit amet elementum.Proin ullamcorper in ligula in cursus.Donec ultrices libero vel neque vehicula blandit.Proin rutrum ante quis maximus pulvinar.Morbi hendrerit et arcu nec venenatis.Cras id nulla quis quam porta posuere.Cras aliquet tellus eu odio pulvinar sodales.Curabitur ac auctor erat, a tempor erat.Nullam aliquam nulla quis quam placerat, ut tempus massa tristique.Fusce facilisis risus in massa mattis venenatis in sed ex.Nullam consequat arcu ac sapien mollis egestas.",
                    Start = new DateTime( 2016, 7, 1 ),
                    End = new DateTime( 2016, 7, 29 ),
                    DayStart = new DateTime( 2016, 7, 1, 9, 0, 0 ),
                    DayEnd = new DateTime( 2016, 7, 1, 17, 0, 0 ),
                    LunchStart = new DateTime( 2016, 7, 1, 12, 0, 0 ),
                    LunchEnd = new DateTime( 2016, 7, 1, 13, 0, 0 ),
                }
            );


            string[] moduleName = { "C#", "MVC", "Entity Framework", "Javascript" };
            DateTime[] moduleStart = { new DateTime( 2016, 7, 1 ), new DateTime( 2016, 7, 11 ), new DateTime( 2016, 7, 18 ), new DateTime( 2016, 7, 25 ) };
            DateTime[] moduleEnd = { new DateTime( 2016, 7, 8 ), new DateTime( 2016, 7, 15 ), new DateTime( 2016, 7, 22 ), new DateTime( 2016, 7, 29 ) };
            for ( int i = 0; i < 4; i++ ) {
                context.Modules.AddOrUpdate( m => m.Id,
                    new Module {
                        Id = guid[i],
                        Name = moduleName[i],
                        Description = moduleName[i] + " " + "Curabitur pharetra ex in enim laoreet vestibulum.Aliquam sollicitudin id dui sit amet elementum.Proin ullamcorper in ligula in cursus.Donec ultrices libero vel neque vehicula blandit.Proin rutrum ante quis maximus pulvinar.Morbi hendrerit et arcu nec venenatis.Cras id nulla quis quam porta posuere.Cras aliquet tellus eu odio pulvinar sodales.Curabitur ac auctor erat, a tempor erat.Nullam aliquam nulla quis quam placerat, ut tempus massa tristique.Fusce facilisis risus in massa mattis venenatis in sed ex.Nullam consequat arcu ac sapien mollis egestas.",
                        Start = moduleStart[i],
                        End = moduleEnd[i],
                        CourseId = demoCourse1Id
                    }
                );
            }


            string[] actName = { "Föreläsning", "E-Learning", "Övning", "Föreläsning", "E-Learning" };
            int day = 0;
            int module = 0;
            int docId = 0;
            for ( int i = 4; i < 40; i++ ) {
                if ( days[day] > moduleEnd[module] )
                    module = (module + 1) % 4;
                context.Activies.AddOrUpdate( a => a.Id,
                    new Activity {
                        Id = guid[i],
                        Name = actName[i % actName.Length] + " " + moduleName[module],
                        Description = "Aliquam sollicitudin id dui sit amet elementum.Proin ullamcorper in ligula in cursus.Donec ultrices libero vel neque vehicula blandit.Proin rutrum ante quis maximus pulvinar.Morbi hendrerit et arcu nec venenatis.Cras id nulla quis quam porta posuere.Cras aliquet tellus eu odio pulvinar sodales.Curabitur ac auctor erat, a tempor erat.Nullam aliquam nulla quis quam placerat, ut tempus massa tristique.Fusce facilisis risus in massa mattis venenatis in sed ex.Nullam consequat arcu ac sapien mollis egestas.",
                        Start = days[day] + (i % 3 != 2 ? new TimeSpan( 9, 0, 0 ) : new TimeSpan( 13, 0, 0 )),
                        End = days[day] + (i % 3 != 1 ? new TimeSpan( 17, 0, 0 ) : new TimeSpan( 12, 0, 0 )),
                        ModuleId = guid[module]
                    }
                );

                if (actName[i % actName.Length] == "Övning" ) {
                    context.Documents.AddOrUpdate( d => d.Id,
                        new TimeSensetiveDocument {
                            Id = docGuid[docId++],
                            Type = DocumentType.Upgift,
                            //Activity = act2,
                            ActivityId = guid[i],
                            Url = "/Documents/Activity/Övning 11- MVC - Personalregister.pdf",
                            PublishDate = days[day],
                            DeadLine = day < days.Count - 2 ? days[day + 2] : days[days.Count - 1],
                            UploadDate = new DateTime( 2016, 1, 1 ),
                            IsLocal = false,
                            UserId = uploaderId,
                        }
                    );
                }

                if ( i % 3 != 1 )
                    day++;
                if ( day >= days.Count )
                    break;
            }
            return demoCourse1Id;
        }
    }
}
