using LarchProvisionsWebsite.Models;
using Microsoft.AspNet.Http.Internal;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LarchProvisionsWebsite.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Roles.ToList());
        }

        //GET: /Admin/CreateRole
        public IActionResult CreateRole()
        {
            return View();
        }

        //POST: /Admin/CreateRole
        [HttpPost]
        public IActionResult CreateRole(FormCollection collection)
        {
            try
            {
                _context.Roles.Add(new IdentityRole()
                {
                    Name = Request.Form["RoleName"]
                });
                _context.SaveChanges();
                ViewBag.ResultMessage = "Role created like a boss!";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Delete(string RoleName)
        {
            if (RoleName != null)
            {
                var thisRole = _context.Roles.Where(r => r.Name.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                _context.Roles.Remove(thisRole);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}