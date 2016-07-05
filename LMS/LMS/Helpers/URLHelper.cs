using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Helpers {
    public class URLHelper {

        public static string Shorten(string url) {
            int ix = url != null ? url.LastIndexOf('/') : -1;
            if (ix >= 0) {
                return url.Substring(ix + 1);
            } else {
                return url;
            }
        }



    }


}
