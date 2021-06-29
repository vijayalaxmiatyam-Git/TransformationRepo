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
    public class OnBoardingController : Controller
    {
        readonly ISfbHandler SfbDataHandler;
        public OnBoardingController(ISfbHandler SfbDataHandler)
        {
            this.SfbDataHandler = SfbDataHandler;
        }
        public ActionResult OnBoarding()
        {
            List<OnBoardModel> model = new List<OnBoardModel>();
            var tasks = SfbDataHandler.GetOnboardTasks();
            foreach (DataRow item in tasks.Rows)
            {
                OnBoardModel modelData = new OnBoardModel();
                modelData.TaskName = item["TaskName"].ToString();
                modelData.SubTasks = item["SubTasks"].ToString();
                modelData.IncludeTask = false;
                
                model.Add(modelData);
            }
            var trackData = SfbDataHandler.GetTrackNamesByType(1);
            List<SelectListItem> dataList = new List<SelectListItem>();
            foreach (DataRow item in trackData.Rows)
            {
                SelectListItem data = new SelectListItem();
                data.Text = item["Name"].ToString();
                data.Value = item["Name"].ToString();
                dataList.Add(data);
            }
            ViewBag.TrackName = dataList;
            return View(model);
        }

        public ActionResult ServiceOnBoard()
        {
            List<OnBoardModel> model = new List<OnBoardModel>();
            var tasks = SfbDataHandler.GetOnboardServices();
            foreach (DataRow item in tasks.Rows)
            {
                OnBoardModel modelData = new OnBoardModel();
                modelData.TaskName = item["TaskName"].ToString();
                modelData.SubTasks = item["SubTasks"].ToString();
                modelData.IncludeTask = false;

                model.Add(modelData);
            }
            var servData = SfbDataHandler.GetTrackNamesByType(2);
            List<SelectListItem> dataList = new List<SelectListItem>();
            foreach (DataRow item in servData.Rows)
            {
                SelectListItem data = new SelectListItem();
                data.Text = item["Name"].ToString();
                data.Value = item["Name"].ToString();
                dataList.Add(data);
            }
            ViewBag.ServiceName = dataList;
            return View(model);
        }        
    }
}