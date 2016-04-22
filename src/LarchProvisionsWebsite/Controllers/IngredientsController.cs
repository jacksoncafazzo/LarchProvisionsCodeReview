using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using LarchProvisionsWebsite.Models;

namespace LarchProvisionsWebsite.Controllers
{
    public class IngredientsController : Controller
    {
        private ApplicationDbContext _context;

        public IngredientsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Ingredients
        public IActionResult Index()
        {
            return View(_context.Ingredients.ToList());
        }

        // GET: Ingredients/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Ingredient ingredient = _context.Ingredients.Single(m => m.IngredientId == id);
            if (ingredient == null)
            {
                return HttpNotFound();
            }

            return View(ingredient);
        }

        // GET: Ingredients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ingredients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                _context.Ingredients.Add(ingredient);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ingredient);
        }

        // GET: Ingredients/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Ingredient ingredient = _context.Ingredients.Single(m => m.IngredientId == id);
            if (ingredient == null)
            {
                return HttpNotFound();
            }
            return View(ingredient);
        }

        // POST: Ingredients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                _context.Update(ingredient);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ingredient);
        }

        // GET: Ingredients/Delete/5
        [ActionName("Delete")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Ingredient ingredient = _context.Ingredients.Single(m => m.IngredientId == id);
            if (ingredient == null)
            {
                return HttpNotFound();
            }

            return View(ingredient);
        }

        // POST: Ingredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Ingredient ingredient = _context.Ingredients.Single(m => m.IngredientId == id);
            _context.Ingredients.Remove(ingredient);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
