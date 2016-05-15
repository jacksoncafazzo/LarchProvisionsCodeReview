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

        public IActionResult PrivacyPolicy()
        {
            return View();
        }

        public IActionResult OAuth()
        {
            return View();
        }

        public IActionResult Instagram()
        {
            return View();
        }

        //public IActionResult InstaLogin()
        //{
        //    href = "https://api.instagram.com/oauth/authorize/?client_id=CLIENT-ID&redirect_uri=REDIRECT-URI&response_type=code"
        //        return View("Instagram");
        //}
    }
}