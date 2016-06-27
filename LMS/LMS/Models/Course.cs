using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class Course : IEntity
    {
        public Guid Id { get; set; }
        public string Description { get; set; }

        //[Index("IX_NameDayStart", 1, IsUnique = true)]
        public string Name { get; set; }

        public DateTime LunchStart { get; set; }

        public DateTime LunchEnd { get; set; }

        //[Index("IX_NameDayStart", 2, IsUnique = true)]
        public DateTime DayStart { get; set; }

        public DateTime DayEnd { get; set; }

        public virtual List<ApplicationUser> Students { get; set; }
    }
}