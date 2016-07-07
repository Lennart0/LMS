using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace LMS.Helpers {
    public static class MultipartFormHelper {
        public static MvcForm BeginMultipartForm(this HtmlHelper htmlHelper) {
            return htmlHelper.BeginForm(null, null, FormMethod.Post,
             new Dictionary<string, object>() { { "enctype", "multipart/form-data" } });
        }
    }
}


