using LarchProvisionsWebsite.Models;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace LarchProvisionsWebsite.Controllers
{
    public class OrdersController : Controller
    {
        private LarchKitchenDbContext _context;

        public OrdersController(LarchKitchenDbContext context)
        {
            _context = context;
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

        [HttpPost]
        public IActionResult UpdateOrder(int orderId, int orderSize, int custPrice, int menuId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (orderSize == 0)
            {
                _context.Orders.Remove(order);
            }
            var recipe = _context.Recipes.FirstOrDefault(r => r.RecipeId == order.RecipeId);
            order.CustPrice = recipe.CustPrice;
            order.OrderSize = orderSize;
            _context.Orders.Update(order);
            _context.SaveChanges();
            return Json(order);
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
    }
}