using LarchProvisionsWebsite.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using System.Linq;

namespace LarchProvisionsWebsite.Controllers
{
    [Authorize(Roles = "Chef")]
    public class PrepsController : Controller
    {
        private ApplicationDbContext _context;

        public PrepsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Preps
        public IActionResult Index()
        {
            return View(_context.Prep.ToList());
        }

        // GET: Preps/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Prep prep = _context.Prep.Single(m => m.PrepId == id);
            if (prep == null)
            {
                return HttpNotFound();
            }

            return View(prep);
        }

        // GET: Preps/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Preps/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Prep prep)
        {
            if (ModelState.IsValid)
            {
                _context.Prep.Add(prep);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(prep);
        }

        // GET: Preps/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Prep prep = _context.Prep.Single(m => m.PrepId == id);
            if (prep == null)
            {
                return HttpNotFound();
            }
            return View(prep);
        }

        // POST: Preps/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Prep prep)
        {
            if (ModelState.IsValid)
            {
                _context.Update(prep);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(prep);
        }

        // GET: Preps/Delete/5
        [ActionName("Delete")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Prep prep = _context.Prep.Single(m => m.PrepId == id);
            if (prep == null)
            {
                return HttpNotFound();
            }

            return View(prep);
        }

        // POST: Preps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Prep prep = _context.Prep.Single(m => m.PrepId == id);
            _context.Prep.Remove(prep);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}