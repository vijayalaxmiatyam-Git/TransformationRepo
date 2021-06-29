using Dashboards_BusinessLayer.Interfaces;
using Dashboards_Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

namespace Dashboards_Web.Controllers
{
    public class OnBoardingVSOController : Controller
    {
        readonly ISfbHandler SfbDataHandler;
        public OnBoardingVSOController(ISfbHandler SfbDataHandler)
        {
            this.SfbDataHandler = SfbDataHandler;
        }

        [HttpPost]
        public ActionResult GetVSOData(string param)
        {
            List<OnBoardVSOModel> model = new List<OnBoardVSOModel>();
            var tasks = SfbDataHandler.GetOnboardVSOTasks(param);
            foreach (DataRow item in tasks.Rows)
            {
                OnBoardVSOModel modelData = new OnBoardVSOModel();
                modelData.TaskID = item["TaskID"].ToString();
                modelData.Title = item["Title"].ToString();
                modelData.Track = param;//item["Track"].ToString();
                modelData.CreatedOn = item["CreatedOn"].ToString();
                modelData.Owner = item["Owner"].ToString();
                model.Add(modelData);
            }
            Session["data"] = model;
            return Json(new { JsonRequestBehavior.AllowGet });
            //return View(model);
        }

        public ActionResult OnBoardingVSO()
        {
            List<OnBoardVSOModel> model = new List<OnBoardVSOModel>();
            model = (List<OnBoardVSOModel>)Session["data"];
            return View(model);
        }
        [HttpPost]
        public ActionResult GetServiceVSOData(string param)
        {
            List<OnBoardVSOModel> model = new List<OnBoardVSOModel>();
            var tasks = SfbDataHandler.GetOnboardVSOServices("SD");
            foreach (DataRow item in tasks.Rows)
            {
                OnBoardVSOModel modelData = new OnBoardVSOModel();
                modelData.TaskID = item["TaskId"].ToString();
                modelData.Title = item["Title"].ToString();
                modelData.Track = param; //item["Track"].ToString();
                modelData.CreatedOn = item["CreatedOn"].ToString();
                modelData.Owner = item["Owner"].ToString();
                model.Add(modelData);
            }
            Session["servicedata"] = model;
            return Json(new { JsonRequestBehavior.AllowGet });
        }
        public ActionResult OnBoardingService()
        {
            List<OnBoardVSOModel> model = new List<OnBoardVSOModel>();
            model = (List<OnBoardVSOModel>)Session["servicedata"];
            return View(model);
        }
        [HttpPost]
        public ActionResult InsertServiceName(string param)
        {
            try
            {
                SfbDataHandler.InsertUpdateTrackTypeName(2, param);
                return Json(new { JsonRequestBehavior.AllowGet });
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult InsertTrackName(string param)
        {
            try
            {
                SfbDataHandler.InsertUpdateTrackTypeName(1, param);
                return Json(new { JsonRequestBehavior.AllowGet });
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult InsertEnggTaskData(string taskName, string subTask)
        {
            try
            {
                subTask = subTask.Replace("\n", "<br/>");
                SfbDataHandler.InsertOnboardTaskData("SD",taskName, subTask);
                return Json(new { JsonRequestBehavior.AllowGet });
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult InsertServiceTaskData(string taskName, string subTask)
        {
            try
            {
                subTask = subTask.Replace("\n", "<br/>");
                SfbDataHandler.InsertOnboardServiceData("SD", taskName, subTask);
                return Json(new { JsonRequestBehavior.AllowGet });
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}