using LarchProvisionsWebsite.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http.Internal;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LarchProvisionsWebsite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
        [ValidateAntiForgeryToken]
        public IActionResult CreateRole(FormCollection collection)
        {
            _context.Roles.Add(new IdentityRole(Request.Form["RoleName"]));
            _context.SaveChanges();
            ViewBag.ResultMessage = "Role created like a boss!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(string RoleName)
        {
            if (RoleName != null)
            {
                var role = _context.Roles.FirstOrDefault(r => r.Name == RoleName);
                if (role != null)
                {
                    try
                    {
                        _context.Roles.Remove(role);
                        _context.SaveChanges();
                    }
                    catch
                    {
                        ViewData["Message"] = "Users still in role " + RoleName;
                        return Index();
                    }
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Assign()
        {
            ViewBag.Users = new SelectList(_userManager.Users.ToList());
            return View();
        }

        [HttpPost]
        public IActionResult GetRoles(string user)
        {
            var thisUser = GetUser(user);
            ViewBag.User = thisUser;
            ViewBag.RolesForThisUser = _userManager.GetRolesAsync(thisUser).Result;
            var allRoles = _context.Roles.ToList();
            var roles = new List<string>();
            foreach (var role in allRoles)
            {
                var add = true;
                foreach (var userRole in ViewBag.RolesForThisUser)
                {
                    if (userRole == role.Name)
                    {
                        add = false;
                    }
                }
                if (add)
                {
                    roles.Add(role.Name);
                }
            }
            ViewBag.Users = new SelectList(_userManager.Users.ToList());
            ViewBag.Roles = roles;
            return View("Assign");
        }

        public IActionResult AddToUser(string username, string roleName)
        {
            var user = GetUser(username);
            IdentityResult Response = _userManager.AddToRoleAsync(user, roleName).Result;
            return GetRoles(username);
        }

        [HttpPost]
        public IActionResult DeleteFromUser(string username, string roleName)
        {
            IdentityResult Response = _userManager.RemoveFromRoleAsync(GetUser(username), roleName).Result;
            return GetRoles(username);
        }

        public ApplicationUser GetUser(string username)
        {
            return _userManager.Users.FirstOrDefault(m => m.UserName == username);
        }

        public IActionResult Edit(string roleName)
        {
            ViewData["roleName"] = roleName;
            return View("Edit");
        }

        [HttpPost]
        public IActionResult Edit()
        {
            var role = _context.Roles.FirstOrDefault(m => m.Name == Request.Form["role-name"]);
            role.Name = Request.Form["edit-role"];
            _context.Roles.Update(role);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}