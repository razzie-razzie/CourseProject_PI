using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CourseProject;
using System.IO;

namespace CourseProject.Controllers
{
    public class DishesController : Controller
    {
        private DinnersDBEntities db = new DinnersDBEntities();

        // GET: Dishes
        public ActionResult Index()
        {
            var dishes = db.Dishes.Include(d => d.DishesType);
            return View(dishes.ToList());
        }

        // GET: Dishes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dish dishes = db.Dishes.Find(id);
            PositionsInDish positionsInDish = db.PositionsInDishes.Where(p => p.DishID == id).First();
            IngredientsOnPosition ingredientsOnPosition = db.IngredientsOnPositions.Where(p => p.PositionInDishID == positionsInDish.ID).First();
            ViewData["Position"] = db.Positions.Where(pos => pos.ID == positionsInDish.PositionID).First().Name;
            ViewData["Ingredient"] = db.Ingredients.Where(ing => ing.ID == ingredientsOnPosition.IngredientID).First().Name;
            if (dishes == null)
            {
                return HttpNotFound();
            }
            return View(dishes);
        }

        // GET: Dishes/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            ViewBag.Type = new SelectList(db.DishesTypes, "ID", "Type");
            ViewData["Positions"] = new SelectList(db.Positions, "ID", "Name");
            ViewData["Ingredients"] = new SelectList(db.Ingredients, "ID", "Name");
            return View();
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Create([Bind(Include = "ID,Name,Type,Cost")] Dish dishes, HttpPostedFileBase file)
        {
            ViewData["Positions"] = new SelectList(db.Positions, "ID", "Name");
            ViewData["Ingredients"] = new SelectList(db.Ingredients, "ID", "Name");

            IngredientsOnPosition ingredientsOnPosition = new IngredientsOnPosition();
            PositionsInDish positionsInDish = new PositionsInDish();

            if (ModelState.IsValid && file != null && file.ContentLength > 0)
            {
                positionsInDish.DishID = GetID();
                positionsInDish.PositionID = Convert.ToInt32(Request.Form["Positions"].ElementAt(0).ToString());
                db.PositionsInDishes.Add(positionsInDish);

                ingredientsOnPosition.PositionInDishID = positionsInDish.ID;
                ingredientsOnPosition.IngredientID = Convert.ToInt32(Request.Form["Ingredients"].ElementAt(0).ToString());
                db.IngredientsOnPositions.Add(ingredientsOnPosition);

                double? wght = db.Ingredients.Find(ingredientsOnPosition.IngredientID).Weight;
                dishes.Weight = wght;


                // extract only the fielname
                var fileName = Path.GetFileName(file.FileName);
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                file.SaveAs(path);

                dishes.ImageName = "/Images/" + fileName;
                db.Dishes.Add(dishes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Type = new SelectList(db.DishesTypes, "ID", "Type", dishes.Type);
            return View(dishes);
        }

        // GET: Dishes/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dish dishes = db.Dishes.Find(id);
            if (dishes == null)
            {
                return HttpNotFound();
            }
            ViewBag.Type = new SelectList(db.DishesTypes, "ID", "Type", dishes.Type);
            return View(dishes);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Edit([Bind(Include = "ID,Name,Type,Cost")] Dish dishes, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                    file.SaveAs(path);

                    dishes.ImageName = "/Images/" + fileName;
                }
                else
                {
                    dishes.ImageName = dishes.ImageName;
                }
                dishes.Weight = dishes.Weight;
                db.Entry(dishes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Type = new SelectList(db.DishesTypes, "ID", "Type", dishes.Type);
            return View(dishes);
        }

        // GET: Dishes/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dish dishes = db.Dishes.Find(id);
            if (dishes == null)
            {
                return HttpNotFound();
            }
            return View(dishes);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Dish dishes = db.Dishes.Find(id);
            db.Dishes.Remove(dishes);

            List<PositionsInDish> positionsInDish = db.PositionsInDishes.Where(d => d.DishID == dishes.ID).ToList();
            foreach (var pos in positionsInDish)
            {
                db.PositionsInDishes.Remove(pos);
                List<IngredientsOnPosition> ingredientsOnPositions = db.IngredientsOnPositions.Where(d => d.PositionInDishID == pos.ID).ToList();
                foreach (var poss in ingredientsOnPositions)
                {
                    db.IngredientsOnPositions.Remove(poss);
                }
            }

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


        public int GetID()
        {
            string connectionString = @"data source=MARINUSIK;initial catalog=DinnersDB;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework;";
            string sqlExpression = "SELECT IDENT_CURRENT('Dishes') AS CURR_ID;";
            int dish_id = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        object id = reader["CURR_ID"];
                        dish_id = Convert.ToInt32(id) + 1;
                    }
                }
                reader.Close();
            }
            return dish_id;
        }
    }
}