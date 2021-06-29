using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Dashboards_Web.Controllers
{
    public class JobSchedulerController : Controller
    {
        public ActionResult JobScheduler()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetDetails(string url,string key,string val)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url+"/api/"+key+"/"+val);
                var responseTask = client.GetAsync(client.BaseAddress);
                responseTask.Wait();

                var result = responseTask.Result;
            }
            return Json(new { JsonRequestBehavior.AllowGet });
        }
        
    }
}