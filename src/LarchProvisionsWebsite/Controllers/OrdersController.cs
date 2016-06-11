using LarchProvisionsWebsite.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace LarchProvisionsWebsite.Controllers
{
    [Authorize(Roles = "Chef")]
    public class OrdersController : Controller
    {
        private LarchKitchenDbContext _context;
        private ApplicationDbContext _usercontext;
        private UserManager<ApplicationUser> _userMan;

        public OrdersController(LarchKitchenDbContext context, ApplicationDbContext usercontext)
        {
            _context = context;
            _usercontext = usercontext;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(_context.Orders.ToList());
        }

        public IActionResult Edit(int? id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == id);
            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult Delete(int orderId)
        {
            Order order = _context.Orders.Single(m => m.OrderId == orderId);
            _context.Orders.Remove(order);
            _context.SaveChanges();
            return Json(order);
        }

        // Get User
        [NonAction]
        public async Task<ApplicationUser> GetUser()
        {
            return await _usercontext.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == User.GetUserId());
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrder(int orderId, int custPrice, int menuId, int orderSize)
        {
            var user = await GetUser();
            var order = await _context.Orders.SingleOrDefaultAsync(o => o.OrderId == orderId);
            if (orderSize != 0)
            {
                var recipe = await _context.Recipes.Include(r => r.Orders).SingleOrDefaultAsync(r => r.RecipeId == order.RecipeId);
                order.CustPrice = recipe.CustPrice;
                order.OrderSize = orderSize;
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
                var orders = await _context.Orders.Where(o => o.MenuId == menuId && o.UserId == user.Id).ToListAsync();
                return Json(new { recipe = recipe, order = order, orders = orders });
            }
            else
            {
                _context.Orders.Remove(order);
                return Json(null);
            }
        }

        //[NonAction]
        //public int CustTotal()
        //{
        //    var allOrders = await _context.Orders.Where(o => o.UserId == user.Id && o.MenuId == menuId).ToListAsync();
        //    var custTotal = 0;
        //    foreach (var anotherOrder in allOrders)
        //    {
        //        custTotal = custTotal + (anotherOrder.OrderSize * anotherOrder.CustPrice);
        //    }
        //    return custTotal;
        //}
    }
}