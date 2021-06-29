using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dashboards_Web.CustomAttribute
{
    public class CustomExceptionAttribute: HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;
            filterContext.ExceptionHandled = true;

            string ControllerName = (string)filterContext.RouteData.Values["controller"];
            string ActionName = (string)filterContext.RouteData.Values["action"];
            var model = new HandleErrorInfo(filterContext.Exception, ControllerName, ActionName);

            filterContext.Result = new ViewResult()
            {
                ViewName = "~/Views/Shared/CustomError.cshtml",

                ViewData = new ViewDataDictionary(model)
            };
        }
    }
}