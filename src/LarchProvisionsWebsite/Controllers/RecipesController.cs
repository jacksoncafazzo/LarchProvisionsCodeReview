using LarchProvisionsWebsite.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LarchProvisionsWebsite.Controllers
{
    [Authorize(Roles = "Chef")]
    public class RecipesController : Controller
    {
        private LarchKitchenDbContext _context;

        public RecipesController(LarchKitchenDbContext context)
        {
            _context = context;
        }

        // GET: Recipes
        public IActionResult Index()
        {
            return View(_context.Recipes.Include(r => r.Ingredients));
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
            recipe.Ingredients = _context.Ingredients.Join(_context.Preps.Where(p => p.RecipeId == id).ToList(),
            p => p.IngredientId,
            i => i.IngredientId,
            (o, i) => o).ToList();
            recipe.Menus = _context.Menus.Join(_context.Servings.Where(s => s.RecipeId == id).ToList(),
            m => m.MenuId,
            s => s.MenuId,
            (o, i) => o).ToList();
            ViewBag.Preps = _context.Preps.Where(p => p.RecipeId == id);
            //ViewData["recipeImgUrl"] = Regex.Replace(recipe.Image, "~", "..");
            ViewData["recipeImgUrl"] = recipe.Image;

            return View(recipe);
        }

        // GET: Recipes/Create
        public IActionResult Create(string returnUrl = null)
        {
            if (returnUrl == null)
            {
                returnUrl = "/Recipes/";
            }
            ViewData["MenuId"] = returnUrl.Remove(0, returnUrl.Length - 2);
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        // POST: Recipes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Recipe recipe, string returnUrl = null, IFormFile file = null)
        {
            if (ModelState.IsValid)
            {
                if (file == null)
                {
                    recipe.Image = "~/img/white-larch-logo.png";
                }
                if (file != null)
                {
                    recipe = this.SavePhoto(recipe, file);
                }
                _context.Recipes.Add(recipe);
                _context.SaveChanges();
                if (returnUrl == null)
                    return RedirectToAction("Index");
                else if (Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
            }

            return View(recipe);
        }

        // GET: Recipes/Edit/5
        public IActionResult Edit(int? id, string returnUrl = null, int menuId = 1)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            if (returnUrl == null)
            {
                returnUrl = "/Recipes/";
            }
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["MenuId"] = menuId;
            Recipe recipe = _context.Recipes.FirstOrDefault(m => m.RecipeId == id);
            if (recipe == null)
            {
                return HttpNotFound();
            }
            recipe.Ingredients = _context.Ingredients.Join(_context.Preps.Where(p => p.RecipeId == recipe.RecipeId).ToList(),
                i => i.IngredientId,
                p => p.IngredientId,
                (o, i) => o).ToList();
            recipe.Menus = _context.Menus.Join(_context.Servings.Where(s => s.RecipeId == id).ToList(),
            m => m.MenuId,
            s => s.MenuId,
            (o, i) => o).ToList();
            ViewBag.Ingredients = _context.Ingredients.ToList().Except(recipe.Ingredients);
            recipe.Preps = _context.Preps.Where(p => p.RecipeId == id).ToList();
            ViewData["RecipeId"] = recipe.RecipeId;
            return View(recipe);
        }

        // POST: Recipes/Edit/5
        [HttpPost]
        public IActionResult Edit(Recipe recipe, string returnUrl = null, IFormFile file = null)
        {
            if (returnUrl == null)
            {
                returnUrl = "/Recipes/";
            }
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    recipe = this.SavePhoto(recipe, file);
                }
                _context.Update(recipe);
                _context.SaveChanges();
                return RedirectToAction("Edit", recipe);
            }
            return View(recipe);
        }

        [NonAction]
        public Recipe SavePhoto(Recipe recipe, IFormFile file)
        {
            recipe.Image = Path.Combine("img/recipes/", Regex.Replace(recipe.RecipeName, " \"*&@", "") + recipe.RecipeId + ".jpg");
            file.SaveAs(recipe.Image);
            recipe.Image = "~/" + recipe.Image;
            return recipe;
        }

        //Post: Recipes/PrepIngredient
        public IActionResult PrepIngredient(Recipe recipe, int ingredientId)
        {
            Prep prep = new Prep();
            prep.IngredientId = ingredientId;
            prep.RecipeId = recipe.RecipeId;
            _context.Preps.Add(prep);
            _context.SaveChanges();
            ViewBag.Ingredients = _context.Ingredients.ToList();

            return RedirectToAction("Edit", "Recipes", new { id = recipe.RecipeId });
        }

        //Post: Recipes/PrepIngredientAjax
        [HttpPost]
        public async Task<IActionResult> PrepIngredientAjax(int RecipeId, int IngredientId)
        {
            Prep prep = new Prep();
            prep.IngredientId = IngredientId;
            prep.RecipeId = RecipeId;
            _context.Preps.Add(prep);
            int x = await _context.SaveChangesAsync();
            if (x == 1)
            {
                Recipe recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.RecipeId == RecipeId);
                return Json(recipe);
            }
            return null;
        }

        //Post: Recipes/RemoveIngredientAjax
        [HttpPost]
        public async Task<IActionResult> RemoveIngredientAjax(int RecipeId, int IngredientId)
        {
            var prep = await _context.Preps.SingleOrDefaultAsync(p => p.RecipeId == RecipeId && p.IngredientId == IngredientId);
            prep.IngredientId = IngredientId;
            prep.RecipeId = RecipeId;
            _context.Preps.Remove(prep);
            await _context.SaveChangesAsync();
            Recipe recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.RecipeId == RecipeId);
            recipe.Ingredients = await _context.Ingredients.Join(_context.Preps.Where(
                p => p.RecipeId == RecipeId).ToList(),
                r => r.IngredientId,
                p => p.IngredientId,
               (o, i) => o).ToListAsync();
            return Json(recipe);
        }

        [HttpGet]
        public async Task<IActionResult> IngredientsDisplay(int recipeId)
        {
            Recipe recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.RecipeId == recipeId);
            recipe.Ingredients = await _context.Ingredients.Join(_context.Preps.Where(p => p.RecipeId == recipe.RecipeId).ToList(),
                p => p.IngredientId,
                i => i.IngredientId,
                (o, i) => o
                ).ToListAsync();
            var ingredients = _context.Ingredients.ToList().Except(recipe.Ingredients);
            recipe.Ingredients = ingredients.ToList();
            return Json(recipe);
        }

        [HttpGet]
        public async Task<IActionResult> RemoveIngredientsDisplay(int RecipeId)
        {
            Recipe recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.RecipeId == RecipeId);
            recipe.Ingredients = await _context.Ingredients.Join(_context.Preps.Where(p => p.RecipeId == RecipeId).ToList(),
                p => p.IngredientId,
                i => i.IngredientId,
                (o, i) => o
                ).ToListAsync();
            return Json(recipe);
        }

        public IActionResult RemoveIngredient(Recipe recipe, int ingredientId)
        {
            Prep prep = _context.Preps.FirstOrDefault(p => p.IngredientId == ingredientId && p.RecipeId == recipe.RecipeId);
            _context.Preps.Remove(prep);

            _context.SaveChanges();
            ViewBag.Ingredients = _context.Ingredients.ToList();

            return RedirectToAction("Edit", "Recipes", new { id = recipe.RecipeId });
        }

        // POST: Recipes/Edit/CreateIngredient

        [HttpPost, ActionName("CreateIngredient")]
        [ValidateAntiForgeryToken]
        public IActionResult CreateIngredient(int recipeId, double amount, string unit, string name, string source)
        {
            Ingredient newIngredient = new Ingredient();
            newIngredient.Amount = amount;
            newIngredient.Unit = unit;
            newIngredient.IngredientName = name;
            newIngredient.Source = source;
            _context.Ingredients.Add(newIngredient);
            _context.SaveChanges();
            var newPrep = new Prep();
            newPrep.RecipeId = recipeId;
            newPrep.IngredientId = newIngredient.IngredientId;
            _context.Preps.Add(newPrep);
            _context.SaveChanges();
            return Json(newIngredient);
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

        [HttpPost, ActionName("DeleteRecipeAjax")]
        public void DeleteRecipeAjax(int recipeId)
        {
            Recipe recipe = _context.Recipes.Single(r => r.RecipeId == recipeId);
            _context.Recipes.Remove(recipe);
            _context.SaveChanges();
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