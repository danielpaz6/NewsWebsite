using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWebsite.Models
{
    public class DistributionByCategory
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int Count { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}