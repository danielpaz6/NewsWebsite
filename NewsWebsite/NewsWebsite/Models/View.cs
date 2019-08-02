using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWebsite.Models
{
    public class View
    {
        public int ViewId { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public ApplicationUser User { get; set; }
    }
}