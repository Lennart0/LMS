using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models {
    public class FileModalPartialViewModel {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DocumentTargetEntity Type { get; set; }
        public string PostBackChange { get; set; }
        public string PostBackCancle { get; set; }
        public string InstanceId { get; set; }

    }

}