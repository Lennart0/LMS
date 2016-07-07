using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMS.Models;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;

namespace LMS.Controllers {

    [Authorize]
    public class DocumentController : Controller {



        public DocumentController() : base() {

            mappings = new Dictionary<DocumentTargetEntity, Func<ApplicationUser, Guid, List<Document>>>() {
                {
                    DocumentTargetEntity.Activity, (user,entityId) => { 

                                               List<PlainDocument> allplainDocs = db.PlainDocuments.Include(n=> n.Course.Students).Include(n=> n.Course).Include(n=> n.Module).Include(n=> n.Activity).Include(n=> n.User).Where(n => n.ActivityId == entityId).ToList();
                        List<TimeSensetiveDocument> allAsignments = db.TimeSensetiveDocuments.Include(n=> n.Course.Students).Include(n=> n.Course).Include(n=> n.Module).Include(n=> n.Activity).Include(n=> n.User).Where(n => n.ActivityId == entityId).Include(n=> n.submissions).ToList();
                        List<AssignmentSubmission> allSubmisions = db.AssignmentSubmissions.Include(n=> n.Course.Students).Include(n=> n.Course).Include(n=> n.Module).Include(n=> n.Activity).Include(n=> n.User).Where(n => n.ActivityId == entityId).Include(n=> n.assignment).ToList();
                        var result = new List<Document>();
                        result.AddRange(allplainDocs);
                        result.AddRange(allAsignments);
                        result.AddRange(allSubmisions);
                       return userPrivlageFileter(result , user);
                        }
                        },
                 {
                    DocumentTargetEntity.Module, (user,entityId) =>
                    {  
                        List<PlainDocument> allplainDocs = db.PlainDocuments.Include(n=> n.Course.Students).Include(n=> n.Course).Include(n=> n.Module).Include(n=> n.Activity).Include(n=> n.User).Where(n => n.ModuleId == entityId).ToList();
                        List<TimeSensetiveDocument> allAsignments = db.TimeSensetiveDocuments.Include(n=> n.Course.Students).Include(n=> n.Course).Include(n=> n.Module).Include(n=> n.Activity).Include(n=> n.User).Where(n => n.ModuleId == entityId).Include(n=> n.submissions).ToList();
                        List<AssignmentSubmission> allSubmisions = db.AssignmentSubmissions.Include(n=> n.Course.Students).Include(n=> n.Course).Include(n=> n.Module).Include(n=> n.Activity).Include(n=> n.User).Where(n => n.ModuleId == entityId).Include(n=> n.assignment).ToList();
                        var result = new List<Document>();
                        result.AddRange(allplainDocs);
                        result.AddRange(allAsignments);
                        result.AddRange(allSubmisions);
                       return userPrivlageFileter(result , user);

                        }
                },
                {
                    DocumentTargetEntity.Course, (user,entityId) =>
                    {
                        List<PlainDocument> allplainDocs = db.PlainDocuments.Include(n=> n.Course.Students).Include(n=> n.Course).Include(n=> n.Module).Include(n=> n.Activity).Include(n=> n.User).Where(n => n.CourseId == entityId).ToList();
                        List<TimeSensetiveDocument> allAsignments = db.TimeSensetiveDocuments.Include(n=> n.Course.Students).Include(n=> n.Course).Include(n=> n.Module).Include(n=> n.Activity).Include(n=> n.User).Where(n => n.CourseId == entityId).Include(n=> n.submissions).ToList();
                        List<AssignmentSubmission> allSubmisions = db.AssignmentSubmissions.Include(n=> n.Course.Students).Include(n=> n.Course).Include(n=> n.Module).Include(n=> n.Activity).Include(n=> n.User).Where(n => n.CourseId == entityId).Include(n=> n.assignment).ToList();
                        var result = new List<Document>();
                        result.AddRange(allplainDocs);
                        result.AddRange(allAsignments);
                        result.AddRange(allSubmisions);
                       return userPrivlageFileter(result , user);
                    }

                }
             };


        }

        private Dictionary<DocumentTargetEntity, Func<ApplicationUser, Guid, List<Document>>> mappings;





        private DataAccessLayer.ApplicationDbContext db = new DataAccessLayer.ApplicationDbContext();

        private void setStatus(Document n, DocumentItem item) {
            if (User.IsInRole(Helpers.Constants.TeacherRole)) {


                item.IsOwner = true;

                Course course = null;
                if (n.ActivityId != null) {
                    course = n.Activity.Module.Course;
                } else if (n.ModuleId != null) {
                    course = n.Module.Course;
                } else if (n.CourseId != null) {
                    course = n.Course;
                }

                if ((n as TimeSensetiveDocument).submissions.Select(c => c.UserId).Distinct().Count() == course.Students.Count) {
                    item.Status = DocumentStatus.Green;
                    item.StatusText = "Allt inlämnat";
                } else {

                    if (DateTime.Now >= ((TimeSensetiveDocument)n)?.DeadLine) {
                        item.Status = DocumentStatus.Red;
                        var y = course.Students.Count();
                        var x = (n as TimeSensetiveDocument).submissions.Select(c => c.UserId).Distinct().Count();
                        item.StatusText = $"Försenad inlämning, {x} av {y} inlämnat";
                    } else {
                        item.Status = DocumentStatus.Yellow;
                        var y = course.Students.Count();
                        var x = (n as TimeSensetiveDocument).submissions.Select(c => c.UserId).Distinct().Count();

                        item.StatusText = $"{x} av {y} inlämnad";
                    }
                }
            } else {

  
                item.IsOwner = n.User.Email == User.Identity.Name;

                if (((TimeSensetiveDocument)n).submissions.Count(u => u.User.UserName == User.Identity.Name) > 0) {
                    item.Status = DocumentStatus.Green;
                    item.StatusText = "Inlämnad";
                } else {

                    if (DateTime.Now >= ((TimeSensetiveDocument)n)?.DeadLine) {
                        item.Status = DocumentStatus.Red;
                        item.StatusText = "Sen inlämning";
                    } else {
                        item.Status = DocumentStatus.Yellow;
                        item.StatusText = "ej inlämning";
                    }
                }
            }
        }

        public List<DocumentItem> CreateViewModelFromDbItem(DocumentTargetEntity target, ApplicationUser user, Guid id) {
            var result = mappings[target].Invoke(user, id).Select(n => {
                var item = new DocumentItem();


                if (ObjectContext.GetObjectType(n.GetType()) == typeof(Models.TimeSensetiveDocument)) {
                    item.URL = n.Url;
                    item.RequiresUpload = false;
                    item.SelectionMechanic = DocumentSelectionMechanic.Url;
                    item.Owner = n.User.UserName;
                    setStatus(n, item);
                    item.PublishDate = n.PublishDate;

                    item.DeadLine = (n as TimeSensetiveDocument)?.DeadLine;
                    item.HasDeadline = true;
                    item.DocumentDbId = n.Id;
                } else
                if (ObjectContext.GetObjectType(n.GetType()) == typeof(Models.AssignmentSubmission)) {
                    var x = n as AssignmentSubmission;

                    item.Feedback = x?.FeedBack;
                    item.URL = n.Url;
                    item.RequiresUpload = false;
                    item.SelectionMechanic = DocumentSelectionMechanic.Url;
                    item.Owner = n.User.UserName;
                    item.PublishDate = n.PublishDate; ;
                    item.HasDeadline = true;
                    item.DocumentDbId = n.Id;
                } else
                if (ObjectContext.GetObjectType(n.GetType()) == typeof(Models.PlainDocument)) {
                    item.URL = n.Url;
                    item.RequiresUpload = false;
                    item.SelectionMechanic = DocumentSelectionMechanic.Url;
                    item.Owner = n.User.UserName;
                    item.PublishDate = n.PublishDate; ;
                    item.HasDeadline = true;
                    item.DocumentDbId = n.Id;
                }
                return item;
            }).ToList();


            return result;
        }


        // GET: Document
        public ActionResult Add(Guid EntityId, Models.DocumentTargetEntity entityType) {
            var user = db.Users.First(n => n.Email == User.Identity.Name);
            List<DocumentItem> items = new List<DocumentItem>();
            var entityName = "";
            items = CreateViewModelFromDbItem(entityType, user, EntityId);

            /*
            switch (entityType) {
                //case Models.DocumentTargetEntity.User:
                //    entityName = db.Users.FirstOrDefault(n => n.Id == EntityId.ToString()).UserName;
                //    items = 
                //        userPrivlageFileter(
                //            db.Documents.Where(n => n.User.Id == EntityId.ToString()) , user)
                //            .Select(n => new DocumentItem() {
                //                URL = n.Url,
                //                RequiresUpload =false,
                //                SelectionMechanic = DocumentSelectionMechanic.Url,
                //                Owner = n.User.UserName,
                //                Status = DocumentStatus.Yellow,
                //                StatusText ="",
                //             PublishDate= DateTime.Now,
                //             Feedback="",
                //             DeadLine=null,
                //             HasDeadline=false,
                //             DocumentDbId = n.Id}).ToList();
                //    break;
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
                             StatusText = "",
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
                             StatusText = "",
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
                         db.Documents.Where(n => n.Course.Id == EntityId), user)
                         .Select(n => new DocumentItem() {
                             URL = n.Url,
                             RequiresUpload = false,
                             SelectionMechanic = DocumentSelectionMechanic.Url,
                             Owner = n.User.UserName,
                             Status = DocumentStatus.Yellow,
                             StatusText = "",
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
            }*/

            if (items == null) {
                items = new List<DocumentItem>();
            }

            


            return View(new Models.AddDocumentsViewModel {
                EntityId = EntityId,
                EntityName = entityName,
                EntityType = entityType,
                Done = false,
                AssigmentsList = GetDropDownForAssignments(entityType, EntityId),
                ComboItems = LMS.Models.ComboBoxListItemHelper.GetOptions(typeof(Models.DocumentSelectionMechanic)),
                Items = items

            });
        }
        private List<SelectListItem> GetDropDownForAssignments(AddDocumentsViewModel model) {

          return   model.Items.Where(n=> n.DeadLine != null && n.PublishDate != null).Select(n => new System.Web.Mvc.SelectListItem() { Value = n.DocumentDbId.ToString(), Text = Helpers.URLHelper.Shorten(n.URL) }).ToList();
      
                   
        }

        private List<System.Web.Mvc.SelectListItem> GetDropDownForAssignments(DocumentTargetEntity entityType, Guid EntityId) {
            List<System.Web.Mvc.SelectListItem> dropDownItems = null;
            switch (entityType) {
                case DocumentTargetEntity.Course:
                    dropDownItems = db.TimeSensetiveDocuments.Where(n => n.CourseId == EntityId && n.PublishDate != null).ToList()
                    .Select(n => new System.Web.Mvc.SelectListItem() { Value = n.Id.ToString(), Text = Helpers.URLHelper.Shorten(n.Url) }).ToList();
                    break;
                case DocumentTargetEntity.Module:
                    dropDownItems = db.TimeSensetiveDocuments.Where(n => n.ModuleId == EntityId && n.PublishDate != null).ToList()
                    .Select(n => new System.Web.Mvc.SelectListItem() { Value = n.Id.ToString(), Text = Helpers.URLHelper.Shorten(n.Url) }).ToList();
                    break;
                case DocumentTargetEntity.Activity:
                    dropDownItems = db.TimeSensetiveDocuments.Where(n => n.ActivityId == EntityId && n.PublishDate != null).ToList()
                    .Select(n => new System.Web.Mvc.SelectListItem() { Value = n.Id.ToString(), Text = Helpers.URLHelper.Shorten(n.Url) }).ToList();
                    break;
            }
            return dropDownItems;
        }

        private List<Document> userPrivlageFileter(List<Document> queryable, ApplicationUser user) {
            return queryable; // think this kind of got redundant.... remove later
        }

        [HttpPost]
        public ActionResult Add(Models.AddDocumentsViewModel model) {
            var user = db.Users.First(n => n.Email == User.Identity.Name);

            if (model.Items == null) {
                model.Items = new List<DocumentItem>();
            }



            if (!model.Done) {
                model.Items.Add(new Models.DocumentItem { SelectionMechanic = model.SelectionMechanic.Value, RequiresUpload = true, PublishDate=DateTime.Now });
               
            } else {
                model.Done = false;
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
            model.AssigmentsList = GetDropDownForAssignments(model);
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

                    if (item.AssignmentId != null) {
                        db.Documents.Add(new AssignmentSubmission {
                            Id = Guid.NewGuid(),
                            IsLocal = false,
                            Activity = activity,
                            Course = course,
                            Module = module,
                                 assignmentId = item.AssignmentId,
                            User = user,
                            Url = item.URL,
                            UploadDate = DateTime.Now,
                            PublishDate = item.PublishDate.Value
                        });

                    } else
                    if (item.DeadLine != null) {
                        db.Documents.Add(new TimeSensetiveDocument {
                            Id = Guid.NewGuid(),
                            IsLocal = false,
                            Activity = activity,
                            Course = course,
                            Module = module,
                            DeadLine = item.DeadLine.Value,
                            User = user,
                            Url = item.URL,
                            UploadDate = DateTime.Now,
                            PublishDate = item.PublishDate.Value
                        });
                    } else {

                        db.Documents.Add(new PlainDocument {
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
                    }
                    db.SaveChanges();
                    return true;
                } else {
                    //return false means this should be removed and not saved
                    return false;
                }
            } else {
                preexisting.Module = module;
                preexisting.Course = course;
                preexisting.Activity = activity;
                preexisting.Url = item.URL;
                preexisting.PublishDate = item.PublishDate;
                db.SaveChanges();
                return true;
            }

        }

        private bool ForEachUrlLocal(DocumentItem item, AddDocumentsViewModel model, ApplicationUser user) {

            return ForEachUrl(item, model, user);
        }

        private bool ForEachFile(DocumentItem item, AddDocumentsViewModel model, ApplicationUser user) {
            var index = model.Items.Where(n=> n.SelectionMechanic== DocumentSelectionMechanic.File).ToList().IndexOf(item);

            var binDir = new System.IO.DirectoryInfo(AppDomain.CurrentDomain.RelativeSearchPath);
       
            var url = Url.Content($"~//Documents//{model.EntityType}//{item.File.FileName}");
        

            var filePath = $"{binDir.Parent.FullName}\\Documents\\{model.EntityType}\\{item.File.FileName}";
            if (System.IO.File.Exists(filePath)) {
                filePath = $"{binDir.Parent.FullName}\\Documents\\{model.EntityType}\\{Guid.NewGuid()}_{item.File.FileName}";
                url = Url.Content($"~//Documents//{model.EntityType}//{Guid.NewGuid()}_{item.File.FileName}");

                item.File.SaveAs(filePath);
                item.URL = url;
                return ForEachUrl(item, model, user);
            } else {
                item.File.SaveAs(filePath);
                item.URL = url;
                return ForEachUrl(item, model, user);
            }
        }



        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
        }
    }
}