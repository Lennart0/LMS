using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMS.Models {
    public class Activity : IEntity {
        public Activity() {
            this.Documents = new List<Document>();
        }
        public string Description {
            get;
            set;
        }
        public string Name {
            get;
            set;
        }

        public Guid Id { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start-datum")]
        public DateTime Start {
            get;
            set;
        }


        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Slut-datum")]
        public DateTime End { get; set; }
        public Guid? ModuleId { get; set; }
        public virtual Module Module { get; set; }
        public virtual List<Document> Documents { get; set; }
    }
}