using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models {
    public class StatusViewModel {
        public DocumentStatus Status { get; set; }
        public DocumentTargetEntity EntityType { get; set; }
        public Guid EntityId { get; set; }
    }
}