using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CourseProject;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace CourseProject.Controllers
{
    public class IngredientsCategoriesController : Controller
    {
        private DinnersDBEntities db = new DinnersDBEntities();

        // GET: IngredientsCategories
        public ActionResult Index()
        {
            return View(db.IngredientsCategories.ToList());
        }

        // GET: IngredientsCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IngredientsCategory ingredientsCategories = db.IngredientsCategories.Find(id);
            if (ingredientsCategories == null)
            {
                return HttpNotFound();
            }
            return View(ingredientsCategories);
        }

        // GET: IngredientsCategories/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: IngredientsCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Create([Bind(Include = "ID,Category_Name")] IngredientsCategory ingredientsCategories)
        {
            if (ModelState.IsValid)
            {
                db.IngredientsCategories.Add(ingredientsCategories);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ingredientsCategories);
        }

        // GET: IngredientsCategories/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IngredientsCategory ingredientsCategories = db.IngredientsCategories.Find(id);
            if (ingredientsCategories == null)
            {
                return HttpNotFound();
            }
            return View(ingredientsCategories);
        }

        // POST: IngredientsCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Edit([Bind(Include = "ID,Category_Name")] IngredientsCategory ingredientsCategories)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ingredientsCategories).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ingredientsCategories);
        }

        // GET: IngredientsCategories/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IngredientsCategory ingredientsCategories = db.IngredientsCategories.Find(id);
            if (ingredientsCategories == null)
            {
                return HttpNotFound();
            }
            return View(ingredientsCategories);
        }

        // POST: IngredientsCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            IngredientsCategory ingredientsCategories = db.IngredientsCategories.Find(id);
            db.IngredientsCategories.Remove(ingredientsCategories);
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
    }
}
