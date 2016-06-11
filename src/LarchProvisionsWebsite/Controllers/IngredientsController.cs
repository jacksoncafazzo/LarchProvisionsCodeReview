using LarchProvisionsWebsite.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using System.Linq;

namespace LarchProvisionsWebsite.Controllers
{
    [Authorize(Roles = "Chef")]
    public class IngredientsController : Controller
    {
        private LarchKitchenDbContext _context;

        public IngredientsController(LarchKitchenDbContext context)
        {
            _context = context;
        }

        // GET: Ingredients
        public IActionResult Index()
        {
            var ingredients = _context.Ingredients.ToList();
            foreach (var ingredient in ingredients)
            {
                ingredient.Preps = _context.Preps.Where(p => p.IngredientId == ingredient.IngredientId).ToList();
            }
            return View(ingredients);
        }

        // GET: Ingredients/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Ingredient ingredient = _context.Ingredients.FirstOrDefault(m => m.IngredientId == id);
            if (ingredient == null)
            {
                return HttpNotFound();
            }

            return View(ingredient);
        }

        // GET: Ingredients/Create
        public IActionResult Create(string returnUrl = null)
        {
            if (returnUrl == null)
            {
                returnUrl = "/Ingredients/";
            }
            ViewData["RecipeId"] = returnUrl.Remove(0, 8);
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        // POST: Ingredients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Ingredient ingredient, string returnUrl = null, string recipeId = null)
        {
            if (ModelState.IsValid)
            {
                _context.Ingredients.Add(ingredient);
                _context.SaveChanges();
                if (returnUrl == null)
                    return RedirectToAction("Edit", "Recipes", new { id = recipeId });
                else if (Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction("Index");
            }
            return View("Index");
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