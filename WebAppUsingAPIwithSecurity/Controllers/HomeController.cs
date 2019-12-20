using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAppUsingAPIwithSecurity.Models;

namespace WebAppUsingAPIwithSecurity.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            //get the token first
            string tokenUrl = "https://localhost:44391/api/values/token";
            string token = string.Empty;
            using (HttpResponseMessage response = await APIHelper.ApiClient.PostAsJsonAsync(tokenUrl, token))
            {
                if (response.IsSuccessStatusCode)
                {
                    token = await response.Content.ReadAsAsync<string>();
                }
            }

            //next part get values
            string url = "https://localhost:44391/api/values";
            List<string> values = new List<string>();

            //gotta add the token to the header of the request
            APIHelper.ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //then get the data
            using (HttpResponseMessage response = await APIHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    values = await response.Content.ReadAsAsync<List<string>>();
                }
            }


            return View(values);
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
