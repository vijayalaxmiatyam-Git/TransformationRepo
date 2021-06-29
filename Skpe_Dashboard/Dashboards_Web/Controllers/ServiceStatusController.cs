using Dashboards_BusinessLayer;
using Dashboards_Web.Models;
using Microsoft.Cloud.Metrics.Client;
using Microsoft.Cloud.Metrics.Client.Metrics;
using Microsoft.Online.Metrics.Serialization.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;

namespace Dashboards_Web.Controllers
{
    public class ServiceStatusController : Controller
    {
        readonly IReadMetricDataHandler readMetricDataHandler;
        ConnectionInfo connectionInfo = null;
        string testCertificateThumbprint = ConfigurationManager.AppSettings["Thumbprint"].ToString();
        // GET: MachineStatus

        MetricReader reader = null;
        public ServiceStatusController(IReadMetricDataHandler _readMetricDataHandler)
        {
            this.readMetricDataHandler = _readMetricDataHandler;
            connectionInfo = new ConnectionInfo(testCertificateThumbprint, StoreLocation.CurrentUser, MdmEnvironment.Production);
            reader = new MetricReader(connectionInfo);
        }

        public ActionResult ServiceStatus(StatusModel model, string time)
        {
            MetricIdentifier Cpuid = new MetricIdentifier(ConfigurationManager.AppSettings["MonitoringAccountName"].ToString(),
                ConfigurationManager.AppSettings["MetricNameSpace1"].ToString(), "ServiceStatus");
            List<MachineStatusModel> machineModel = new List<MachineStatusModel>();

            var data = readMetricDataHandler.ReadCPUStatusData(reader, Cpuid, Convert.ToDouble(ConfigurationManager.AppSettings["CPUStatusThresholdLow"]), string.IsNullOrEmpty(time) ? 30 : Convert.ToInt16(time) * 60);
            foreach (var item in data)
            {
                machineModel.Add(new MachineStatusModel
                { Environment = item.Environment, ForestName = item.ForestName, Machine = item.Machine, PoolFqdn = item.PoolFqdn, Role = item.Role, ServiceName = item.ServiceName, Status = item.Status, EvaluatedResult = item.EvaluatedResult, LastUpdatedValue = item.LastUpdatedValue, LastUpdatedDate = item.LastUpdatedDate });
            }
            machineModel = machineModel.OrderByDescending(a => a.Status).ToList();
            model.Machines = machineModel
                .Where(
                    x =>
                    (model.ForestName == null || x.ForestName.ToUpper().Contains(model.ForestName.ToUpper()))
                    && (model.Machine == null || x.Machine.ToUpper().Contains(model.Machine.ToUpper()))
                    && (model.Status == null || x.Status.ToUpper().Contains(model.Status.ToUpper()))
                    && (model.ServiceName == null || x.ServiceName.ToUpper().Contains(model.ServiceName.ToUpper()))
                   )
                .Skip((model.Page - 1) * model.PageSize)
                .Take(model.PageSize)
                .ToList();

            // total records for paging
            model.TotalRecords = machineModel
                .Count(x =>
                    (model.ForestName == null || x.ForestName.ToUpper().Contains(model.ForestName.ToUpper()))
                    && (model.Machine == null || x.Machine.ToUpper().Contains(model.Machine.ToUpper()))
                    && (model.Status == null || x.Status.ToUpper().Contains(model.Status.ToUpper()))
                   && (model.ServiceName == null || x.ServiceName.ToUpper().Contains(model.ServiceName.ToUpper()))
                   );
            ViewBag.TotalForest = model.Machines.GroupBy(a => a.ForestName).Count();
            ViewBag.TotalMachine = model.Machines.GroupBy(a => a.Machine).Count();
            ViewBag.MachinesRunning = model.Machines.Count(a => a.Status == "Running");
            ViewBag.StopedCount = model.Machines.Count(a => a.Status == "Stopped");
            ViewBag.StopPendingCount = model.Machines.Count(a => a.Status == "StopPending");
            ViewBag.StartPending = model.Machines.Count(a => a.Status == "StartPending");
            return View(model);
        }
    }
}
