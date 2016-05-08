using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using LarchProvisionsWebsite.Models;

namespace LarchProvisionsWebsite.Controllers
{
    public class ServingsController : Controller
    {
        private ApplicationDbContext _context;

        public ServingsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Servings
        public IActionResult Index()
        {
            ViewData["dateTemplate"] = "yyyy-MM-dd";
            var applicationDbContext = _context.Serving.Include(s => s.Menu).Include(s => s.Recipe);
            return View(applicationDbContext.ToList());
        }

        // GET: Servings/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Serving serving = _context.Serving.Single(m => m.ServingId == id);
            if (serving == null)
            {
                return HttpNotFound();
            }

            return View(serving);
        }

        // GET: Servings/Create
        public IActionResult Create()
        {
            ViewData["MenuId"] = new SelectList(_context.Set<Menu>(), "MenuId", "Menu");
            ViewData["RecipeId"] = new SelectList(_context.Set<Recipe>(), "RecipeId", "Recipe");
            return View();
        }

        // POST: Servings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Serving serving)
        {
            if (ModelState.IsValid)
            {
                _context.Serving.Add(serving);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["MenuId"] = new SelectList(_context.Set<Menu>(), "MenuId", "Menu", serving.MenuId);
            ViewData["RecipeId"] = new SelectList(_context.Set<Recipe>(), "RecipeId", "Recipe", serving.RecipeId);
            return View(serving);
        }

        // GET: Servings/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Serving serving = _context.Serving.Single(m => m.ServingId == id);
            if (serving == null)
            {
                return HttpNotFound();
            }
            ViewData["MenuId"] = new SelectList(_context.Set<Menu>(), "MenuId", "Menu", serving.MenuId);
            ViewData["RecipeId"] = new SelectList(_context.Set<Recipe>(), "RecipeId", "Recipe", serving.RecipeId);
            return View(serving);
        }

        // POST: Servings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Serving serving)
        {
            if (ModelState.IsValid)
            {
                _context.Update(serving);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["MenuId"] = new SelectList(_context.Set<Menu>(), "MenuId", "Menu", serving.MenuId);
            ViewData["RecipeId"] = new SelectList(_context.Set<Recipe>(), "RecipeId", "Recipe", serving.RecipeId);
            return View(serving);
        }

        // GET: Servings/Delete/5
        [ActionName("Delete")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Serving serving = _context.Serving.Single(m => m.ServingId == id);
            if (serving == null)
            {
                return HttpNotFound();
            }

            return View(serving);
        }

        // POST: Servings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Serving serving = _context.Serving.Single(m => m.ServingId == id);
            _context.Serving.Remove(serving);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
