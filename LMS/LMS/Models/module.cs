﻿using System;
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
        private DateTime start;
        public DateTime  Start { get { return start; } set {
                if (value < Course.Start) {
                    throw new ArgumentOutOfRangeException("Modul kan ej starta före kurs");
                } else {
                    start = value;
                }
            } }
        public DateTime End { get; set; }
        public  virtual Course Course { get; set;}
        public virtual List<Activity> Activities { get; set; }
    }

}