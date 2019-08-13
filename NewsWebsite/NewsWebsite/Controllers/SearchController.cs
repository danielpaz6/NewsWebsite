using NewsWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsWebsite.Controllers
{
    public class SearchController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Search
        public ActionResult Index(string title, string author, string desc, int? category, string source)
        {
            if (!Request.IsAuthenticated) // checks if the user is logged in
                return View();
            
            //IQueryable<Article> query = null;
            var query = from m in db.Articles select m;
            ViewBag.isSearched = false;

            if (!String.IsNullOrEmpty(title))
            {
                query = db.Articles.Where(article => article.Title.Contains(title));
                ViewBag.isSearched = true;
            }

            if (!String.IsNullOrEmpty(author)) {
                query = query.Where(article => article.User.UserName.Contains(author));
                ViewBag.isSearched = true;
            }

            if (!String.IsNullOrEmpty(desc))
            {
                query = query.Where(article => article.Description.Contains(desc));
                ViewBag.isSearched = true;
            }

            if (!String.IsNullOrEmpty(source))
            {
                query = query.Where(article => article.Source.Contains(source));
                ViewBag.isSearched = true;
            }

            if (category != null)
            {
                query = query.Where(article => article.CategoryId == category);
                ViewBag.isSearched = true;
            }

            if (!ViewBag.isSearched)
            {
                ViewBag.Categories = db.Categories.ToList();
                return View();
            }
            else
            {
                return View(query.ToList());
            }
        }

        // GET: Users Search
        public ActionResult Users(string username, string email, int? permission)
        {
            var query = from m in db.Users select m;
            if (!String.IsNullOrEmpty(username))
                query = query.Where(x => x.UserName.Contains(username));

            if (!String.IsNullOrEmpty(email))
                query = query.Where(x => x.Email.Contains(email));

            if (permission != null)
                query = query.Where(x => x.Permission == permission);

            return View(query.ToList());
        }
    }
}