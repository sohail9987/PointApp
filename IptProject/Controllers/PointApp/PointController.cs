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
    public class PointController : Controller
    {
        string Baseurl = "http://localhost:64989/";
        public async Task<ActionResult> Index()
        {
            List<Point> PointInfo = new List<Point>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/PointApp/Point/Get");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var PointResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    PointInfo = JsonConvert.DeserializeObject<List<Point>>(PointResponse);

                }
                //returning the employee list to view  
                return View(PointInfo);
            }
        }

        [HttpGet]
        public ActionResult AddPoint()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddPoint([Bind(Include = "NumberPlate,NumberOfSeats,CNIC")] Point point)
        {

            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);

                    var response = await client.PostAsJsonAsync("api/PointApp/Point/Post", point);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Point Can't Found");
                    }
                }
            }
            return View(point);
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (id < 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Point point = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                var result = await client.GetAsync($"api/PointApp/Point?PointID={id}");

                if (result.IsSuccessStatusCode)
                {
                    point = await result.Content.ReadAsAsync<Point>();
                    //return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Point Can't Found");
                }
            }

            if (point == null)
            {
                return HttpNotFound();
            }
            return View(point);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                var response = await client.DeleteAsync($"api/PointApp/Point?PointID={id}");
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
            Point point = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                var result = await client.GetAsync($"api/PointApp/Point?PointID={id}");

                if (result.IsSuccessStatusCode)
                {
                    point = await result.Content.ReadAsAsync<Point>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            if (point == null)
            {
                return HttpNotFound();
            }
            return View(point);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditConfirm(int id, [Bind(Include = "NumberPlate,NumberOfSeats,CNIC")] Point point)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    var response = await client.PutAsJsonAsync($"api/PointApp/Point?PointID={id}", point);
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
            return View(point);
        }
    }
}