using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class Course : IEntity
    {
        public Guid Id { get; set; }
        public string Description {
            get;
            set;
        }
        public string Name {
            get;
            set;
        }
        public DateTime LunchStart { get; set; }
        public DateTime LunchEnd { get; set; }
        public DateTime DayStart { get; set; }
        public DateTime DayEnd { get; set; }
        public virtual List<ApplicationUser> Students { get; set; }
    }
}