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

        protected override void Seed(LMS.DataAccessLayer.ApplicationDbContext context)
        {
            var courseGuid = new Guid("91838d6c-ec99-4b97-b930-ea99d3e52967");


            if (context.Courses.Count(n=> n.Id == courseGuid)==0) {
                var y = context.Courses.ToList();
                var course1 = new Course {
                    Id = courseGuid,
                    Name = "Modern burklax produktion",
                    Description = "conservering och lax",
                    Start = new DateTime(2016, 11, 3),
                    End = new DateTime(2017, 4, 16)
                };

                var module1 = new Module {
                    Id = new Guid("cb817e0c-4df2-490a-a188-4140c9b07ca5"),
                    Name = "Conservering 101",
                    Description = "Inl�ggning och konservering",
                    Start = new DateTime(2016, 11, 4),
                    End = new DateTime(2017, 2, 25),

                };

                var activity1 = new Activity {
                    Id = new Guid("c186df12-1220-4c30-90e2-5bd095be3ac3"),
                    Name = "f�rpackningar och burkar A",
                    Start = new DateTime(2016, 11, 3, 8, 30, 0),
                    End = new DateTime(2016, 11, 3, 17, 0, 0),

                };


                var activity2 = new Activity {
                    Id = new Guid("40de38aa-ff3f-4d45-b8df-b7c1cb81c147"),
                    Name = "f�rpackningar och burkar B",
                    Start = new DateTime(2016, 11, 4, 8, 30, 0),
                    End = new DateTime(2016, 11, 4, 17, 0, 0),

                };

                var activity3 = new Activity {
                    Id = new Guid("08952d1c-f8ea-43db-a2b1-bff489acd9a7"),
                    Name = "Konservering �mnen",
                    Start = new DateTime(2016, 11, 5, 8, 30, 0),
                    End = new DateTime(2016, 11, 5, 17, 0, 0),

                };
                var activity4 = new Activity {
                    Id = new Guid("d6760b11-13e0-40c2-b27f-9ab4bfda3242"),
                    Name = "Konservering �mnen och mats�kerhet",
                    Start = new DateTime(2016, 11, 6, 8, 30, 0),
                    End = new DateTime(2016, 11, 6, 17, 0, 0),

                };

                //    context.Activies.AddOrUpdate(n => n.Id, activity1, activity2, activity3, activity4);
                //    context.SaveChanges();
                module1.Activities = new List<Activity> { activity1, activity2, activity3, activity4 };

                var module2 = new Module {
                    Id = new Guid("63e5cc14-3fff-48b5-b640-a0f5f792512b"),
                    Name = "Fiskrensning och Hygien",
                    Description = "Fiskrensning och Hygien",
                    Start = new DateTime(2017, 2, 26),
                    End = new DateTime(2017, 3, 11),

                };
                var module3 = new Module {
                    Id = new Guid("2bccc925-b668-40b5-bad4-66676edcf2f2"),
                    Name = "Automations system Grund",
                    Description = "Grundl�ggande kurs i hur man jobbar med automation system f�r konsevburkar.",
                    Start = new DateTime(2017, 3, 12),
                    End = new DateTime(2017, 6, 26),

                };
                var module4 = new Module {
                    Id = new Guid("6bc0323b-a2f1-44eb-8ef3-e56605c3c743"),
                    Name = "Burklax 101",
                    Description = "Burlax ABs f�rtags specifika kurs.",
                    Start = new DateTime(2017, 6, 27),
                    End = new DateTime(2017, 7, 13),

                };

                // context.Modules.AddOrUpdate(p => p.Id, module1, module2, module3, module4);
                //context.SaveChanges();
                course1.Modules = new List<Module> { module1, module2, module3, module4 };

                var user = context.Users.FirstOrDefault(n => n.Id == "679a290d-8b3b-4488-8ffb-7dea7a44efca");


                LMS.Helpers.DefaultUserAndRoleStartupHelper.Create();



                var doc1 = new Document {
                    Id = new Guid("f6128c63-d52c-4b70-a1a1-f7ca9040a044"),
                    Type = DocumentType.Info,
                    IsLocal = false,
                    Url = "www.burklaxAB.se/kursInfo/intro.pdf",
                    PublishDate = DateTime.Now,
                    UploadDate = DateTime.Now,
                    User = user,
                    Activity = activity1
                };


                //   context.Documents.AddOrUpdate(n => n.Id, doc1);
                //  context.SaveChanges();
                user.Documents.Add(doc1);
                course1.Documents.Add(doc1);




                context.Courses.AddOrUpdate(p => p.Id, course1);
                context.SaveChanges();

            }

        }
        }
}
