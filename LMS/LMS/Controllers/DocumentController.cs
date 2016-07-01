using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMS.Models;
using System.Data.Entity;
namespace LMS.Controllers {
    [Authorize]
    public class DocumentController : Controller {
        private DataAccessLayer.ApplicationDbContext db = new DataAccessLayer.ApplicationDbContext();

        // GET: Document
        public ActionResult Add(Guid EntityId, Models.DocumentTargetEntity entityType) {
            var user = db.Users.First(n => n.Email == User.Identity.Name);
            List<DocumentItem> items = new List<DocumentItem>();
            var entityName = "";
            switch (entityType) {
                case Models.DocumentTargetEntity.User:
                    entityName = db.Users.FirstOrDefault(n => n.Id == EntityId.ToString()).UserName;
                    items = 
                        userPrivlageFileter(
                            db.Documents.Where(n => n.User.Id == EntityId.ToString()) , user)
                            .Select(n => new DocumentItem() {
                                URL = n.Url,
                                RequiresUpload =false,
                                SelectionMechanic = DocumentSelectionMechanic.Url,
                                Owner = n.User.UserName,
                                Status = DocumentStatus.Yellow,
                                StatusText ="test",
                             PublishDate= DateTime.Now,
                             Feedback="",
                             DeadLine=null,
                             HasDeadline=false,
                             DocumentDbId = n.Id}).ToList();
                    break;
                case Models.DocumentTargetEntity.Activity:
                    entityName = db.Activies.FirstOrDefault(n => n.Id == EntityId).Name;
                    items =
                     userPrivlageFileter(
                         db.Documents.Where(n => n.Activity.Id == EntityId), user)
                         .Select(n => new DocumentItem() {
                             URL = n.Url,
                             RequiresUpload = false,
                             SelectionMechanic = DocumentSelectionMechanic.Url,
                             Owner = n.User.UserName,
                             Status = DocumentStatus.Yellow,
                             StatusText = "test",
                             PublishDate = DateTime.Now,
                             Feedback = "",
                             DeadLine = null,
                             HasDeadline = false,
                             DocumentDbId = n.Id
                         }).ToList();
                    break;
                case Models.DocumentTargetEntity.Module:
                    entityName = db.Modules.FirstOrDefault(n => n.Id == EntityId).Name;
                    items =
                     userPrivlageFileter(
                         db.Documents.Where(n => n.Module.Id == EntityId), user)
                         .Select(n => new DocumentItem() {
                             URL = n.Url,
                             RequiresUpload = false,
                             SelectionMechanic = DocumentSelectionMechanic.Url,
                             Owner = n.User.UserName,
                             Status = DocumentStatus.Yellow,
                             StatusText = "test",
                             PublishDate = DateTime.Now,
                             Feedback = "",
                             DeadLine = null,
                             HasDeadline = false,
                             DocumentDbId = n.Id
                         }).ToList();
                    break;
                case Models.DocumentTargetEntity.Course:
                    entityName = db.Courses.FirstOrDefault(n => n.Id == EntityId).Name;
                    items =
                     userPrivlageFileter(
                         db.Documents.Where(n => n.Course.Id == EntityId),user)
                         .Select(n => new DocumentItem() {
                             URL = n.Url,
                             RequiresUpload = false,
                             SelectionMechanic = DocumentSelectionMechanic.Url,
                             Owner = n.User.UserName,
                             Status = DocumentStatus.Yellow,
                             StatusText = "test",
                             PublishDate = DateTime.Now,
                             Feedback = "",
                             DeadLine = null,
                             HasDeadline = false,
                             DocumentDbId = n.Id
                         }).ToList();
                    break;
                default:
                    entityName = "";
                    items = new List<DocumentItem>();
                    break;
            }

            if (items == null) {
                items = new List<DocumentItem>();
            }


            return View(new Models.AddDocumentsViewModel {
                EntityId = EntityId,
                EntityName = entityName,
                EntityType = entityType,
                Done = false,
                ComboItems = LMS.Models.ComboBoxListItemHelper.GetOptions(typeof(Models.DocumentSelectionMechanic)),
                Items =   items

            });
        }

        private IQueryable<Document> userPrivlageFileter(IQueryable<Document> queryable,ApplicationUser user) {
            return queryable;
        }

        [HttpPost]
        public ActionResult Add(Models.AddDocumentsViewModel model) {
            var user = db.Users.First(n => n.Email == User.Identity.Name);

            if (model.Items == null) {
                model.Items = new List<DocumentItem>();
            }



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

        private bool ForEachUrl(DocumentItem item, AddDocumentsViewModel model, ApplicationUser user) {

            Activity activity = null;
            Module module = null;
            Course course = null;
            Document preexisting = db.Documents.FirstOrDefault(n => n.Id == item.DocumentDbId);

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

            if (preexisting == null) {

                if (item.PublishDate.HasValue) {
                    db.Documents.Add(new Document {
                        Id = Guid.NewGuid(),
                        IsLocal = false,
                        Activity = activity,
                        Course = course,
                        Module = module,

                        User = user,
                        Url = item.URL,
                        UploadDate = DateTime.Now,
                        PublishDate = item.PublishDate.Value
                    });
                    db.SaveChanges();
                    return true;
                } else {
                    //return false means this should be removed and not saved
                    return false; }
            } 
            else {
                preexisting.Module = module;
                preexisting.Course = course;
                preexisting.Activity = activity;
                preexisting.Url  = item.URL;
                preexisting.PublishDate = item.PublishDate;
                  db.SaveChanges();
                return true;
            }
      
        }

        private bool ForEachUrlLocal(DocumentItem item, AddDocumentsViewModel model, ApplicationUser user) {

        return    ForEachUrl(item, model, user);
        }

        private bool ForEachFile(DocumentItem item, AddDocumentsViewModel model, ApplicationUser user) {
            var filePath = $"Documents\\{model.EntityType}\\{item.File.FileName}";
            if (System.IO.File.Exists(filePath)) {
                 filePath = $"Documents\\{model.EntityType}\\{Guid.NewGuid()}_{item.File.FileName}";
                item.File.SaveAs(filePath);
                item.URL = filePath;
               return ForEachUrl(item, model, user);
            } else {
                item.File.SaveAs(filePath);
                item.URL = filePath;
               return ForEachUrl(item, model, user);
            }
        }



        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
        }
    }
}