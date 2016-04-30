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
    }
}