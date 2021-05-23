using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SBAccountClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SBAccountClient.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            string BaseUrl = "http://localhost:28299/";
            var AccountInfo = new List<Account>();
            //HttpClient cl = new HttpClient();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/Accounts");
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var AccountResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    AccountInfo = JsonConvert.DeserializeObject<List<Account>>(AccountResponse);

                }
                //returning the employee list to view  
                return View(AccountInfo);
            }
        }
        [HttpGet]
        // GET: AccountController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            Account A = new Account();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:28299/api/Accounts/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    A = JsonConvert.DeserializeObject<Account>(apiResponse);
                }
            }
            return View(A);
        }
        [HttpGet]
        // GET: AccountController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Account B)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(B), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("http://localhost:28299/api/Accounts", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<Account>(apiResponse);
                }
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        // GET: AccountController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Account A = new Account();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:28299/api/Accounts/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    A = JsonConvert.DeserializeObject<Account>(apiResponse);
                }
            }
            return View(A);
        }


        // POST: AccountController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Account B)
        {
            int Bid = Convert.ToInt32(TempData["BlogId"]);
            Account rec = new Account();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(B), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync("http://localhost:28299/api/Accounts/" + Bid, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    rec = JsonConvert.DeserializeObject<Account>(apiResponse);
                }
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        // GET: AccountController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            TempData["BlogId"] = id;
            Account A = new Account();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:28299/api/Accounts/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    A = JsonConvert.DeserializeObject<Account>(apiResponse);
                }
            }
            return View(A);
        }

        // POST: AccountController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Account A)
        {
            int Bid = Convert.ToInt32(TempData["BlogId"]);
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("http://localhost:28299/api/Accounts/" + Bid))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("Index");
        }
    }
}
