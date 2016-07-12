using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models {
    public enum DocumentType {
        Info, Upgift,Annan //?? lite osäker på om vi alla hade samma åsikt här vi får diskutera så att vi inte har samma förväntningar tror jag
    }
    
    public abstract class Document {
        public Guid Id { get; set; }
        public DocumentType Type { get; set; }
        public Guid? CourseId { get; set; }
        public virtual Course Course { get; set; } //possibly include later...
        public Guid? ModuleId { get; set; }
        public virtual Module Module { get; set; }
        public Guid? ActivityId { get; set; }
        public virtual Activity Activity { get; set; }

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
        public DateTime? PublishDate { get; set; }

        public string PresentationName {
            get {
                string name = Url != null ? Url : "";
                if (Url != null && !Url.StartsWith("http", StringComparison.CurrentCultureIgnoreCase ) ) {
                    int ix = Url.LastIndexOf( '/' );
                    if ( ix >= 0 )
                        name = Url.Substring( ix + 1 );
                }
                return name;
            }
        }
    }

    public class PlainDocument : Document {

    }
}