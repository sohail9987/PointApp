using System;
using System.Collections.Generic;
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
    public class SemesterController : Controller
    {
        string Baseurl = "http://localhost:64989/";
        public async Task<ActionResult> Index()
        {
            List<Semester> SemInfo = new List<Semester>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/PointApp/Semester/Get");
                if (Res.IsSuccessStatusCode)
                {
                    var SemResponse = Res.Content.ReadAsStringAsync().Result;
                    SemInfo = JsonConvert.DeserializeObject<List<Semester>>(SemResponse);
                }
                return View(SemInfo);
            }
        }
    }
}