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
    public class RouteController : Controller
    {
        string Baseurl = "http://localhost:64989/";
        public async Task<ActionResult> Index()
        {
            List<Route> RtInfo = new List<Route>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/PointApp/Route/Get");
                if (Res.IsSuccessStatusCode)
                {
                    var RtResponse = Res.Content.ReadAsStringAsync().Result; 
                    RtInfo = JsonConvert.DeserializeObject<List<Route>>(RtResponse);
                }
                return View(RtInfo);
            }
        }

        [HttpGet]
        public ActionResult AddRoute()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddRoute([Bind(Include = "PointID,RouteStop,RouteTime")] Route route)
        {

            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);

                    var response = await client.PostAsJsonAsync("api/PointApp/Route/Post", route);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Create Point First");
                    }
                }
            }
            return View(route);
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (id < 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Route route = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                var result = await client.GetAsync($"api/PointApp/Route?RouteID={id}");

                if (result.IsSuccessStatusCode)
                {
                    route = await result.Content.ReadAsAsync<Route>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }

            if (route == null)
            {
                return HttpNotFound();
            }
            return View(route);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                var response = await client.DeleteAsync($"api/PointApp/Route?RouteID={id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
            }
            return View();
        }

        public async Task<ActionResult> Edit(int id)
        {
            if (id < 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Route route = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                var result = await client.GetAsync($"api/PointApp/Route?RouteID={id}");

                if (result.IsSuccessStatusCode)
                {
                    route = await result.Content.ReadAsAsync<Route>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            if (route == null)
            {
                return HttpNotFound();
            }
            return View(route);
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditConfirm(string id, [Bind(Include = "PointID,RouteStop,RouteTime")] Route route)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    var response = await client.PutAsJsonAsync($"api/PointApp/Route?RouteID={id}", route);
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
            return View(route);
        }
    }
}