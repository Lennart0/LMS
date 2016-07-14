using LMS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Helpers
{
    public static class DocHelper
    {
        public static IQueryable<T> ShowDeletedFiles<T>(IQueryable<T> items, bool showHidden ) where T : Document {
            if (showHidden) {
                return items.Where(n=> n.PublishDate.Value <= DateTime.Now); //needs work not used so far
            } else {
                return items.Where(n => n.PublishDate != null).Where(n => n.PublishDate.Value <= DateTime.Now);
            }
        }
        public static List<T> ShowDeletedFiles<T>(List<T> items, bool showHidden) where T : Document {
            if (showHidden) {
                return items.Where(n => n.PublishDate.Value <= DateTime.Now).ToList(); //needs work not used so far
            } else {
                return items.Where(n => n.PublishDate != null).Where(n => n.PublishDate.Value <= DateTime.Now).ToList();
            }
        }

        static public bool IsLink(Document doc) {
            return doc != null && doc.Url != null && doc.Url.TrimStart().StartsWith( "http", StringComparison.CurrentCultureIgnoreCase );
        }

        static public void AssocDocsToViewBag(IEnumerable<Document> docs, dynamic viewBag) {
            var links = new List<Document>();
            var otherDocs = new List<Document>();
            foreach ( var doc in docs ) {
                if ( doc.Url == null )
                    continue;
                if (doc.Url.TrimStart().StartsWith("http", StringComparison.CurrentCultureIgnoreCase))
                    links.Add( doc );
                else
                    otherDocs.Add( doc );
            }
            viewBag.AssocUrls = links;
            viewBag.AssocDocs = otherDocs;
        }
    }
}