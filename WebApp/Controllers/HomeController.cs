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
        private readonly string apiBaseUrl = "https://localhost:44398/api/TodoItems/";
        
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            //apiBaseUrl = _Configure.GetValue<string>("WebAPIBaseUrl");
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> List() 
        {
            return await GetItems();
        }

        private async Task<IActionResult> GetItems()
        {
            using (HttpClient client = new HttpClient())
            {
                //string endpoint = apiBaseUrl + "/TodoItems/1";
                string endpoint = apiBaseUrl;

                using (var Response = await client.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = await Response.Content.ReadAsStringAsync();
                        var j = JsonConvert.DeserializeObject<IEnumerable<TodoItem>>(data);

                        return View(j);                        
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Username or Password is Incorrect");
                        return View();
                    }
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> List(TodoItem todoItem)
        {
            using (HttpClient client = new HttpClient())
            {
                //string endpoint = apiBaseUrl + "/TodoItems/1";
                string endpoint = apiBaseUrl;

                using (var Response = await client.PostAsync(endpoint, CreateHttpContent<TodoItem>(todoItem)))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return await GetItems();
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Username or Password is Incorrect");
                        return View();
                    }
                }
            }
        }

        private HttpContent CreateHttpContent<T>(T content)
        {
            var json = JsonConvert.SerializeObject(content, MicrosoftDateFormatSettings);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private static JsonSerializerSettings MicrosoftDateFormatSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
            }
        }

        public async Task<IActionResult> Item(int id)
        {
            return await GetItem(id);
        }

        private async Task<IActionResult> GetItem(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                //string endpoint = apiBaseUrl + "/TodoItems/1";
                string endpoint = apiBaseUrl + id;

                using (var Response = await client.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = await Response.Content.ReadAsStringAsync();
                        var j = JsonConvert.DeserializeObject<TodoItem>(data);

                        return View(j);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Username or Password is Incorrect");
                        return View();
                    }
                }
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            return await GetItem(id);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TodoItem todoItem)
        {
            using (HttpClient client = new HttpClient())
            {                
                string endpoint = apiBaseUrl + todoItem.Id;
                
                using (var Response = await client.PutAsync(endpoint, CreateHttpContent<TodoItem>(todoItem)))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Redirect("~/Home/List");
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Username or Password is Incorrect");
                        return View();
                    }
                }
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            return await GetItem(id);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //return await GetItem(id);

            using (HttpClient client = new HttpClient())
            {
                string endpoint = apiBaseUrl + id;

                //using (var Response = await client.PostAsync(endpoint, CreateHttpContent<TodoItem>(todoItem)))
                using (var Response = await client.DeleteAsync(endpoint))
                {
                    //if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    if (Response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Redirect("~/Home/List");
                        //return await GetItems();
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Username or Password is Incorrect");
                        return View();
                    }
                }
            }
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

