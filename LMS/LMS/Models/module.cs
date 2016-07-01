using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMS.Models {
    public class Module : IEntity {

        public Module() {
            this.Documents = new List<Document>();
            this.Activities = new List<Activity>();
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

        [DataType(DataType.Date)]
        public DateTime  Start { get; set; }

        [DataType(DataType.Date)]
        public DateTime End { get; set; }

        public Guid? CourseId { get; set; }
        public  virtual Course Course { get; set;}
        public virtual List<Activity> Activities { get; set; }
        public virtual List<Document> Documents { get;  set; }
    }

}