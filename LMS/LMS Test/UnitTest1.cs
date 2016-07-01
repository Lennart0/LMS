using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LMS.Models;
using System.Data.Entity.Migrations;
using System.Collections.Generic;




using System;
using System.Data.Entity;
using System.Linq;
using System.IO;
using LMS.Helpers;

namespace LMS_Test {
    [TestClass]
    public class UnitTest1 {

        [TestMethod]
        public void CourseDaysTest() {
            var cd = new CourseDays( new DateTime( 2016, 3, 23 ), new DateTime( 2016, 4, 1 ) );

            var days = cd.Days;

            Assert.IsTrue( days.Count == 6 );
            if ( days.Count > 0 )
                Assert.AreEqual( days[0], new DateTime(2016,3,23) );
            if ( days.Count > 1 )
                Assert.AreEqual( days[1], new DateTime( 2016, 3, 24 ) );
            if ( days.Count > 2 )
                Assert.AreEqual( days[2], new DateTime( 2016, 3, 29 ) );
            if ( days.Count > 3 )
                Assert.AreEqual( days[3], new DateTime( 2016, 3, 30 ) );
            if ( days.Count > 4 )
                Assert.AreEqual( days[4], new DateTime( 2016, 3, 31 ) );
            if ( days.Count > 5 )
                Assert.AreEqual( days[5], new DateTime( 2016, 4, 1 ) );
        }



//        [TestMethod]
//        public void SeedTest() {


//            using (var context = new LMS.DataAccessLayer.ApplicationDbContext()) {
//                var y = context.Courses.ToList();
//                var course1 = new Course {
//                    Id = new Guid("d46f1dae-a53f-460d-8b89-a6c53753b4c2"),
//                    Name = "Modern burklax produktion",
//                    Description = "conservering och lax",
//                    Start = new DateTime(2016, 11, 3),
//                    End = new DateTime(2017, 4, 16)
//                };

//                var module1 = new Module {
//                    Id = new Guid("0cfbe895-4fc0-4d75-ac28-0ce2224ca117"),
//                    Name = "Conservering 101",
//                    Description = "Inläggning och konservering",
//                    Start = new DateTime(2016, 11, 4),
//                    End = new DateTime(2017, 2, 25),
                
//                };

//                var activity1 = new Activity {
//                    Id = new Guid("c186df12-1220-4c30-90e2-5bd095be3ac3"),
//                    Name = "förpackningar och burkar A",
//                    Start = new DateTime(2016, 11, 3, 8, 30, 0),
//                    End = new DateTime(2016, 11, 3, 17, 0, 0),
        
//                };


//                var activity2 = new Activity {
//                    Id = new Guid("4c1a93ad-b13c-48e2-a07d-a6f840c85d05"),
//                    Name = "förpackningar och burkar B",
//                    Start = new DateTime(2016, 11, 4, 8, 30, 0),
//                    End = new DateTime(2016, 11, 4, 17, 0, 0),
                   
//                };

//                var activity3 = new Activity {
//                    Id = new Guid("2bb0523c-ede0-478c-8d83-0d9aef58ca90"),
//                    Name = "Konservering ämnen",
//                    Start = new DateTime(2016, 11, 5, 8, 30, 0),
//                    End = new DateTime(2016, 11, 5, 17, 0, 0),
           
//                };
//                var activity4 = new Activity {
//                    Id = new Guid("d6760b11-13e0-40c2-b27f-9ab4bfda3242"),
//                    Name = "Konservering ämnen och matsäkerhet",
//                    Start = new DateTime(2016, 11, 6, 8, 30, 0),
//                    End = new DateTime(2016, 11, 6, 17, 0, 0),
            
//                };

//                context.Activies.AddOrUpdate(n => n.Id, activity1, activity2, activity3, activity4);
//                context.SaveChanges();
//                module1.Activities = new List<Activity> { activity1, activity2, activity3, activity4 };

//                var module2 = new Module {
//                    Id = new Guid("63e5cc14-3fff-48b5-b640-a0f5f792512b"),
//                    Name = "Fiskrensning och Hygien",
//                    Description = "Fiskrensning och Hygien",
//                    Start = new DateTime(2017, 2, 26),
//                    End = new DateTime(2017, 3, 11),

//                };
//                var module3 = new Module {
//                    Id = new Guid("2bccc925-b668-40b5-bad4-66676edcf2f2"),
//                    Name = "Automations system Grund",
//                    Description = "Grundläggande kurs i hur man jobbar med automation system för konsevburkar.",
//                    Start = new DateTime(2017, 3, 12),
//                    End = new DateTime(2017, 6, 26),

//                };
//                var module4 = new Module {
//                    Id = new Guid("bcae51d1-c44f-403e-a786-6a367bda8649"),
//                    Name = "Burklax 101",
//                    Description = "Burlax ABs förtags specifika kurs.",
//                    Start = new DateTime(2017, 6, 27),
//                    End = new DateTime(2017, 7, 13),

//                };

//                context.Modules.AddOrUpdate(p => p.Id, module1, module2, module3, module4);
//                context.SaveChanges();
//                course1.Modules = new List<Module> { module1, module2, module3, module4 };

//                var user = context.Users.FirstOrDefault(n => n.Id == "679a290d-8b3b-4488-8ffb-7dea7a44efca");


//                LMS.Helpers.DefaultUserAndRoleStartupHelper.Create();



//                var doc1 = new Document {
//                    Id = new Guid("f6128c63-d52c-4b70-a1a1-f7ca9040a044"),
//                    Type = DocumentType.Info,
//                    IsLocal = false,
//                    Url = "www.burklaxAB.se/kursInfo/intro.pdf",
//                    PublishDate = DateTime.Now,
//                    UploadDate = DateTime.Now,
//                    User = user,
//                    Activity = activity1
//                };
//  //              user.Documents.Add(doc1);
////                course1.Documents.Add(doc1);

//               // context.Documents.AddOrUpdate(n => n.Id, doc1);
//              //  context.SaveChanges();

//                context.Courses.AddOrUpdate(p => p.Id, course1);
//                context.SaveChanges();
         
//                var z = context.Courses.ToList();


//                Assert.IsTrue(z.Count > 0);
               
//            }
//        }
    }
}
