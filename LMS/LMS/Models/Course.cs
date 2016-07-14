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

        public Course()
        {
            this.Documents = new List<Document>();
            this.Modules   = new List<Module>();
        }

        public Guid Id { get; set; }

        [Display(Name = "Kursnamn")]
        public string Name { get; set; }

        [Display(Name = "Beskrivning")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat (DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Start-datum")]
        public DateTime Start { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Slut-datum")]
        public DateTime End { get; set; }

        public DateTime? LunchStart { get; set; }

        public DateTime? LunchEnd { get; set; }

        public DateTime? DayStart { get; set; }

        public DateTime? DayEnd { get; set; }

        public virtual List<Module> Modules { get; set; }
        public virtual List<ApplicationUser> Students { get; set; }
        public virtual List<Document> Documents { get; set; }

        public string NameAndStartPresentation {
            get { return Name + Start.ToString( " yyMMdd" ); }
        }
    }
}