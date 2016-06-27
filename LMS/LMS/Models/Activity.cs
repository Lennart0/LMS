using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models {
    public class Activity : IEntity {
        public string Description {
            get;
            set;
        }
        public string Name {
            get;
            set;
        }

        public Guid Id { get; set; }

        private DateTime start;
        public DateTime Start {
            get { return start; }
            set {
                if (value < Module.Start) {
                    throw new ArgumentOutOfRangeException("Aktivitet kan ej starta före modul");
                } else {
                    start = value;
                }
            }
        }
        public DateTime End { get; set; }
        public virtual Module Module { get; set; }
    }
}