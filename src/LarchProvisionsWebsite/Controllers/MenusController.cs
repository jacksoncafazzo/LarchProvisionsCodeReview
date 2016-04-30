using LarchProvisionsWebsite.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using System.Linq;
using System.Security.Claims;

namespace LarchProvisionsWebsite.Controllers
{
    public class MenusController : Controller
    {
        private LarchKitchenDbContext _context;
        private ApplicationDbContext _userdb;
        private readonly UserManager<ApplicationUser> _userManager;

        public MenusController(LarchKitchenDbContext context, ApplicationDbContext userdb, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userdb = userdb;
            _userManager = userManager;
        }

        // GET: Menus
        public IActionResult Index()
        {
            return View(_context.Menus.ToList());
        }

        //Get: Menus/CurrentMenu
        public IActionResult CurrentMenu()
        {
            var menu = _context.Menus.Last();
            menu.Recipes = _context.Recipes.Join(
                _context.Servings.Where(
                s => s.MenuId == menu.MenuId).ToList(),
                s => s.RecipeId,
                s => s.RecipeId,
                (o, i) => o).ToList();
            return View("Details", menu);
        }

        // GET: Menus/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Menu menu = _context.Menus.FirstOrDefault(m => m.MenuId == id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            menu.Recipes = _context.Recipes.Join(
                _context.Servings.Where(
                s => s.MenuId == id).ToList(),
                s => s.RecipeId,
                s => s.RecipeId,
                (o, i) => o).ToList();

            return View(menu);
        }

        //Post CurrentMenu/Order
        public IActionResult Order(int menuId, int recipeId, ApplicationUser user)
        {
            var menu = _context.Menus.FirstOrDefault(m => m.MenuId == menuId);
            menu.Recipes = _context.Recipes.Join(
                _context.Servings.Where(
                s => s.MenuId == menuId).ToList(),
                s => s.RecipeId,
                s => s.RecipeId,
                (o, i) => o).ToList();
            var order = new Order();
            order = StackOrder(order, menuId, recipeId, user, menu);
            _context.Orders.Add(order);
            _context.SaveChanges();
            var orders = _context.Orders.Where(o => o.MenuId == menuId).ToList();
            foreach (var o in menu.Orders)
            {
                var stackedOrder = StackOrder(o, menuId, recipeId, user, menu);
                menu.Orders.Add(stackedOrder);
            }
            return View("Details", menu);
        }

        [NonAction]
        public Order StackOrder(Order order, int menuId, int recipeId, ApplicationUser user, Menu menu)
        {
            order.MenuId = menuId;
            order.RecipeId = recipeId;
            order.Recipe = _context.Recipes.FirstOrDefault(r => r.RecipeId == recipeId);
            order.UserId = User.GetUserId();
            order.ApplicationUser = _userdb.Users.FirstOrDefaultAsync(u => u.Id == order.UserId).Result;
            order.OrderSize = +1;
            order.Menu = menu;
            return order;
        }

        // GET: Menus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Menus/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Menu menu)
        {
            if (ModelState.IsValid)
            {
                _context.Menus.Add(menu);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(menu);
        }

        // GET: Menus/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var user = _userdb.Users.FirstOrDefaultAsync(u => u.Id == User.GetUserId()).Result;
            Menu menu = _context.Menus.FirstOrDefault(m => m.MenuId == id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            var recipes = _context.Recipes.Join(
                _context.Servings.Where(
                s => s.MenuId == id).ToList(),
                s => s.RecipeId,
                s => s.RecipeId,
                (o, i) => o).ToList();
            foreach (var recipe in recipes)
            {
                recipe.Preps = _context.Preps.Where(p => p.RecipeId == id).ToList();
                recipe.Ingredients = _context.Ingredients.Join(_context.Preps.Where(p => p.RecipeId == recipe.RecipeId).ToList(),
                i => i.IngredientId,
                p => p.IngredientId,
                (o, i) => o).ToList();
                var orders = _context.Orders.Where(o => o.RecipeId == recipe.RecipeId).ToList();
                foreach (var o in orders)
                {
                    var stackedOrder = StackOrder(o, menu.MenuId, recipe.RecipeId, user, menu);
                    recipe.Orders.Add(stackedOrder);
                }
            }
            menu.Recipes = recipes;
            ViewBag.Recipes = _context.Recipes.ToList().Except(menu.Recipes);
            menu.Orders = _context.Orders.Where(o => o.MenuId == id).ToList();

            ViewBag.Orders = _context.Orders.ToList().Except(menu.Orders);

            ViewData["ReturnUrl"] = "/Menus/Edit/" + id;
            return View(menu);
        }

        // POST: Menus/ServeRecipe
        public IActionResult ServeRecipe(int menuId, int recipeId)
        {
            Serving serving = new Serving();
            serving.RecipeId = recipeId;
            serving.MenuId = menuId;
            _context.Servings.Add(serving);
            _context.SaveChanges();
            return RedirectToAction("Edit", new { id = menuId });
        }

        public IActionResult RemoveRecipe(int menuId, int recipeId)
        {
            Serving serving = _context.Servings.FirstOrDefault(s => s.MenuId == menuId && s.RecipeId == recipeId);
            _context.Servings.Remove(serving);
            _context.SaveChanges();
            return RedirectToAction("Edit", new { id = menuId });
        }

        // POST: Menus/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Menu menu)
        {
            if (ModelState.IsValid)
            {
                _context.Update(menu);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(menu);
        }

        // GET: Menus/Delete/5
        [ActionName("Delete")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Menu menu = _context.Menus.Single(m => m.MenuId == id);
            if (menu == null)
            {
                return HttpNotFound();
            }

            return View(menu);
        }

        // POST: Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Menu menu = _context.Menus.Single(m => m.MenuId == id);
            _context.Menus.Remove(menu);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}