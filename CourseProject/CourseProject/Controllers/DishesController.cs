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
using System.Data.Entity.Infrastructure;

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
            List<string> positions = new List<string>();
            List<string> ingredients = new List<string>();
            foreach (var pos in db.PositionsInDishes.Where(p => p.DishID == id).ToList())
            {
                positions.Add(db.Positions.Where(p => p.ID == pos.PositionID).First().Name);
                int ingrID = db.IngredientsOnPositions.Where(p => p.PositionInDishID == pos.ID).First().IngredientID;
                ingredients.Add(db.Ingredients.Where(i => i.ID == ingrID).First().Name);
            }

            ViewData["Position"] = new SelectList(positions);
            ViewData["Ingredient"] = new SelectList(ingredients);

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

            if (ModelState.IsValid && file != null && file.ContentLength > 0)
            {
                List<int> pos_list = new List<int>();
                string pos_str = "";
                foreach (var pos in Request.Form["Positions"].ToList())
                {
                    pos_str += pos;
                }
                string[] pos_arr = pos_str.Split(',');
                foreach (var str in pos_arr)
                {
                    pos_list.Add(Convert.ToInt32(str));
                }


                List<int> ingr_list = new List<int>();
                string ingr_str = "";
                foreach (var ingr in Request.Form["Ingredients"].ToList())
                {
                    ingr_str += ingr;
                }
                string[] ingr_arr = ingr_str.Split(',');
                foreach (var str in ingr_arr)
                {
                    ingr_list.Add(Convert.ToInt32(str));
                }

                foreach (var ingr in ingr_list)
                {
                    double? wght = db.Ingredients.Where(i => i.ID == ingr).First().Weight;
                    if (dishes.Weight == null)
                    {
                        dishes.Weight = wght;
                    }
                    else
                    {
                        dishes.Weight += wght;
                    }
                }
                // extract only the fielname
                var fileName = Path.GetFileName(file.FileName);
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                file.SaveAs(path);
                dishes.ImageName = "/Images/" + fileName;
                db.Dishes.Add(dishes);
                db.SaveChanges();

                for (int i = 0; i < pos_list.Count; i++)
                {
                    PositionsInDish positionsInDish = new PositionsInDish();
                    IngredientsOnPosition ingredientsOnPosition = new IngredientsOnPosition();

                    positionsInDish.DishID = GetID();
                    positionsInDish.PositionID = pos_list[i];
                    db.PositionsInDishes.Add(positionsInDish);
                    db.SaveChanges();

                    ingredientsOnPosition.PositionInDishID = positionsInDish.ID;
                    ingredientsOnPosition.IngredientID = ingr_list[i];
                    db.IngredientsOnPositions.Add(ingredientsOnPosition);
                    db.SaveChanges();

                    //double? wght = db.Ingredients.Find(ingredientsOnPosition.IngredientID).Weight;
                    //dishes.Weight += wght;
                    //db.Entry(dishes).State = EntityState.Modified;
                    //db.SaveChanges();
                }

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
            List<string> positions = new List<string>();
            List<int> ingredients = new List<int>();
            foreach (var pos in db.PositionsInDishes.Where(p => p.DishID == id).ToList())
            {
                positions.Add(db.Positions.Where(p => p.ID == pos.PositionID).First().Name);
                int ingrID = db.IngredientsOnPositions.Where(p => p.PositionInDishID == pos.ID).First().IngredientID;
                ingredients.Add(ingrID);
            }
            ViewData["Position"] = new SelectList(positions);
            ViewData["Ingredient"] = new SelectList(ingredients);
            ViewData["Ingredients"] = new SelectList(db.Ingredients, "ID", "Name");

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
        public ActionResult Edit([Bind(Include = "ID,Weight,Name,Type,Cost,ImageName")] Dish dishes, HttpPostedFileBase file)
        {
            List<int> ingr_list = new List<int>();
            string ingr_str = "";
            foreach (var ingr in Request.Form["Ingredients"].ToList())
            {
                ingr_str += ingr;
            }
            string[] ingr_arr = ingr_str.Split(',');
            foreach (var str in ingr_arr)
            {
                ingr_list.Add(Convert.ToInt32(str));
            }
            ingr_list.Remove(ingr_list.Last());

            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string fileName = System.IO.Path.GetFileName(file.FileName);
                    file.SaveAs(Server.MapPath("~/Images/" + fileName));
                    db.Entry(dishes).State = EntityState.Modified;
                    dishes.ImageName = "/Images/" + fileName;
                    db.SaveChanges();
                }
                else
                {
                    db.Entry(dishes).State = EntityState.Modified;
                    db.SaveChanges();
                }

                int counter = 0;
                foreach (var pos_id in db.PositionsInDishes.Where(p => p.DishID == dishes.ID).Select(p => p.ID).ToList())
                {
                    IngredientsOnPosition ingredientsOnPosition = db.IngredientsOnPositions.Where(i => i.PositionInDishID == pos_id).First();
                    ingredientsOnPosition.IngredientID = ingr_list[counter];
                    db.Entry(ingredientsOnPosition).State = EntityState.Modified;
                    db.SaveChanges();
                    counter++;
                }

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
                        dish_id = Convert.ToInt32(id);
                    }
                }
                reader.Close();
            }
            return dish_id;
        }
    }
}