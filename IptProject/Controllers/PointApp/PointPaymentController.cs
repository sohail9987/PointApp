﻿using System;
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
    public class PointPaymentController : Controller
    {
        string Baseurl = "http://localhost:64989/";
        public async Task<ActionResult> Index()
        {
            List<PointPayment> PointInfo = new List<PointPayment>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/PointApp/PointPayment/Get");
                if (Res.IsSuccessStatusCode)
                {
                    var PointResponse = Res.Content.ReadAsStringAsync().Result;
                    PointInfo = JsonConvert.DeserializeObject<List<PointPayment>>(PointResponse);
                }
                return View(PointInfo);
            }
        }

        [HttpGet]
        public ActionResult AddStudentPayment()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddStudentPayment([Bind(Include = "DepositDate,NumberofDaysLate,TotalPayable,PaymentID,StudentID,FeeID")] PointPayment pp)
        {

            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);

                    var response = await client.PostAsJsonAsync("api/PointApp/StudentPoint/Post", pp);
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
            return View(pp);
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (id < 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PointPayment pp = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                var result = await client.GetAsync($"api/PointApp/PointPayment?PaymentID={id}");

                if (result.IsSuccessStatusCode)
                {
                    pp = await result.Content.ReadAsAsync<PointPayment>();
                    //return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }

            if (pp == null)
            {
                return HttpNotFound();
            }
            return View(pp);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                var response = await client.DeleteAsync($"api/PointApp/PointPayment?PaymentID={id}");
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
            PointPayment pp = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                var result = await client.GetAsync($"api/PointApp/PointPayment?PaymentID={id}");

                if (result.IsSuccessStatusCode)
                {
                    pp = await result.Content.ReadAsAsync<PointPayment>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            if (pp == null)
            {
                return HttpNotFound();
            }
            return View(pp);
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditConfirm(int id, [Bind(Include = "DepositDate,NumberofDaysLate,TotalPayable,PaymentID,StudentID,FeeID")] PointPayment pp)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    var response = await client.PutAsJsonAsync($"api/PointApp/PointPayment?PaymentID={id}", pp);
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
            return View(pp);
        }
    }
}