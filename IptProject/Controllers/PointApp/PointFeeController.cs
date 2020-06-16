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
    public class PointFeeController : Controller
    {
        string Baseurl = "http://localhost:64989/";
        public async Task<ActionResult> Index()
        {
            List<PointFee> FeeInfo = new List<PointFee>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/PointApp/PointFee/Get");
                if (Res.IsSuccessStatusCode)
                {
                    var DriResponse = Res.Content.ReadAsStringAsync().Result;
                    FeeInfo = JsonConvert.DeserializeObject<List<PointFee>>(DriResponse);
                }
                return View(FeeInfo);
            }
        }

        [HttpGet]
        public ActionResult AddFee()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddFee([Bind(Include = "FeeID,TransportFee,DueDate,FineCharges,SemesterID")] PointFee fee)
        {

            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);

                    var response = await client.PostAsJsonAsync("api/PointApp/PointFee/Post", fee);
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
            return View(fee);
        }

        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PointFee fee = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                var result = await client.GetAsync($"api/PointApp/PointFee?FeeID={id}");

                if (result.IsSuccessStatusCode)
                {
                    fee = await result.Content.ReadAsAsync<PointFee>();
                    //return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }

            if (fee == null)
            {
                return HttpNotFound();
            }
            return View(fee);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                var response = await client.DeleteAsync($"api/PointApp/PointFee?FeeID={id}");
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
            PointFee fee = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                var result = await client.GetAsync($"api/PointApp/PointFee?FeeID={id}");

                if (result.IsSuccessStatusCode)
                {
                    fee = await result.Content.ReadAsAsync<PointFee>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            if (fee == null)
            {
                return HttpNotFound();
            }
            return View(fee);
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditConfirm(string id, [Bind(Include = "FeeID,TransportFee,DueDate,FineCharges,SemesterID")] PointFee fee)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    var response = await client.PutAsJsonAsync($"api/PointApp/PointFee?FeeID={id}", fee);
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
            return View(fee);
        }
    }
}