using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMS.Models;

namespace LMS.Controllers {
    [Authorize]
    public class DocumentController : Controller {
        private DataAccessLayer.ApplicationDbContext db = new DataAccessLayer.ApplicationDbContext();

        // GET: Document
        public ActionResult Add(Guid EntityId, Models.DocumentTargetEntity entityType) {

            var entityName = "";
            switch (entityType) {
                case Models.DocumentTargetEntity.User:
                    entityName = db.Users.FirstOrDefault(n => n.Id == EntityId.ToString()).UserName;
                    break;
                case Models.DocumentTargetEntity.Activity:
                    entityName = db.Activies.FirstOrDefault(n => n.Id == EntityId).Name;
                    break;
                case Models.DocumentTargetEntity.Module:
                    entityName = db.Modules.FirstOrDefault(n => n.Id == EntityId).Name;
                    break;
                case Models.DocumentTargetEntity.Course:
                    entityName = db.Courses.FirstOrDefault(n => n.Id == EntityId).Name;
                    break;
                default:
                    entityName = "";
                    break;
            }

            var user = db.Users.First(n => n.Email == User.Identity.Name);

            return View(new Models.AddDocumentsViewModel {
                EntityId = EntityId,
                EntityName = entityName,
                EntityType = entityType,
                Done = false,
                ComboItems = LMS.Models.ComboBoxListItemHelper.GetOptions(typeof(Models.DocumentSelectionMechanic)),
                Items = new List<Models.DocumentItem>() {
                    // new Models.DocumentItem { Owner = user.FullName, SelectionMechanic= Models.DocumentSelectionMechanic.File, RequiresUpload = true },
                  //   new Models.DocumentItem { Owner = user.FullName, SelectionMechanic= Models.DocumentSelectionMechanic.InternalUrlLookup, RequiresUpload = true },
                     new Models.DocumentItem { Owner = user.FullName, SelectionMechanic= Models.DocumentSelectionMechanic.Url, RequiresUpload = true }
                }
            });
        }

        [HttpPost]
        public ActionResult Add(Models.AddDocumentsViewModel model) {
            var user = db.Users.First(n => n.Email == User.Identity.Name);


            if (!model.Done) {
                model.Items.Add(new Models.DocumentItem { SelectionMechanic = model.SelectionMechanic.Value, RequiresUpload = true });
            } else {
                foreach (var item in model.Items) {
                    switch (item.SelectionMechanic) {
                        case DocumentSelectionMechanic.File:
                            ForEachFile(item, model, user);
                            break;
                        case DocumentSelectionMechanic.InternalUrlLookup:
                            ForEachUrlLocal(item, model, user);
                            break;
                        case DocumentSelectionMechanic.Url:
                            ForEachUrl(item, model, user);
                            break;
                    }

                }
            }

            model.ComboItems = LMS.Models.ComboBoxListItemHelper.GetOptions(typeof(Models.DocumentSelectionMechanic));
            return View(model);
        }

        private void ForEachUrl(DocumentItem item, AddDocumentsViewModel model, ApplicationUser user) {

            Activity activity = null;
            Module module = null;
            Course course = null;

            //hmmmmm.... only seam to be important to do activity here one sided ralationship here i think..
            switch (model.EntityType) {
                case DocumentTargetEntity.Activity:
                    activity = db.Activies.FirstOrDefault(n => n.Id == model.EntityId);
                    break;
                case DocumentTargetEntity.Module:
                    module = db.Modules.FirstOrDefault(n => n.Id == model.EntityId);                
                    break;
                case DocumentTargetEntity.Course:
                    course = db.Courses.FirstOrDefault(n => n.Id == model.EntityId);
                    break;
            }


            db.Documents.Add(new Document {
                Id = Guid.NewGuid(),
                IsLocal = false,
                Activity = activity,
                User = user,
                Url = item.URL,
                UploadDate = DateTime.Now,
                PublishDate = DateTime.Now //tempoarary we need to rethink document... i think it should be a IEntity

            });
            db.SaveChanges();
        }

        private void ForEachUrlLocal(DocumentItem item, AddDocumentsViewModel model, ApplicationUser user) {

            ForEachUrl(item, model, user);
        }

        private void ForEachFile(DocumentItem item, AddDocumentsViewModel model, ApplicationUser user) {
            var filePath = $"Documents\\{model.EntityType}\\{item.File.FileName}";
            if (System.IO.File.Exists(filePath)) {
                 filePath = $"Documents\\{model.EntityType}\\{Guid.NewGuid()}_{item.File.FileName}";
                item.File.SaveAs(filePath);
                item.URL = filePath;
                ForEachUrl(item, model, user);
            } else {
                item.File.SaveAs(filePath);
                item.URL = filePath;
                ForEachUrl(item, model, user);
            }
        }



        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
        }
    }
}