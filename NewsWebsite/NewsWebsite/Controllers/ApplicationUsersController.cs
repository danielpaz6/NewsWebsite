using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using NewsWebsite.Extensions;
using NewsWebsite.Models;

namespace NewsWebsite.Controllers
{
    public class ApplicationUsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ApplicationUsers
        public ActionResult Index(string username)
        {
            var users = from m in db.Users select m;
            if (!String.IsNullOrEmpty(username))
                users = users.Where(x => x.UserName.Contains(username));

            return View(users);
        }

        // GET: ApplicationUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // GET: ApplicationUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApplicationUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Permission,StatsLastUpdate,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (Request.IsAuthenticated && User.Identity.GetPermission() != 0) // Checks if the user is logged in and has access
            {
                if (ModelState.IsValid)
                {
                    db.Users.Add(applicationUser);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(applicationUser);
        }

        // GET: ApplicationUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Permission,StatsLastUpdate,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (Request.IsAuthenticated && User.Identity.GetPermission() != 0) // Checks if the user is logged in and has access
            {
                if (ModelState.IsValid)
                {
                    db.Entry(applicationUser).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(applicationUser);
        }

        // GET: ApplicationUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (Request.IsAuthenticated && User.Identity.GetPermission() != 0) // Checks if the user is logged in and has access
            {
                // reference to the user that we are about to delete
                ApplicationUser applicationUser = db.Users.Find(id);

                // delete all DistributionByCategory of the specific user that we are about to delete
                applicationUser.DistributionByCategory.Clear();

                // delete all the articles of the current user
                var articles = db.Articles.ToList();
                articles.RemoveAll(x => x.User == applicationUser);

                // delete all the views of the current user
                var views = db.Views.ToList();
                views.RemoveAll(x => x.User == applicationUser);

                // checking the case that the user deleting his own user
                var userId = User.Identity.GetUserId();

                if (userId == applicationUser.Id) // If we enter this condition, it means that the user has been deleted but the cookies/session is still alive.
                {
                    // Log off ( remove cookies & session )
                    HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                }

                // remove the user itself
                db.Users.Remove(applicationUser);

                db.SaveChanges();
            }

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
    }
}
