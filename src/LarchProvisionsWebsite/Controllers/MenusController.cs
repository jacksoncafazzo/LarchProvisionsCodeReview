using LarchProvisionsWebsite.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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

        //GET: Menus/TestPage
        public IActionResult TestPage()
        {
            return View();
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
            if (menu == null)
            {
                return HttpNotFound();
            }

            return RedirectToAction("Details", new { id = menu.MenuId });
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
            var recipes = GetRecipes(menu.MenuId);
            if (User.IsSignedIn())
            {
                var user = GetUser().Result;
                ViewData["UserName"] = user.UserName;
                ViewData["UserId"] = user.Id;

                var totalPrice = 0;
                foreach (var recipe in recipes)
                {
                    var stackedRecipe = StackRecipeOrders(recipe, menu, user);
                    foreach (var order in stackedRecipe.Orders)
                    {
                        order.ApplicationUser = user;
                        order.Recipe = stackedRecipe;
                        var monies = stackedRecipe.CustPrice * order.OrderSize;
                        totalPrice = totalPrice + monies;
                    }
                    menu.Recipes.Remove(recipe);
                    menu.Recipes.Add(stackedRecipe);
                }
                ViewData["CustTotal"] = totalPrice;
            }
            return View(menu);
        }

        //stack orders for display
        [NonAction]
        public Recipe StackRecipeOrders(Recipe recipe, Menu menu, ApplicationUser user)
        {
            recipe.Orders = _context.Orders.Where(o => o.RecipeId == recipe.RecipeId && o.MenuId == menu.MenuId && o.UserId == user.Id).ToList();
            return recipe;
        }

        ////POST PlaceOrderAjax
        //public IActionResult GetRecipeOrders(int menuId, int recipeId, ApplicationUser user)
        //{
        //    var recipe = _context.Recipes.FirstOrDefault(r => r.RecipeId == recipeId);
        //    var menu = _context.Menus.FirstOrDefault(m => m.MenuId == menuId);
        //    var order = new Order(user, recipe, menu);
        //    order = StackOrder(order, menuId, recipeId, user);
        //    _context.Orders.Add(order);
        //    _context.SaveChanges();
        //    var recipeOrders = _context.Orders.Where(o => o.RecipeId == recipeId && o.MenuId == menuId).ToList();
        //    return Json(recipeOrders);
        //}

        //Post Menus/Order
        public IActionResult Order(int menuId, int recipeId, ApplicationUser user)
        {
            if (User.IsSignedIn())
            {
                user = GetUser().Result;
            }
            var order = new Order();
            var recipes = GetRecipes(menuId);
            var menu = _context.Menus.FirstOrDefault(m => m.MenuId == menuId);
            order = StackOrder(order, menuId, recipeId, user);

            _context.Orders.Add(order);
            _context.SaveChanges();
            var totalPrice = 0;
            foreach (var lilrecipe in recipes.ToList())
            {
                var recipe = StackRecipeOrders(lilrecipe, order.Menu, user);
                var monies = lilrecipe.CustPrice * lilrecipe.Orders.Count;
                totalPrice = totalPrice + monies;
                recipes.Remove(lilrecipe);
                menu.Recipes.Add(recipe);
            }
            ViewData["CustTotal"] = totalPrice;
            ViewData["UserName"] = user.UserName;
            return View("Details", menu);
        }

        // get that info on there
        [NonAction]
        public Order StackOrder(Order order, int menuId, int recipeId, ApplicationUser user)
        {
            order.MenuId = menuId;
            order.RecipeId = recipeId;
            order.Recipe = _context.Recipes.FirstOrDefault(r => r.RecipeId == recipeId);
            order.RecipeName = order.Recipe.RecipeName;
            order.UserId = user.Id;
            order.ApplicationUser = user;
            order.OrderSize = 1;
            order.UserName = user.UserName;
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
            Menu menu = _context.Menus.FirstOrDefault(m => m.MenuId == id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            var user = GetUser().Result;
            var recipes = GetRecipes(menu.MenuId);

            foreach (var recipe in recipes)
            {
                menu.Recipes.Remove(recipe);
                menu.Recipes.Add(StackRecipe(recipe, menu, user));
            }
            ViewBag.AllRecipes = _context.Recipes.ToList().Except(menu.Recipes);
          

            //ViewBag.AllOrders = _context.Orders.ToList().Except(menu.Orders);

            ViewData["ReturnUrl"] = "/Menus/Edit/" + id;
            return View(menu);
        }

        // stack em bro
        [NonAction]
        public Recipe StackRecipe(Recipe recipe, Menu menu, ApplicationUser user)
        {
            recipe.Ingredients = _context.Ingredients.Join(_context.Preps.Where(p => p.RecipeId == recipe.RecipeId).ToList(),
            i => i.IngredientId,
            p => p.IngredientId,
            (o, i) => o).ToList();
            recipe.Orders = _context.Orders.Where(o => o.RecipeId == recipe.RecipeId && o.MenuId == menu.MenuId).ToList();

            return recipe;
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

        // Post: Menus/RemoveOrder
        public IActionResult RemoveOrder(int menuId, int recipeId)
        {
            var user = GetUser().Result;
            var order = _context.Orders.FirstOrDefault(o => o.RecipeId == recipeId && o.MenuId == menuId);
            _context.Orders.Remove(order);
            _context.SaveChanges();

            var recipes = GetRecipes(menuId);
            var menu = _context.Menus.FirstOrDefault(m => m.MenuId == menuId);
            foreach (var recipe in recipes)
            {
                menu.Recipes.Remove(recipe);
                menu.Recipes.Add(StackRecipe(recipe, menu, user));
            }
            return RedirectToAction("CurrentMenu");
        }

        // POST: /Menus/Edit/RemoveOrderAjax
        public IActionResult RemoveOrderAjax(int menuId, int recipeId)
        {
            var user = GetUser().Result;
            var order = _context.Orders.FirstOrDefault(o => o.RecipeId == recipeId && o.MenuId == menuId);
            _context.Orders.Remove(order);
            _context.SaveChanges();
            var recipe = _context.Menus.FirstOrDefault(m => m.MenuId == menuId);
            //var recipes = GetRecipes(menu.MenuId);
            //foreach (var recipe in recipes)
            //{
            //    menu.Recipes.Remove(recipe);
            //    menu.Recipes.Add(StackRecipe(recipe, menu, user));
            //}
            return Json(recipe);
        }

        public IActionResult RemoveRecipe(int menuId, int recipeId)
        {
            Serving serving = _context.Servings.FirstOrDefault(s => s.MenuId == menuId && s.RecipeId == recipeId);
            _context.Servings.Remove(serving);
            _context.SaveChanges();
            var menu = _context.Menus.Where(m => m.MenuId == menuId);
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

        // Get User
        [NonAction]
        public async Task<ApplicationUser> GetUser()
        {
            return await _userdb.Users.FirstOrDefaultAsync(u => u.Id == User.GetUserId());
        }

        //Get order
        [NonAction]
        public async Task<Order> GetOrder(int recipeId)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.UserId == User.GetUserId() && o.RecipeId == recipeId);
        }

        //Get orders for a recipe
        [NonAction]
        public ICollection<Order> GetOrders(int recipeId, ApplicationUser user)
        {
            return _context.Orders.Join(
                _userdb.Users.Where(
                u => u.Id == user.Id).ToList(),
                o => o.UserId,
                u => u.Id,
                (o, i) => o).ToList();
        }

        // get served recipes for menu
        [NonAction]
        public ICollection<Recipe> GetRecipes(int menuId)
        {
            return _context.Recipes.Join(
                _context.Servings.Where(
                s => s.MenuId == menuId).ToList(),
                r => r.RecipeId,
                s => s.RecipeId,
                (o, i) => o).ToList();
        }

        // GET: Menus/RecipesDisplay
        public IActionResult RecipesDisplay()
        {
            var menu = _context.Menus.Last();
            if (menu == null)
            {
                return HttpNotFound();
            }
            var user = GetUser().Result;
            var recipes = GetRecipes(menu.MenuId);

            foreach (var recipe in recipes)
            {
                menu.Recipes.Remove(recipe);
                menu.Recipes.Add(StackRecipe(recipe, menu, user));
            }
            var totalPrice = 0;

            foreach (var recipe in menu.Recipes)
            {
                var monies = recipe.CustPrice * recipe.Orders.Count;
                totalPrice = totalPrice + monies;
            }
            ViewData["CustTotal"] = totalPrice;
            ViewBag.AllRecipes = _context.Recipes.ToList().Except(recipes);

            ViewBag.AllOrders = _context.Orders.ToList().Except(menu.Orders);

            ViewData["ReturnUrl"] = "/Menus/Display/" + menu.MenuId;

            return View(menu);

            //var user = GetUser().Result;
            //var recipes = GetRecipes(menu.MenuId);

            //foreach (var recipe in recipes)
            //{
            //    var thisRecipe = StackRecipeOrders(recipe, menu);
            //    foreach (var order in thisRecipe.Orders)
            //    {
            //        var thisOrder = StackOrder(order, menu.MenuId, recipe.RecipeId, user);
            //    }
            //    menu.Recipes.Add(thisRecipe);
            //}
            //ViewData["CurrentMenu"] = menu;
            //return View(menu);
        }
    }
}