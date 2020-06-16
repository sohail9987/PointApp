using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PointDataAccess;

namespace IptProject.PointManagementSystem.Controllers
{
    public class DriverController : Controller
    {
        string Baseurl = "http://localhost:64989/";
        public async Task<ActionResult> Index()
        {
            List<Driver> DriInfo = new List<Driver>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/PointApp/Driver/Get");
                if (Res.IsSuccessStatusCode)
                {
                    var DriResponse = Res.Content.ReadAsStringAsync().Result;
                    DriInfo = JsonConvert.DeserializeObject<List<Driver>>(DriResponse);
                }
                return View(DriInfo);
            }
        }

        [HttpGet]
        public ActionResult AddDriver()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddDriver([Bind(Include = "CNIC,DriverName,ContactNumber")] Driver driver)
        {

            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);

                    var response = await client.PostAsJsonAsync("api/PointApp/Driver/Post", driver);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server error try after some time.");
                    }
                }
            }
            return View(driver);
        }

        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Driver driver = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                var result = await client.GetAsync($"api/PointApp/Driver?CNIC={id}");

                if (result.IsSuccessStatusCode)
                {
                    driver = await result.Content.ReadAsAsync<Driver>();
                    //return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }

            if (driver == null)
            {
                return HttpNotFound();
            }
            return View(driver);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                var response = await client.DeleteAsync($"api/PointApp/Driver?CNIC={id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
            }
            return View();
        }

        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Driver driver = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                var result = await client.GetAsync($"api/PointApp/Driver?CNIC={id}");

                if (result.IsSuccessStatusCode)
                {
                    driver = await result.Content.ReadAsAsync<Driver>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            if (driver == null)
            {
                return HttpNotFound();
            }
            return View(driver);
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditConfirm(string id, [Bind(Include = "CNIC,DriverName,ContactNumber")] Driver driver)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    var response = await client.PutAsJsonAsync($"api/PointApp/Driver?CNIC={id}", driver);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server error try after some time.");
                    }
                }
                return RedirectToAction("Index");
            }
            return View(driver);
        }
    }
}