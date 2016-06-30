using System;
using System.Collections.Generic;
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

 
        public DateTime Start {
            get;
            set;
        }
        public DateTime End { get; set; }
        public virtual Module Module { get; set; }
        public virtual List<Document> Documents { get; set; }
    }
}