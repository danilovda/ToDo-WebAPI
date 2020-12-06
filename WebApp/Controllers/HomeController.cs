using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApp.Models;

//https://www.c-sharpcorner.com/article/consuming-web-apis-in-asp-net-core-mvc-application/
namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _Configure;
        private readonly string apiBaseUrl;

        //public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            //apiBaseUrl = _Configure.GetValue<string>("WebAPIBaseUrl");
        }

        public async Task<IActionResult> Index()
        {
            using (HttpClient client = new HttpClient())
            {
                //StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                //StringContent content = "";//new StringContent("", Encoding.UTF8, "application/json");
                //string endpoint = apiBaseUrl + "/TodoItems/1";
                string endpoint = "https://localhost:44398/api/TodoItems/1";

                
                //using (var Response = await client.PostAsync(endpoint, content))
                using (var Response = await client.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        //TempData["Profile"] = JsonConvert.SerializeObject(user);
                        
                        var data = await Response.Content.ReadAsStringAsync();
                        var j = JsonConvert.DeserializeObject<TodoItem>(data);
                        return RedirectToAction("Profile");

                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Username or Password is Incorrect");
                        return View();

                    }

                }
            }
                
            //return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
