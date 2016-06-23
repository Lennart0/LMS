using System;
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
    }

}