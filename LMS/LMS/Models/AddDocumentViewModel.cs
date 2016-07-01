using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace LMS.Models {
    public enum DocumentTargetEntity {
        Course, Module, Activity, User
    }

    //very abstract status... its up to the controller to add the proper text for the status
    public enum DocumentStatus {
        Red,Yellow,Green
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
        public HttpPostedFileBase File { get; set; }//not used in sprint 1
        public DocumentStatus Status {get;set;}
        public string StatusText { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true, NullDisplayText = "Ej publiserad")]
        [DataType(DataType.Date)]
        public DateTime? PublishDate { get; set; }
        public bool HasDeadline { get; set; }
        public TimeSpan? DeadLine { get; set; }
        public string Feedback { get; set; }
  
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