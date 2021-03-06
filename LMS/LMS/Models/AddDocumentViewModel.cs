﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace LMS.Models {
    public enum DocumentTargetEntity {
        Course, Module, Activity, User
    }

    //very abstract status... its up to the controller to add the proper text for the status
    public enum DocumentStatus {
       None=0, Green=1, Yellow=2, Red=3
    }

    public enum DocumentSelectionMechanic {
        File, Url, InternalUrlLookup
    }

 

    public class DocumentItem {

 


   
        public Guid DocumentDbId { get; set; }
        public DocumentSelectionMechanic SelectionMechanic { get; set; }
        public bool RequiresUpload { get; set; } //if it has already been uploaded this is also compared with selection machanic
 
        public string Owner { get; set; }

        public string URL {
            get; set;
        }

       static Regex rexA = new Regex(@".*\/([^\/]*)");
       static Regex rexB = new Regex(@".*\/([^\/]*)\/");
        public string ShortName{ get {
                return Helpers.URLHelper.Shorten(this.URL);
            } 
        }
        public HttpPostedFileBase File { get; set; }//not used in sprint 1
        public DocumentStatus Status {get;set;}
        public string StatusText { get; set; }
 
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true, NullDisplayText = "Ej publiserad")]
        public DateTime? PublishDate { get; set; }
        public bool HasDeadline { get; set; }

 
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true, NullDisplayText = "")]
        public DateTime? DeadLine { get; set; }


        public bool IsAssigmentSubmission { get; set; }
        public Guid? AssignmentId { get; set; }

        public string Feedback { get; set; }

        public bool IsOwner { get; set; }
    }

    public class ComboBoxListItemHelper {
        private static Dictionary<Type, List<SelectListItem>> items = new Dictionary<Type, List<SelectListItem>>();
        public static List<SelectListItem> GetOptions(Type enm) {
            if (enm.IsEnum) {
                if (!items.ContainsKey(enm)) {
                    items.Add(enm, Enum.GetNames(enm).Select(n => new SelectListItem() { Value = n, Text = n }).ToList());
                }
                return items[enm];
            } else {
                return null;
            }
        }
    }

    public class AddDocumentsViewModel {
        public List<System.Web.Mvc.SelectListItem> AssigmentsList { get; set; }

        public string Error { get; set; }
        public DocumentTargetEntity EntityType { get; set; }
        public Guid EntityId { get; set; }
        public string EntityName { get; set; }

        public List<DocumentItem> Items { get; set; }
        public bool Done { get; set; }
        public DocumentSelectionMechanic? SelectionMechanic { get; set; }
        public List<SelectListItem> ComboItems { get; set; }
   
    }
}