using Dashboards_BusinessLayer;
using Dashboards_BusinessLayer.Models;
using Dashboards_Web.CustomAttribute;
using Microsoft.Cloud.Metrics.Client;
using Microsoft.Cloud.Metrics.Client.Metrics;
using Microsoft.Online.Metrics.Serialization.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;

namespace Dashboards_Web.Controllers
{
    [CustomExceptionAttribute]
    public class OCDM_Health_DashboardController : Controller
    {
        readonly IReadMetricDataHandler readMetricDataHandler;
        ConnectionInfo connectionInfo = null;
        string testCertificateThumbprint = ConfigurationManager.AppSettings["Thumbprint"].ToString();

        MetricReader reader = null;
        public OCDM_Health_DashboardController(IReadMetricDataHandler _readMetricDataHandler)
        {
            this.readMetricDataHandler = _readMetricDataHandler;
            connectionInfo = new ConnectionInfo(testCertificateThumbprint, StoreLocation.CurrentUser, MdmEnvironment.Production);
            reader = new MetricReader(connectionInfo);
        }
        public ActionResult Health_Dashboard()
        {
           
            //GetFilteredDimensionValues(60);
            Session["CPUThresholdLow"] = ConfigurationManager.AppSettings["CPUThresholdLow"];
            Session["CPUThresholdHigh"] = ConfigurationManager.AppSettings["CPUThresholdHigh"];
            Session["CPUTimeSpan"] = ConfigurationManager.AppSettings["CPUTimeSpan"];

            Session["MemoryThresholdLow"] = ConfigurationManager.AppSettings["MemoryThresholdLow"];
            Session["MemoryThresholdHigh"] = ConfigurationManager.AppSettings["MemoryThresholdHigh"];

            Session["ADSThresholdLow"] = ConfigurationManager.AppSettings["ADSThresholdLow"];
            Session["ADSThresholdHigh"] = ConfigurationManager.AppSettings["ADSThresholdHigh"];
            Session["ADSTimeSpan"] = ConfigurationManager.AppSettings["ADSTimeSpan"];

            Session["TenantThresholdLow"] = ConfigurationManager.AppSettings["TenantThresholdLow"];
            Session["TenantThresholdHigh"] = ConfigurationManager.AppSettings["TenantThresholdHigh"];
            Session["TenantTimeSpan"] = ConfigurationManager.AppSettings["TenantTimeSpan"];

            Session["UserSubProvisionThresholdLow"] = ConfigurationManager.AppSettings["UserSubProvisionThresholdLow"];
            Session["UserSubProvisionThresholdHigh"] = ConfigurationManager.AppSettings["UserSubProvisionThresholdHigh"];
            Session["UserSubProvisionTimeSpan"] = ConfigurationManager.AppSettings["UserSubProvisionTimeSpan"];


            Session["MNCThresholdLow"] = ConfigurationManager.AppSettings["MNCThresholdLow"];
            Session["MNCThresholdHigh"] = ConfigurationManager.AppSettings["MNCThresholdHigh"];
            Session["UserProvisionEvalValue"] = ConfigurationManager.AppSettings["UserProvisionEvalValue"];

            return View();
        }
        [HttpPost]
        public ActionResult ReadCPUMetricsData(double Interval)
        {
            MetricIdentifier Cpuid = new MetricIdentifier(ConfigurationManager.AppSettings["MonitoringAccountName"].ToString(),
            ConfigurationManager.AppSettings["MetricNameSpace"].ToString(), "\\Processor(_Total)\\% Processor Time");
            List<DivData> readCPUList = this.readMetricDataHandler.ReadCPUMetricsData(reader, Cpuid, Convert.ToDouble(ConfigurationManager.AppSettings["CPUThresholdLow"]), Interval);
           
            var divList = GetMetricsGroupedData(readCPUList);
            
            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadMemoryMetricsData(double Interval)
        {
            MetricIdentifier Cpuid = new MetricIdentifier(ConfigurationManager.AppSettings["MonitoringAccountName"].ToString(),
            ConfigurationManager.AppSettings["MetricNameSpace"].ToString(), "\\Processor(_Total)\\% Processor Time");
            List<DivData> readMemoryList = this.readMetricDataHandler.ReadMemoryMetricsData(reader, Convert.ToDouble(ConfigurationManager.AppSettings["MemoryThresholdLow"]), Interval);
            var divList = GetMetricsGroupedData(readMemoryList);
            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadADSaveLatency(double Interval)
        {
            MetricIdentifier ADSid = new MetricIdentifier(ConfigurationManager.AppSettings["MonitoringAccountName"].ToString(),
            ConfigurationManager.AppSettings["MetricNameSpace"].ToString(), "\\LS:Provision - Provision(_Total)\\- Save latency (sec)");
            List<DivData> adLatencyList = this.readMetricDataHandler.ReadADSaveLatency(reader, ADSid, Convert.ToDouble(ConfigurationManager.AppSettings["ADSThresholdLow"]), Interval);
            var divList = GetMetricsGroupedData(adLatencyList);
            return Json(divList);
        }
        [HttpPost]
        public ActionResult TenantProvisionfailures(double Interval)
        {
            MetricIdentifier Tenanatid = new MetricIdentifier(ConfigurationManager.AppSettings["MonitoringAccountName"].ToString(),
            ConfigurationManager.AppSettings["MetricNameSpace"].ToString(), "\\LS:Provision - Provision(_Total)\\- Tenant Provision failures");
            List<DivData> tenantProvisionfailuresList = this.readMetricDataHandler.TenantProvisionfailures(reader, Tenanatid, Convert.ToDouble(ConfigurationManager.AppSettings["TenantThresholdLow"]), Interval);
            var divList = GetMetricsGroupedData(tenantProvisionfailuresList);
            return Json(divList);
        }
        [HttpPost]
        public ActionResult TenantSubProvisionFailures(double Interval)
        {
            MetricIdentifier TSPid = new MetricIdentifier(ConfigurationManager.AppSettings["MonitoringAccountName"].ToString(),
            ConfigurationManager.AppSettings["MetricNameSpace"].ToString(), "\\LS:Provision - Provision(_Total)\\- Tenant sub-provision failures");
            List<DivData> tenantSubProvisionFailuresList = this.readMetricDataHandler.TenantSubProvisionFailures(reader, TSPid, Convert.ToDouble(ConfigurationManager.AppSettings["TenantThresholdLow"]), Interval);
            var divList = GetMetricsGroupedData(tenantSubProvisionFailuresList);
            return Json(divList);
        }
        [HttpPost]
        public ActionResult TenantUserPublishFailures(double Interval)
        {
            MetricIdentifier TUpublishFid = new MetricIdentifier(ConfigurationManager.AppSettings["MonitoringAccountName"].ToString(),
            ConfigurationManager.AppSettings["MetricNameSpace"].ToString(), "\\LS:Provision - Provision(_Total)\\- Failed publish calls");
            List<DivData> tenantUserpublishfailuresList = this.readMetricDataHandler.TenantUserpublishfailures(reader, TUpublishFid, Convert.ToDouble(ConfigurationManager.AppSettings["TenantThresholdLow"]), Interval);
            var divList = GetMetricsGroupedData(tenantUserpublishfailuresList);
            return Json(divList);
        }
        [HttpPost]
        public ActionResult UserSubProvisionFailures(double Interval)
        {
            MetricIdentifier UPSid = new MetricIdentifier(ConfigurationManager.AppSettings["MonitoringAccountName"].ToString(),
            ConfigurationManager.AppSettings["MetricNameSpace"].ToString(), "\\LS:Provision - Provision(_Total)\\- User sub provision failures");
            List<DivData> userSubProvisionFailuresList = this.readMetricDataHandler.UserSubProvisionFailures(reader, UPSid, Convert.ToDouble(ConfigurationManager.AppSettings["UserSubProvisionThresholdLow"]), Interval < 120 ? 120 : Interval);
            var divList = GetMetricsGroupedData(userSubProvisionFailuresList);
            return Json(divList);
        }
        [HttpPost]
        public ActionResult UserProvisionFailures(double Interval)
        {
            MetricIdentifier UPFid = new MetricIdentifier(ConfigurationManager.AppSettings["MonitoringAccountName"].ToString(),
            ConfigurationManager.AppSettings["MetricNameSpace"].ToString(), "\\LS:Provision - Provision(_Total)\\- User provision failures");
            MetricIdentifier NoOfUsrsPk = new MetricIdentifier(ConfigurationManager.AppSettings["MonitoringAccountName"].ToString(),
                        ConfigurationManager.AppSettings["MetricNameSpace"].ToString(), "\\LS:Provision - Provision(_Total)\\- Number of User Sync Per Second in Peak Mode");
            MetricIdentifier NoOfUsrsNPk = new MetricIdentifier(ConfigurationManager.AppSettings["MonitoringAccountName"].ToString(),
                        ConfigurationManager.AppSettings["MetricNameSpace"].ToString(), "\\LS:Provision - Provision(_Total)\\- Number of User Sync Per Second in None Peak Mode");

            List<DivData> userProvisionFailuresList = this.readMetricDataHandler.UserProvisionFailures(reader, UPFid, NoOfUsrsPk, NoOfUsrsNPk, Convert.ToDouble(ConfigurationManager.AppSettings["ProvisionThreshold"]), Interval);
            var divList = GetMetricsGroupedData(userProvisionFailuresList);
            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadNumberofMNCUsersFailed(double Interval)
        {
            MetricIdentifier MNCUSersid = new MetricIdentifier(ConfigurationManager.AppSettings["MonitoringAccountName"].ToString(),
            ConfigurationManager.AppSettings["MetricNameSpace"].ToString(), "\\LS:Provision - XForestMoveHandler(_Total)\\- Number of MNC Users Successfully Moved during Cross Region Move");
            MetricIdentifier MNCUSersFid = new MetricIdentifier(ConfigurationManager.AppSettings["MonitoringAccountName"].ToString(),
            ConfigurationManager.AppSettings["MetricNameSpace"].ToString(), "\\LS:Provision - XForestMoveHandler(_Total)\\- Number of MNC Users Failed during Cross Region Move");
            List<DivData> readNumberofMNCUsersFailedList = this.readMetricDataHandler.ReadNumberofMNCUsersFailed(reader, MNCUSersFid, MNCUSersid, Convert.ToDouble(ConfigurationManager.AppSettings["MNCThreshold"]), Interval);
            var divList = GetMetricsGroupedData(readNumberofMNCUsersFailedList);
            return Json(divList);
        }
        [HttpPost]
        public ActionResult ProvisioningQueue(double Interval)
        {
            MetricIdentifier ProQid = new MetricIdentifier(ConfigurationManager.AppSettings["MonitoringAccountName"].ToString(),
            ConfigurationManager.AppSettings["MetricNameSpace"].ToString(), "\\LS:Provision - Provision(_Total)\\- Number of Provision Items Processed Per Sec");
            List<DivData> provisioningQueueList = this.readMetricDataHandler.ProvisioningQueue(reader, ProQid, Convert.ToDouble(ConfigurationManager.AppSettings["ProvisionThreshold"]), Interval);
            var divList = GetMetricsGroupedData(provisioningQueueList);
            return Json(divList);
        }
        [HttpPost]
        public ActionResult SubProvisioningQueue(double Interval)
        {
            MetricIdentifier SPQid = new MetricIdentifier(ConfigurationManager.AppSettings["MonitoringAccountName"].ToString(),
            ConfigurationManager.AppSettings["MetricNameSpace"].ToString(), "\\LS:Provision - Provision(_Total)\\- Number of SubProvision Items Processed Per Sec");
            List<DivData> subProvisioningQueueList = this.readMetricDataHandler.SubProvisioningQueue(reader, SPQid, Convert.ToDouble(ConfigurationManager.AppSettings["ProvisionThreshold"]), Interval);
            var divList = GetMetricsGroupedData(subProvisioningQueueList);
            return Json(divList);
        }
        [HttpPost]
        public ActionResult PublishingQueue(double Interval)
        {
            MetricIdentifier PUQid = new MetricIdentifier(ConfigurationManager.AppSettings["MonitoringAccountName"].ToString(),
            ConfigurationManager.AppSettings["MetricNameSpace"].ToString(), "\\LS:Provision - Provision(_Total)\\- Number of Publish Items Processed Per Sec");
            List<DivData> publishingQueueList = this.readMetricDataHandler.PublishingQueue(reader, PUQid, Convert.ToDouble(ConfigurationManager.AppSettings["ProvisionThreshold"]), Interval);
            var divList = GetMetricsGroupedData(publishingQueueList);
            return Json(divList);
        }
        private IGrouping<(string, string), DivData>[] GetMetricsGroupedData(List<DivData> divList)
        {
            if (divList != null)
            {
                var groupedData = divList.OrderBy(a => a.PercentageFree).GroupBy(x => (x.Forest, x.DivColor)).ToArray();
                return groupedData;
            }
            return null;
        }
    }
}