using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class AssignmentSubmission : Document
    {
        public string FeedBack { get; set; }
        public Guid? assignmentId { get; set; }
        public virtual TimeSensetiveDocument assignment { get; set; }
    }
}