using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CourseProject;

namespace CourseProject.Controllers
{
    public class DishesTypesController : Controller
    {
        private DinnersDBEntities db = new DinnersDBEntities();

        // GET: DishesTypes
        public ActionResult Index()
        {
            return View(db.DishesTypes.ToList());
        }

        // GET: DishesTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DishesType dishesTypes = db.DishesTypes.Find(id);
            if (dishesTypes == null)
            {
                return HttpNotFound();
            }
            return View(dishesTypes);
        }

        // GET: DishesTypes/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: DishesTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Create([Bind(Include = "ID,Type")] DishesType dishesTypes)
        {
            if (ModelState.IsValid)
            {
                db.DishesTypes.Add(dishesTypes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dishesTypes);
        }

        // GET: DishesTypes/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DishesType dishesTypes = db.DishesTypes.Find(id);
            if (dishesTypes == null)
            {
                return HttpNotFound();
            }
            return View(dishesTypes);
        }

        // POST: DishesTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Edit([Bind(Include = "ID,Type")] DishesType dishesTypes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dishesTypes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dishesTypes);
        }

        // GET: DishesTypes/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DishesType dishesTypes = db.DishesTypes.Find(id);
            if (dishesTypes == null)
            {
                return HttpNotFound();
            }
            return View(dishesTypes);
        }

        // POST: DishesTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            DishesType dishesTypes = db.DishesTypes.Find(id);
            db.DishesTypes.Remove(dishesTypes);
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
