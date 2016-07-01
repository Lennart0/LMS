using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models {
    public enum DocumentType {
        Info, Upgift,Annan //?? lite osäker på om vi alla hade samma åsikt här vi får diskutera så att vi inte har samma förväntningar tror jag
    }
    
    public class Document {
        public Guid Id { get; set; }
        public DocumentType Type { get; set; }
        public Guid? CourseId { get; set; }
        public Course Course { get; set; } //possibly include later...
        public Guid? ModuleId { get; set; }
        public Module Module { get; set; }
        public Guid? ActivityId { get; set; }
        public Activity Activity { get; set; }

        /// <summary>
        /// document location local or external as a web accessible url
        /// </summary>
        public string Url { get; set; }
      
        /// <summary>
            /// was this file downloaded locally thus do we need to track if it needs to be removed if this record is deleted?
            /// </summary>
        public bool IsLocal { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime UploadDate { get; set; }
        public DateTime PublishDate { get; set; }
    //    public Guid? CourseId { get; internal set; }
     //   public Guid? ModuleId { get; internal set; }
      //  public Guid? ActivityId { get; internal set; }
    }
}