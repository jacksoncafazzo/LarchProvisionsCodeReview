using InstaSharp;
using LarchProvisionsWebsite.Models;
using LarchProvisionsWebsite.Services;
using Microsoft.AspNet.Http.Features;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.OptionsModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace LarchProvisionsWebsite.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(IOptions<InstagramApiOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public InstagramApiOptions Options { get; }
        private string _accessToken { get; set; }

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

        //[HttpGet]
        //public IActionResult AuthenticateInstagram(string code = null)
        //{
        //if (code == null)
        //{
        //    var urlString = "https://api.instagram.com/oauth/authorize/?client_id=" + Options.instClientId + "&redirect_uri=" + Options.instRedirectUri + "&response_type=code";
        //    return Redirect(urlString);
        //}
        //else
        //{
        //    try
        //    {
        //        NameValueCollection parameters = new NameValueCollection();

        //        parameters.Add("client_id", Options.instClientId);

        //        parameters.Add("client_secret", Options.instSecret);

        //        parameters.Add("grant_type", "authorization_code");

        //        parameters.Add("redirect_uri", Options.instRedirectUri);

        //        parameters.Add("code", code);

        //        WebClient client = new WebClient();
        //        var result = client.UploadValues("https://api.instagram.com/oauth/access_token", "POST", parameters);
        //        var response = System.Text.Encoding.Default.GetString(result);

        //        var jsResult = (JObject)JsonConvert.DeserializeObject(response);
        //        ViewData["AccessToken"] = (string)jsResult["access_token"];
        //        int id = (int)jsResult["user"]["id"];
        //        ViewData["InstId"] = id;
        //        ViewBag.jsResult = jsResult;
        //        return View(jsResult);
        //        }

        //                catch (private Exception ex)

        //                {
        //                    throw;
        //    }
        //}
        //

        public IActionResult Instagram()
        {
            return View();
        }

        public IActionResult GetInstDetails(string AccessToken, int InstId)
        {
            try
            {
                var realtimeUri = "";
                InstagramConfig config = new InstagramConfig(Options.instClientId, Options.instSecret, Options.instRedirectUri, realtimeUri);

                var client = new RestClient("https://api.instagram.com/v1");
                var request = new RestRequest { RootElement = "data", Resource = "/users/" + InstId };
                request.AddParameter("access_token", AccessToken);
                var posts = client.Execute<InstagramObject>(request);
                return View("Instagram", posts);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}