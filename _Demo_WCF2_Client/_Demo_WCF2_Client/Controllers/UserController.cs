using _Demo_WCF2_Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace _Demo_WCF2_Client.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient Client = new HttpClient();
        private string CustomerApiUrl = "http://localhost:52041/UserService.svc/User";
        private MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");


        // GET: User
        public async Task<ActionResult> Index()
        {
            Client.DefaultRequestHeaders.Accept.Add(contentType);
            HttpResponseMessage response = await Client.GetAsync(CustomerApiUrl);
            string strData = await response.Content.ReadAsStringAsync();

            List<User> users = JsonConvert.DeserializeObject<List<User>>(strData);
            return View(users);
        }

        // GET: CustomerController/Details/5
        public async Task<ActionResult> Details(int Id)
        {
            User customer = await GetUserById(Id);
            return View(customer);
        }

        // GET: CustomerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string Name)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Name = Name
                };

                //Send Create request
                HttpRequestMessage request = new HttpRequestMessage();
                request.Headers.Add("Accept", "application/json");
                request.RequestUri = new Uri(CustomerApiUrl + "/Add");
                request.Content = new StringContent(
                    Newtonsoft.Json.JsonConvert.SerializeObject(user),
                    Encoding.UTF8,
                    "application/json");
                request.Method = HttpMethod.Post;
                HttpResponseMessage apiResponse = await Client.SendAsync(request);

                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: CustomerController/Edit/5
        public async Task<ActionResult> Edit(int Id)
        {
            User user = await GetUserById(Id);
            return View(user);
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int Id, string Name)
        {
            var user = new User
            {
                Id = Id,
                Name = Name
            };

            if (ModelState.IsValid)
            {


                //Send update request
                HttpRequestMessage request = new HttpRequestMessage();
                request.Headers.Add("Accept", "application/json");
                request.RequestUri = new Uri(CustomerApiUrl + "/Edit");
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(user),
                    Encoding.UTF8,
                    "application/json");
                request.Method = HttpMethod.Put;
                HttpResponseMessage apiResponse = await Client.SendAsync(request);
                return RedirectToAction("Index");
            }
            return View(user);
        }


        public async Task<ActionResult> Delete(int Id)
        {


            try
            {
                User user = await GetUserById(Id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteConfirm(int Id)
        {
            try
            {
                if (Id == 0)
                {
                    return HttpNotFound();
                }


                HttpRequestMessage request = new HttpRequestMessage();
                request.Headers.Add("Accept", "application/json");
                request.RequestUri = new Uri(CustomerApiUrl + $"/Delete/{Id}");
                request.Method = HttpMethod.Delete;
                HttpResponseMessage apiResponse = await Client.SendAsync(request);
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {

                return View("Error", ex);
            }
        }

        private async Task<User> GetUserById(int Id)
        {
            User user = null;
            HttpResponseMessage response = await Client.GetAsync(CustomerApiUrl + $"/{Id}");
            string strData = await response.Content.ReadAsStringAsync();

            user = JsonConvert.DeserializeObject<User>(strData);

            return user;
        }

    }
}