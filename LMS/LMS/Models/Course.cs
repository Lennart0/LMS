using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class Course : IEntity
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime Start { get; set; }
        [DataType(DataType.Date)]
        public DateTime End { get; set; }


        public virtual List<Module> Modules { get; set; }

        public virtual List<ApplicationUser> Students { get; set; }
        public List<Document> Documents { get; internal set; }
    }
}