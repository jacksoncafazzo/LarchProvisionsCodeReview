using LarchProvisionsWebsite.Models;
using Microsoft.AspNet.Authorization;
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

        // GET: Menus
        public async Task<IActionResult> Index()
        {
            return View(await _context.Menus.ToListAsync());
        }

        //Get: Menus/CurrentMenu
        public async Task<IActionResult> CurrentMenu()
        {
            var menu = await _context.Menus.LastAsync();
            if (menu == null)
            {
                return HttpNotFound();
            }

            return RedirectToAction("Details", new { id = menu.MenuId });
        }

        // GET: Menus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Menu menu = await _context.Menus.FirstOrDefaultAsync(m => m.MenuId == id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            ViewData["MenuId"] = menu.MenuId;

            var recipes = GetRecipes(menu.MenuId).Result;
            var user = new ApplicationUser();
            if (User.IsSignedIn())
            {
                user = GetUser().Result;
            }
            ViewData["UserName"] = user.UserName;
            ViewData["UserId"] = user.Id;

            var totalPrice = 0;
            foreach (var recipe in recipes)
            {
                var stackedRecipe = StackRecipeOrders(recipe, menu, user).Result;
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

            return View(menu);
        }

        //stack orders for display
        [NonAction]
        public async Task<Recipe> StackRecipeOrders(Recipe recipe, Menu menu, ApplicationUser user)
        {
            recipe.Orders = await _context.Orders.Where(o => o.RecipeId == recipe.RecipeId && o.MenuId == menu.MenuId && o.UserId == user.Id).ToListAsync();
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
        public async Task<IActionResult> Order(int menuId, int recipeId, ApplicationUser user)
        {
            if (User.IsSignedIn())
            {
                user = GetUser().Result;
            }
            var recipes = GetRecipes(menuId).Result;
            var menu = await _context.Menus.FirstOrDefaultAsync(m => m.MenuId == menuId);
            var order = new Order();
            var previousOrder = await _context.Orders.SingleOrDefaultAsync(o => o.MenuId == menuId && o.RecipeId == recipeId && o.UserId == user.Id);
            if (previousOrder != null)
            {
                previousOrder.OrderSize = +1;
                order = previousOrder;
                order = StackOrder(order, menuId, recipeId, user, previousOrder.OrderSize);
                _context.Orders.Update(order);
            }
            else if (previousOrder == null)
            {
                order = StackOrder(order, menuId, recipeId, user, 1);
                _context.Orders.Add(order);
            }
            await _context.SaveChangesAsync();
            //var totalPrice = 0;
            //foreach (var lilrecipe in recipes.ToList())
            //{
            //    var recipe = StackRecipeOrders(lilrecipe, order.Menu, user).Result;
            //    var monies = lilrecipe.CustPrice * lilrecipe.Orders.Count;
            //    totalPrice = totalPrice + monies;
            //    recipes.Remove(lilrecipe);
            //    menu.Recipes.Add(recipe);
            //}
            //ViewData["CustTotal"] = totalPrice;
            //ViewData["UserName"] = user.UserName;
            return RedirectToAction("Details", new { id = menu.MenuId });
        }

        // get that info on there
        [NonAction]
        public Order StackOrder(Order order, int menuId, int recipeId, ApplicationUser user, int orderSize)
        {
            order.MenuId = menuId;
            order.RecipeId = recipeId;
            order.Recipe = _context.Recipes.FirstOrDefault(r => r.RecipeId == recipeId);
            order.RecipeName = order.Recipe.RecipeName;
            order.UserId = user.Id;
            order.ApplicationUser = user;
            order.OrderSize = orderSize;
            order.UserName = user.UserName;
            return order;
        }

        [Authorize(Roles = "Chef")]
        // GET: Menus/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Chef")]
        // POST: Menus/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Menu menu)
        {
            if (ModelState.IsValid)
            {
                _context.Menus.Add(menu);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(menu);
        }

        [Authorize(Roles = "Chef")]
        // GET: Menus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Menu menu = await _context.Menus.FirstOrDefaultAsync(m => m.MenuId == id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            var user = GetUser().Result;
            var recipes = GetRecipes(menu.MenuId).Result;

            foreach (var recipe in recipes)
            {
                menu.Recipes.Remove(recipe);
                menu.Recipes.Add(StackRecipe(recipe, menu, user).Result);
            }
            ViewBag.AllRecipes = _context.Recipes.ToList().Except(menu.Recipes);

            //ViewBag.AllOrders = _context.Orders.ToList().Except(menu.Orders);

            ViewData["ReturnUrl"] = "/Menus/Edit/" + id;
            return View(menu);
        }

        // stack em bro
        [NonAction]
        public async Task<Recipe> StackRecipe(Recipe recipe, Menu menu, ApplicationUser user)
        {
            recipe.Ingredients = await _context.Ingredients.Join(_context.Preps.Where(p => p.RecipeId == recipe.RecipeId).ToList(),
            i => i.IngredientId,
            p => p.IngredientId,
            (o, i) => o).ToListAsync();
            recipe.Orders = await _context.Orders.Where(o => o.RecipeId == recipe.RecipeId && o.MenuId == menu.MenuId).ToListAsync();
            return recipe;
        }

        // POST: Menus/ServeRecipe
        public async Task<IActionResult> ServeRecipe(int menuId, int recipeId)
        {
            Serving serving = new Serving();
            serving.RecipeId = recipeId;
            serving.MenuId = menuId;
            _context.Servings.Add(serving);
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit", new { id = menuId });
        }

        // Post: Menus/RemoveOrder
        public async Task<IActionResult> RemoveOrder(int menuId, int recipeId)
        {
            var user = GetUser().Result;
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.RecipeId == recipeId && o.MenuId == menuId);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            var recipes = GetRecipes(menuId).Result;
            var menu = await _context.Menus.FirstOrDefaultAsync(m => m.MenuId == menuId);
            foreach (var recipe in recipes)
            {
                menu.Recipes.Remove(recipe);
                menu.Recipes.Add(StackRecipe(recipe, menu, user).Result);
            }
            return RedirectToAction("CurrentMenu");
        }

        // POST: /Menus/Edit/RemoveOrderAjax
        public async Task<IActionResult> RemoveOrderAjax(int menuId, int recipeId)
        {
            var user = GetUser().Result;
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.RecipeId == recipeId && o.MenuId == menuId);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            var recipe = await _context.Menus.FirstOrDefaultAsync(m => m.MenuId == menuId);
            return Json(recipe);
        }

        public async Task<IActionResult> RemoveRecipe(int menuId, int recipeId)
        {
            Serving serving = await _context.Servings.FirstOrDefaultAsync(s => s.MenuId == menuId && s.RecipeId == recipeId);
            _context.Servings.Remove(serving);
            await _context.SaveChangesAsync();
            var menu = _context.Menus.Where(m => m.MenuId == menuId);
            return RedirectToAction("Edit", new { id = menuId });
        }

        [Authorize(Roles = "Chef")]
        // POST: Menus/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Menu menu)
        {
            if (ModelState.IsValid)
            {
                _context.Update(menu);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(menu);
        }

        [Authorize(Roles = "Chef")]
        // GET: Menus/Delete/5
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Menu menu = await _context.Menus.SingleAsync(m => m.MenuId == id);
            if (menu == null)
            {
                return HttpNotFound();
            }

            return View(menu);
        }

        [Authorize(Roles = "Chef")]
        // POST: Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Menu menu = await _context.Menus.SingleAsync(m => m.MenuId == id);
            _context.Menus.Remove(menu);
            await _context.SaveChangesAsync();
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
        public async Task<ICollection<Order>> GetOrders(int recipeId, ApplicationUser user)
        {
            return await _context.Orders.Join(
                _userdb.Users.Where(
                u => u.Id == user.Id).ToList(),
                o => o.UserId,
                u => u.Id,
                (o, i) => o).ToListAsync();
        }

        // get served recipes for menu
        [NonAction]
        public async Task<ICollection<Recipe>> GetRecipes(int menuId)
        {
            return await _context.Recipes.Join(
                _context.Servings.Where(
                s => s.MenuId == menuId).ToList(),
                r => r.RecipeId,
                s => s.RecipeId,
                (o, i) => o).ToListAsync();
        }
    }
}