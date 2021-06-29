using Dashboards_BusinessLayer.Interfaces;
using Dashboards_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dashboards_Web.Controllers
{
    public class VsoDataController : Controller
    {
        // GET: VsoData
        readonly ITaskHandler iTaskHandler;
        public VsoDataController(ITaskHandler iTaskHandler)
        {
            this.iTaskHandler = iTaskHandler;
        }
        public ActionResult Index()
        {
            List<TaskModel> model = new List<TaskModel>();
            var data = iTaskHandler.GetVsoData();
            foreach (var item in data)
            {
                model.Add(new TaskModel {Id=item.Id,CreatedDate=item.CreatedDate,MachineCount=item.MachineCount,Machines=item.Machines,Title=item.Title });
            }
            ViewBag.LstUpdatedDt = model.Select(a => a.CreatedDate).First();
            return View(model);
        }
    }
}