using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models {
    public class Module : IEntity {
        public string Description {
            get;
            set;
        }
        public string Name {
            get;
            set;
        }
        public Guid Id { get; set; }
        [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public DateTime  Start { get; set; }
        [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public DateTime End { get; set; }
        public  virtual Course Course { get; set;}
        public virtual List<Activity> Activities { get; set; }
        public List<Document> Documents { get; internal set; }
    }

}