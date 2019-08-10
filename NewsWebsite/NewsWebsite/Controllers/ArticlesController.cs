using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using NewsWebsite.Extensions;
using NewsWebsite.Models;

namespace NewsWebsite.Controllers
{
    public class ArticlesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Articles
        public ActionResult Index(string searchText, int? pageNumber)
        {
            //var articles = from m in db.Articles select m
            int pageSize = 20;

            if (searchText != null)
            {
                pageNumber = 1;
            }

            ViewBag.Categories = db.Categories.ToList();
            var articles = db.Articles.Include(a => a.Category);

           return View(PaginatedList<Article>.Create(articles, pageNumber ?? 1, pageSize));
           //return View(articles.ToList());
        }

        // GET: Articles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // GET: Articles/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArticleId,Title,Description,Date,NumOfLikes,ImageLink,ArticleLink,CategoryId")] Article article)
        {
            if (!Request.IsAuthenticated || User.Identity.GetPermission() == 0) // Checks if the user is logged in and has access
                return View();

            if (ModelState.IsValid)
            {
                db.Articles.Add(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", article.CategoryId);
            return View(article);
        }

        // GET: Articles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", article.CategoryId);
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArticleId,Title,Description,Date,NumOfLikes,ImageLink,ArticleLink,CategoryId")] Article article)
        {
            if (!Request.IsAuthenticated || User.Identity.GetPermission() == 0) // Checks if the user is logged in and has access
                return View();

            if (ModelState.IsValid)
            {
                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", article.CategoryId);
            return View(article);
        }

        // GET: Articles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!Request.IsAuthenticated || User.Identity.GetPermission() == 0) // Checks if the user is logged in and has access
                return View();

            Article article = db.Articles.Find(id);
            db.Articles.Remove(article);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (Request.IsAuthenticated && User.Identity.GetPermission() != 0) // Checks if the user is logged in and has access
            {
                if (disposing)
                {
                    db.Dispose();
                }
                base.Dispose(disposing);
            }
        }

        // News from other resources
        [HttpPost]
        public ActionResult CNN_News(int cat, int amount)
        {
            ViewBag.Errors = new List<String>();
            if (!Request.IsAuthenticated || User.Identity.GetPermission() == 0) // Checks if the user is logged in and has access
                ViewBag.Errors.Add("Not logged in");

            Category c = db.Categories.SingleOrDefault(x => x.CategoryId == cat);
            if (c == null)
                ViewBag.Errors.Add("Not valid category.");

            if (amount < 5 || amount > 50)
                ViewBag.Errors.Add("not a valid amount");

            if (ViewBag.Errors.Count > 0)
                return RedirectToAction("Index");

            GetNews gn = new GetNews();
            List<string[]> lst = gn.Add_CNN_News();

            var userId = User.Identity.GetUserId();
            ApplicationUser user = db.Users.SingleOrDefault(x => x.Id.Equals(userId));

            int count = 0;
            foreach (string[] str in lst)
            {
                if (count++ >= amount)
                    break;

                Article a = new Article();
                a.ArticleLink = str[2];
                a.Category = c;
                a.Date = DateTime.Now;
                a.Description = str[1];
                a.ImageLink = str[3];
                a.Title = str[0];
                a.User = user;
                a.NumOfLikes = 0;
                a.Source = "CNN";

                if(!db.Articles.Any(x => x.ArticleLink.Equals(a.ArticleLink))) // checks if the article is already exists
                    Create(a);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult FOX_News(int cat, int amount)
        {
            ViewBag.Errors = new List<String>();
            if (!Request.IsAuthenticated || User.Identity.GetPermission() == 0) // Checks if the user is logged in and has access
                ViewBag.Errors.Add("Not logged in");

            Category c = db.Categories.SingleOrDefault(x => x.CategoryId == cat);
            if (c == null)
                ViewBag.Errors.Add("Not valid category.");

            if (amount < 5 || amount > 50)
                ViewBag.Errors.Add("not a valid amount");

            if (ViewBag.Errors.Count > 0)
                return RedirectToAction("Index");

            GetNews gn = new GetNews();
            List<string[]> lst = gn.Add_FOX_News();

            var userId = User.Identity.GetUserId();
            ApplicationUser user = db.Users.SingleOrDefault(x => x.Id.Equals(userId));

            int count = 0;
            foreach (string[] str in lst)
            {
                if (count++ >= amount)
                    break;

                Article a = new Article();
                a.ArticleLink = str[2];
                a.Category = c;
                a.Date = DateTime.Now;
                a.Description = str[1];
                a.ImageLink = str[3];
                a.Title = str[0];
                a.User = user;
                a.NumOfLikes = 0;
                a.Source = "FOX";

                if (!db.Articles.Any(x => x.ArticleLink.Equals(a.ArticleLink))) // checks if the article is already exists
                    Create(a);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult YNET_News(int cat, int amount)
        {
            ViewBag.Errors = new List<String>();
            if (!Request.IsAuthenticated || User.Identity.GetPermission() == 0) // Checks if the user is logged in and has access
                ViewBag.Errors.Add("Not logged in");

            Category c = db.Categories.SingleOrDefault(x => x.CategoryId == cat);
            if (c == null)
                ViewBag.Errors.Add("Not valid category.");

            if (amount < 5 || amount > 50)
                ViewBag.Errors.Add("not a valid amount");

            if (ViewBag.Errors.Count > 0)
                return RedirectToAction("Index");

            GetNews gn = new GetNews();
            List<string[]> lst = gn.Add_Ynet_News();

            var userId = User.Identity.GetUserId();
            ApplicationUser user = db.Users.SingleOrDefault(x => x.Id.Equals(userId));

            int count = 0;
            foreach (string[] str in lst)
            {
                if (count++ >= amount)
                    break;

                Article a = new Article();
                a.ArticleLink = str[2];
                a.Category = c;
                a.Date = DateTime.Now;
                a.Description = str[1];
                a.ImageLink = str[3];
                a.Title = str[0];
                a.User = user;
                a.NumOfLikes = 0;
                a.Source = "YNET";

                if (!db.Articles.Any(x => x.ArticleLink.Equals(a.ArticleLink))) // checks if the article is already exists
                    Create(a);
            }

            return RedirectToAction("Index");
        }
    }
}
