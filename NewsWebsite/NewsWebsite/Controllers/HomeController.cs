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

                if(user == null) // If we enter this condition, it means that the user has been deleted but the cookies/session is still alive.
                {
                    // Log off ( remove cookies & session )
                    HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    return RedirectToAction("Index", "Home");
                }
                
                if(user.StatsLastUpdate == null || (DateTime.Now - user.StatsLastUpdate).Value.Days >= 1)
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
                        d.User = user;

                        user.DistributionByCategory.Add(d);
                    }

                    db.Users.Find(userId).StatsLastUpdate = DateTime.Now;
                    db.SaveChanges();
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("User is not logged in.");
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
            var locations = db.Settings.Where(x => x.Key.Equals("Location"));

            return View(locations.ToList());
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
            int articlesPerPage = 21; // settings

            IEnumerable<Article> articles;
            List<IEnumerable<Article>> list = new List<IEnumerable<Article>>();
            
            if (!Request.IsAuthenticated)
                articles = db.Articles.OrderBy(x => x.Date).ToList().Skip((pageIndex - 1) * articlesPerPage).Take(articlesPerPage);
            else
            {
                var userId = User.Identity.GetUserId();
                ApplicationUser user = db.Users.SingleOrDefault(x => x.Id.Equals(userId));

                if (user == null) // If we enter this condition, it means that the user has been deleted but the cookies/session is still alive.
                {
                    // Log off ( remove cookies & session )
                    HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    return "";
                }

                List<Category> othersCategory = new List<Category>(db.Categories);

                int categorySize = 0;
                foreach(var v in user.DistributionByCategory)
                {
                    categorySize += v.Count;
                }

                double percent = categorySize == db.Categories.Count() ? 1 : 0.8;
                foreach (var category in user.DistributionByCategory)
                {
                    int limit = (int)Math.Ceiling((double)((double)category.Count / (double)categorySize) * (int)(percent * articlesPerPage));
                    var tmp = db.Articles.Where(item => item.CategoryId == category.CategoryId).OrderBy(x => x.Date).Skip((pageIndex - 1) * limit).Take(limit).ToList();

                    list.Add(tmp);
                    othersCategory.Remove(category.Category);
                }

                //for(int i = (int)Math.Ceiling(0.2 * articlesPerPage); i <= articlesPerPage; i++)
                foreach(var category in othersCategory)
                {
                    int limit = (int)(Math.Ceiling(0.2 * articlesPerPage) / (double)othersCategory.Count());
                    list.Add(db.Articles.Where(item => item.CategoryId == category.CategoryId).OrderBy(x => x.Date).Skip((pageIndex - 1) * limit).Take(limit).ToList());
                }

                articles = list.SelectMany(x => x).ToList();
            }
            
            // --------- Displaying the articles list
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