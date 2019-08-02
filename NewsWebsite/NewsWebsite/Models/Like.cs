using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWebsite.Models
{
    public class Like
    {
        public int LikeId { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public ApplicationUser User { get; set; }
    }
}