﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models {
    public class TimeSensetiveDocument: Document {
        public DateTime DeadLine { get; set; }
        public virtual List<AssignmentSubmission> submissions { get; set; }
    }
}