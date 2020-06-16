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
    public class StudentPointController : Controller
    {
        string Baseurl = "http://localhost:64989/";
        public async Task<ActionResult> Index()
        {
            List<StudentPoint> SInfo = new List<StudentPoint>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/PointApp/StudentPoint/Get");
                if (Res.IsSuccessStatusCode)
                {
                    var SResponse = Res.Content.ReadAsStringAsync().Result;
                    SInfo = JsonConvert.DeserializeObject<List<StudentPoint>>(SResponse);
                }
                return View(SInfo);
            }
        }

        [HttpGet]
        public ActionResult AddStudentPoint()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddStudentPoint([Bind(Include = "PickUpAddress,PointID,StudentID,SemesterID,RegistrationDate")] StudentPoint sp)
        {

            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);

                    var response = await client.PostAsJsonAsync("api/PointApp/StudentPoint/Post", sp);
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
            return View(sp);
        }


        public async Task<ActionResult> Delete(int id)
        {
            if (id < 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentPoint sp = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                var result = await client.GetAsync($"api/PointApp/StudentPoint?SPID={id}");

                if (result.IsSuccessStatusCode)
                {
                    sp = await result.Content.ReadAsAsync<StudentPoint>();
                    //return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }

            if (sp == null)
            {
                return HttpNotFound();
            }
            return View(sp);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                var response = await client.DeleteAsync($"api/PointApp/StudentPoint?SPID={id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
            }
            return View();
        }
        /*
        public async Task<ActionResult> Print(int id)
        {
            if (id < 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentPoint sp = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                var result = await client.GetAsync($"api/PointApp/StudentPoint?SPID={id}");

                if (result.IsSuccessStatusCode)
                {
                    sp = await result.Content.ReadAsAsync<StudentPoint>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            if (sp == null)
            {
                return HttpNotFound();
            }
            return View(sp);
        }
        */
        public async Task<ActionResult> Edit(int id)
        {
            if (id < 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentPoint sp = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                var result = await client.GetAsync($"api/PointApp/StudentPoint?SPID={id}");

                if (result.IsSuccessStatusCode)
                {
                    sp = await result.Content.ReadAsAsync<StudentPoint>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            if (sp == null)
            {
                return HttpNotFound();
            }
            return View(sp);
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditConfirm(int id, [Bind(Include = "PickUpAddress,PointID,StudentID,SemesterID,RegistrationDate")] StudentPoint sp)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    var response = await client.PutAsJsonAsync($"api/PointApp/StudentPoint?SPID={id}", sp);
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
            return View(sp);
        }
    }
}