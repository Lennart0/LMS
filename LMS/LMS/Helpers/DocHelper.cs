using LMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Helpers
{
    public static class DocHelper
    {
        static public void AssocDocsToViewBag(IEnumerable<Document> docs, dynamic viewBag) {
            var urls = new List<Document>();
            var otherDocs = new List<Document>();
            foreach ( var doc in docs ) {
                if ( string.IsNullOrWhiteSpace( doc.Url ) )
                    ; // otherDocs.Add( doc );
                else
                    urls.Add( doc );
            }
            viewBag.AssocUrls = urls;
            viewBag.AssocDocs = otherDocs;
        }
    }
}