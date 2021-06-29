using Dashboards_Web.CustomAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dashboards_Web.Controllers
{
    [CustomExceptionAttribute]
    public class HomeController : Controller
    {
        public ActionResult ChatBot()
        {
            //int a = 1;
            //int b = 0;
            //int c = 0;

            //c = a / b; //it would cause exception. 
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}