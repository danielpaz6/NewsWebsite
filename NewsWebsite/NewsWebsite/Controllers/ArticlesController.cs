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
        public ActionResult Index(string searchText)
        {
            //var articles = from m in db.Articles select m

           var articles = db.Articles.Include(a => a.Category);

           return View(articles.ToList());
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
            Article article = db.Articles.Find(id);
            db.Articles.Remove(article);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // News from other resources
        public string CNN_News()
        {
            if (!Request.IsAuthenticated || User.Identity.GetPermission() == 0) // Checks if the user is logged in and has access
                return "Not logged in";

            GetNews gn = new GetNews();
            List<string[]> lst = gn.Add_CNN_News();
            Category c = new Category();
            c.Color = "blue";
            c.Name = "blue";
            c.CategoryId = 0;

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext()
                .GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            foreach (string[] str in lst)
            {
                Article a = new Article();
                a.ArticleLink = str[2];
                a.Category = c;
                a.Date = DateTime.Now;
                a.Description = str[1];
                a.ImageLink = str[3];
                a.Title = str[0];
                a.User = user;
                a.NumOfLikes = 0;
                Create(a);
            }

            return "Done.";
        }

        public void FOX_News()
        {
            if (!Request.IsAuthenticated || User.Identity.GetPermission() == 0) // Checks if the user is logged in and has access
                return;

            GetNews gn = new GetNews();
            List<string[]> lst = gn.Add_FOX_News();
            Category c = new Category();
            c.Color = "blue";
            c.Name = "blue";
            c.CategoryId = 0;

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext()
                .GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            foreach (string[] str in lst)
            {
                Article a = new Article();
                a.ArticleLink = str[2];
                a.Category = c;
                a.Date = DateTime.Now;
                a.Description = str[1];
                a.ImageLink = str[3];
                a.Title = str[0];
                a.User = user;
                a.NumOfLikes = 0;

                this.Create(a);
            }
        }

        public void YNET_News()
        {
            if (!Request.IsAuthenticated || User.Identity.GetPermission() == 0) // Checks if the user is logged in and has access
                return;

            GetNews gn = new GetNews();
            List<string[]> lst = gn.Add_Ynet_News();
            Category c = new Category();
            c.Color = "blue";
            c.Name = "blue";
            c.CategoryId = 0;

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext()
                .GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            foreach (string[] str in lst)
            {
                Article a = new Article();
                a.ArticleLink = str[2];
                a.Category = c;
                a.Date = DateTime.Now;
                a.Description = str[1];
                a.ImageLink = str[3];
                a.Title = str[0];
                a.User = user;
                a.NumOfLikes = 0;
                this.Create(a);
            }
        }
    }
}
