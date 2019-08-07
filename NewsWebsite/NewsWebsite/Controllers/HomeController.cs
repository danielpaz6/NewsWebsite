using Microsoft.AspNet.Identity;
using NewsWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NewsWebsite.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                ApplicationUser user = db.Users.SingleOrDefault(x => x.Id.Equals(userId));
                
                if(!user.StatsLastUpdate.HasValue || (user.StatsLastUpdate != null && (DateTime.Now - user.StatsLastUpdate).Value.Days >= 1))
                {
                    /*
                    var entryPoint = (from a in db.Articles
                                      join v in db.Views on a.ArticleId equals v.ArticleId
                                      join c in db.Categories on a.CategoryId equals c.CategoryId into n
                                      where v.User.Id == userId
                                      group n by 
                                      select new
                                      {
                                          UID = v.User.Id,
                                          SRC = a.Source,
                                          CATEGORY = c.Name
                                      });
                     */

                    var entryPoint = (from a in db.Articles
                                      join v in db.Views on a.ArticleId equals v.ArticleId
                                      select new
                                      {
                                          UID = v.User.Id,
                                          SRC = a.Source,
                                          CATEGORYID = a.CategoryId
                                      }).ToList();

                    var entryPoint2 = from e in entryPoint
                                 join c in db.Categories on e.CATEGORYID equals c.CategoryId
                                 where e.UID == userId
                                 select new
                                 {
                                     e.UID,
                                     e.SRC,
                                     CATEGORYNAME = c.Name,
                                     CATEGORYID = c.CategoryId
                                 };

                    var entryPoint3 = entryPoint2.GroupBy(n => n.CATEGORYID)
                                                .Select(group => new
                                                {
                                                    CategoryId = group.Key,
                                                    Count = group.Count()
                                                }).ToList();

                    var entryPoint4 = entryPoint2.GroupBy(n => n.SRC)
                                                .Select(group => new
                                                {
                                                    Source = group.Key,
                                                    Count = group.Count()
                                                }).ToList();

                    user.DistributionByCategory.Clear();

                    foreach (var item in entryPoint3)
                    {
                        System.Diagnostics.Debug.WriteLine("Your count: ");
                        System.Diagnostics.Debug.WriteLine(item.CategoryId);
                        System.Diagnostics.Debug.WriteLine(item.Count);

                        DistributionByCategory d = new DistributionByCategory();
                        d.Category = db.Categories.Find(item.CategoryId);
                        d.CategoryId = item.CategoryId;
                        d.Count = item.Count;

                        user.DistributionByCategory.Add(d);
                    }

                    db.SaveChanges();
                }
            }
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        // POST: IncreaseView
        [HttpPost]
        public void IncreaseView(int id)
        {
            if (!Request.IsAuthenticated) // Check if the user is logged in
                return;

            var userId = User.Identity.GetUserId();
            ApplicationUser user = db.Users.SingleOrDefault(x => x.Id.Equals(userId));

            Article article = db.Articles.SingleOrDefault(x => x.ArticleId.Equals(id));

            if(article != null && user != null)
            {
                bool checkIfExists = db.Views.Any(x => x.ArticleId.Equals(id) && x.User.Id.Equals(userId));

                if (checkIfExists) // Checks if the View row is already exists
                    return;

                View v = new View();
                v.ArticleId = id;
                v.Article = article;
                v.User = user;
                v.WatchDate = DateTime.Now;

                db.Views.Add(v);
                db.SaveChanges();

            }
        }

        // GET: Get News
        public string GetArticles(int pageIndex)
        {
            int articlesPerPage = 12; // settings

            IEnumerable<Article> articles;
            List<IEnumerable<Article>> list = new List<IEnumerable<Article>>();
            
            if (!Request.IsAuthenticated)
                articles = db.Articles.ToList().Skip((pageIndex - 1) * articlesPerPage).Take(articlesPerPage);
            else
            {
                int categorySize = db.Categories.Count();
                
            }


            //var articles = db.Articles.ToList().Skip((pageIndex-1)* articlesPerPage).Take(articlesPerPage);

            // --------- Displaying the articles list --------- \\
            StringBuilder s = new StringBuilder();

            foreach(var article in articles)
            {
                s.Append("<div class=\"card\">"+
                "<img class=\"card-img-top img-fluid\" src=\""+article.ImageLink + "\" alt=\"Card image cap\">"+
                "<div class=\"card-block\">"+
                    "<h5 class=\"card-title fixed-font\"><a class=\"openLink\" val=\""+article.ArticleId+"\" target=\"_blank\" href=\""+article.ArticleLink+"\">"+article.Title+"</a></h5>"+
                    "<p class=\"card-text fixed-fontsize\">"+article.Description+"</p>"+
                "</div>"+
                "<div class=\"card-footer\">"+
                    "<small class=\"text-muted\">"+article.lastUpdated(article.Date)+"</small>"+
                "</div>"+
                "</div>");
            }

            return s.ToString();
            
        }
    }
}