﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMS.Models;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LMS.Controllers {

    [Authorize]
    public class DocumentController : Controller {


        private List<T> RoleFilter<T>(IQueryable<T> items) where T:Document {
            if (User.IsInRole(Helpers.Constants.TeacherRole)) {
                return Helpers.DocHelper.ShowDeletedFiles(items,false).ToList(); // teacher sees all
            } else {
                // student sees teachers files and own files only
                return Helpers.DocHelper.ShowDeletedFiles(items,false).Where(n => 
                n.User.Roles.Count(m => m.RoleId == role.Id) > 0 
                || 
                n.User.UserName == User.Identity.Name
                ).ToList();
            }         
        }

        private IdentityRole role;
        private ApplicationUser logedInUser___Ignore;
        private ApplicationUser logedInUser {get{
                if (logedInUser___Ignore == null) {
                    logedInUser___Ignore = db.Users.FirstOrDefault(n => n.UserName == User.Identity.Name);
                }
                return logedInUser___Ignore;
            } }




        public DocumentController() : base() {
          role =  db.Roles.First(n=> n.Name == Helpers.Constants.TeacherRole);



            mappings = new Dictionary<DocumentTargetEntity, Func<ApplicationUser, Guid, List<Document>>>() {
                {
                    DocumentTargetEntity.Activity, (user,entityId) => { 

                        List<PlainDocument> allplainDocs = RoleFilter(db.PlainDocuments.Include(n=> n.Course.Students).Include(n=> n.Course).Include(n=> n.Module).Include(n=> n.Activity).Include(n=> n.User).Where(n => n.ActivityId == entityId));
                        List<TimeSensetiveDocument> allAsignments = RoleFilter(db.TimeSensetiveDocuments.Include(n=> n.Course.Students).Include(n=> n.Course).Include(n=> n.Module).Include(n=> n.Activity).Include(n=> n.User).Where(n => n.ActivityId == entityId).Include(n=> n.submissions));
                        List<AssignmentSubmission> allSubmisions = RoleFilter(db.AssignmentSubmissions.Include(n=> n.Course.Students).Include(n=> n.Course).Include(n=> n.Module).Include(n=> n.Activity).Include(n=> n.User).Where(n => n.ActivityId == entityId).Include(n=> n.assignment));
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

        public class NoCache : ActionFilterAttribute {
            public override void OnResultExecuting(ResultExecutingContext filterContext) {
                filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
                filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
                filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                filterContext.HttpContext.Response.Cache.SetNoStore();

                base.OnResultExecuting(filterContext);
            }
        }
        [NoCache]
        [ChildActionOnly]
        public ActionResult Status(Guid EntityId, Models.DocumentTargetEntity entityType) {
            var user = db.Users.First(n => n.Email == User.Identity.Name);
            var documents = mappings[entityType](user, EntityId);
            int WorstStatus = 0;
                      
            documents.ForEach(n => {
                DocumentStatus status;
                string statusText;
                bool IsOwner;
                setStatus(n, out status, out statusText, out IsOwner);
                int statusint = (int)status;
                if (statusint > WorstStatus) {
                    WorstStatus = statusint; 
                }
            });
            return View( new StatusViewModel {  EntityId = EntityId,  EntityType = entityType,   Status = (DocumentStatus)WorstStatus });
        }

        private void setStatus(Document document, out DocumentStatus Status, out string StatusText,out  bool IsOwner) {
            if (User.IsInRole(Helpers.Constants.TeacherRole)) {


                IsOwner = true;// teachers are all powerfull and are considerd owner of all documents!

                Course course = null; //Get Course reguardless of what entity its from
                if (document.ActivityId != null) {
                    course = document.Activity.Module.Course;
                } else if (document.ModuleId != null) {
                    course = document.Module.Course;
                } else if (document.CourseId != null) {
                    course = document.Course;
                }
                if (document is TimeSensetiveDocument) {

                    var timeSensetive = (document as TimeSensetiveDocument);

                    if (timeSensetive.submissions.Select(c => c.UserId).Distinct().Count() == course.Students.Count) {
                        Status = DocumentStatus.Green;
                        StatusText = "Allt inlämnat";
                    } else {

                        if (DateTime.Now >= timeSensetive.DeadLine) {
                            Status = DocumentStatus.Red;
                            var y = course.Students.Count();
                            var x = timeSensetive.submissions.Select(c => c.UserId).Distinct().Count();
                            StatusText = $"Försenad inlämning, {x} av {y} inlämnat";
                        } else {
                            Status = DocumentStatus.Yellow;
                            var y = course.Students.Count();
                            var x = timeSensetive.submissions.Select(c => c.UserId).Distinct().Count();
                            StatusText = $"{x} av {y} inlämnad";
                        }

                    }
                } else {
                    Status = DocumentStatus.None;
                    StatusText = "";
                }
                //End of Teacher scope
            } else {
                //start of student scope

                IsOwner = document.User.Email == User.Identity.Name;
                if (document is TimeSensetiveDocument) {
                    var timeSensetive = (document as TimeSensetiveDocument);

                   
                    if (timeSensetive.submissions.Count(u => u.UserId == logedInUser.Id) > 0) {
                        Status = DocumentStatus.Green;
                        StatusText = "Inlämnad";
                    } else {

                        if (DateTime.Now >= ((TimeSensetiveDocument)document)?.DeadLine) {
                            Status = DocumentStatus.Red;
                            StatusText = "Sen inlämning";
                        } else {
                            Status = DocumentStatus.Yellow;
                            StatusText = "ej inlämning";
                        }
                    }
                } else {
                    Status = DocumentStatus.None;
                    StatusText = "";
                }
            
                //end of student scope
            }
        }

        private void setStatus(Document n, DocumentItem item) {
            DocumentStatus status;
            string statusText;
            bool IsOwner;
            setStatus(n, out status, out statusText, out IsOwner);
            item.IsOwner = IsOwner;
            item.StatusText = statusText;
            item.Status = status;
        }

        public List<DocumentItem> CreateViewModelFromDbItem(DocumentTargetEntity target, ApplicationUser user, Guid id) {
            var result = mappings[target].Invoke(user, id).Select(n => {
                var item = new DocumentItem();


                if (n is Models.TimeSensetiveDocument) {
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
                if (n is Models.AssignmentSubmission) {
                    var x = n as AssignmentSubmission;

                    item.Feedback = x?.FeedBack;
                    item.URL = x.Url;
                    item.RequiresUpload = false;
                    item.SelectionMechanic = DocumentSelectionMechanic.Url;
                    item.Owner = x.User.UserName;
                    item.PublishDate = x.PublishDate;
                    setStatus(n, item);
                    item.HasDeadline = false;
                    item.IsAssigmentSubmission = true;
                    item.DocumentDbId = x.Id;
                    item.AssignmentId = x.assignmentId;
                } else
                if (n is Models.PlainDocument) {
                    item.URL = n.Url;
                    item.RequiresUpload = false;
                    item.SelectionMechanic = DocumentSelectionMechanic.Url;
                    item.Owner = n.User.UserName;
                    item.PublishDate = n.PublishDate;
                    setStatus(n, item);
                    item.HasDeadline = false;
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
                model.Items.Add(
                    new Models.DocumentItem {
                        SelectionMechanic = model.SelectionMechanic.Value,
                        RequiresUpload = true,
                        PublishDate =DateTime.Now
                    });
               
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
            Document preexisting =  db.Documents.FirstOrDefault(n => n.Id == item.DocumentDbId);

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

                    if (item.IsAssigmentSubmission) {
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
                            PublishDate = item.PublishDate.Value,
        
                        });

                    } else
                    if (item.HasDeadline) {
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
                if (preexisting is AssignmentSubmission) {
                    (preexisting as AssignmentSubmission).FeedBack = item.Feedback;
                }
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

            var lastSlash = item.File.FileName.Replace("/","\\").LastIndexOf("\\");// just in case of unix...
            var filenamen = item.File.FileName;
            if (lastSlash != -1) {
                filenamen = item.File.FileName.Substring(lastSlash+1);
            }

        var filePath = $"{binDir.Parent.FullName}\\Documents\\{model.EntityType}\\{filenamen}";
            if (System.IO.File.Exists(filePath)) {
                filePath = $"{binDir.Parent.FullName}\\Documents\\{model.EntityType}\\{Guid.NewGuid()}_{filenamen}";
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