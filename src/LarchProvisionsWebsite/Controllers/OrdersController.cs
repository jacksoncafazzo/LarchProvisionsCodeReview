using LarchProvisionsWebsite.Models;
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
    public class OrdersController : Controller
    {
        private LarchKitchenDbContext _context;
        private ApplicationDbContext _usercontext;

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
    }
}