using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using LarchProvisionsWebsite.Models;

namespace LarchProvisionsWebsite.Controllers
{
    public class RecipesController : Controller
    {
        private ApplicationDbContext _context;

        public RecipesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Recipes
        public IActionResult Index()
        {
            var applicationDbContext = _context.Recipes.Include(r => r.Menu);
            return View(applicationDbContext.ToList());
        }

        // GET: Recipes/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Recipe recipe = _context.Recipes.Single(m => m.RecipeId == id);
            if (recipe == null)
            {
                return HttpNotFound();
            }

            return View(recipe);
        }

        // GET: Recipes/Create
        public IActionResult Create()
        {
            ViewData["MenuId"] = new SelectList(_context.Menus, "MenuId", "Menu");
            return View();
        }

        // POST: Recipes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                _context.Recipes.Add(recipe);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["MenuId"] = new SelectList(_context.Menus, "MenuId", "Menu", recipe.MenuId);
            return View(recipe);
        }

        // GET: Recipes/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Recipe recipe = _context.Recipes.Single(m => m.RecipeId == id);
            if (recipe == null)
            {
                return HttpNotFound();
            }
            ViewData["MenuId"] = new SelectList(_context.Menus, "MenuId", "Menu", recipe.MenuId);
            return View(recipe);
        }

        // POST: Recipes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                _context.Update(recipe);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["MenuId"] = new SelectList(_context.Menus, "MenuId", "Menu", recipe.MenuId);
            return View(recipe);
        }

        // GET: Recipes/Delete/5
        [ActionName("Delete")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Recipe recipe = _context.Recipes.Single(m => m.RecipeId == id);
            if (recipe == null)
            {
                return HttpNotFound();
            }

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Recipe recipe = _context.Recipes.Single(m => m.RecipeId == id);
            _context.Recipes.Remove(recipe);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
