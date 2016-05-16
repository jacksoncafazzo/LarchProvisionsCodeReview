using Microsoft.AspNet.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace LarchProvisionsWebsite.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
           
                return View();
        
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Provisions()
        {
            return View();
        }
    }
}