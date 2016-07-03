using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMS.Models {
    public class Module : IEntity {

        public Module() {
            this.Documents  = new List<Document>();
            this.Activities = new List<Activity>();
        }

        public Guid Id { get; set; }

        [Display(Name = "Modul namn")]
        public string Name { get; set; }

        [Display(Name = "Beskrivning")]
        public string Description { get; set; }        

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Start-datum")]
        public DateTime  Start { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Slut-datum")]
        public DateTime End { get; set; }

        
        public Guid? CourseId { get; set; }

        public virtual Course Course { get; set;}
        public virtual List<Activity> Activities { get; set; }
        public virtual List<Document> Documents { get;  set; }
    }

}