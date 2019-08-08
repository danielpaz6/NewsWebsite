using NewsWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NewsWebsite.Controllers
{
    public class StatisticsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Statistics
        public ActionResult Index()
        {
            var map = new Dictionary<string, int>();
            var amap = new Dictionary<int, int>();

            // Categories Stats
            var groupJoinQuery =
                           from article in db.Articles
                           join category in db.Categories on article.CategoryId equals category.CategoryId into prodGroup
                           from prod in prodGroup
                           group prod by prod.Name into g
                           select new { CategoryName = g.Key, Count = g.Count() };

            ViewBag.CategoryAndCountMax = 0;


            foreach (var item in groupJoinQuery.ToList())
            {
                map.Add(item.CategoryName, item.Count);

                if (item.Count > ViewBag.CategoryAndCountMax)
                    ViewBag.CategoryAndCountMax = item.Count;
            }

            ViewBag.CatMap = map;

            var PopularArticles =
                         (from views in db.Views
                         join articles in db.Articles on views.ArticleId equals articles.ArticleId into prodGroup
                         from prod in prodGroup
                         group prod by prod.Title into g
                         select new { Title = g.Key.Substring(0, 40) + "...", Count = g.Count() }).ToList().OrderByDescending(x => x.Count).Take(3);


            ViewBag.Articles = new Dictionary<string, int>(PopularArticles.ToDictionary(x => x.Title, x => x.Count));

            ViewBag.CountArticles = db.Articles.Count();
            ViewBag.CountCategories = db.Categories.Count();
            ViewBag.CountUsers = db.Users.Count();


            return View();
        }

        // GET: GetGraph1
        public string GetGraph1()
        {
            /*var PopularArticles =
                         (from article in db.Articles
                          group article by article.Date into g
                          select new { Date = g.Key, Count = g.Count() }).GroupBy(x => x.Date).Select(group => new
                          {
                              Source = group.Key,
                              Count = group.Count()
                          })*/


            StringBuilder s = new StringBuilder();
            s.Append("date,close\n\r");
            foreach(var item in PopularArticles)
            {
                s.Append(item.Source + "," + item.Count + "\n\r");
            }

            return s.ToString();
        }
    }
}