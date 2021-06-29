using Dashboards_BusinessLayer.Handlers.Interfaces;
using Dashboards_BusinessLayer.Models;
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
    public class MSODS_DashboardController : Controller
    {
        readonly IMsodsHandler readMetricDataHandler;
        ConnectionInfo connectionInfo = null;
        MetricIdentifier account;
        string testCertificateThumbprint = ConfigurationManager.AppSettings["Thumbprint"].ToString();

        MetricReader reader = null;
        public MSODS_DashboardController(IMsodsHandler _readMetricDataHandler)
        {
            this.readMetricDataHandler = _readMetricDataHandler;
            connectionInfo = new ConnectionInfo(testCertificateThumbprint, StoreLocation.CurrentUser, MdmEnvironment.Production);
            reader = new MetricReader(connectionInfo);
        }
        public ActionResult MSODSDashboard()
        {
            //MetricIdentifier account = new MetricIdentifier(ConfigurationManager.AppSettings["MsAccountName"].ToString(),
            //ConfigurationManager.AppSettings["MsNameSpace"].ToString(), "SyncStreamBacklog_incrementalchangesusnbacklog");
            //var readForestList = this.readMetricDataHandler.ReadMsodsMetricsData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), 360);

            //var divList = GetMetricsGroupedData(readForestList);
            Session["MSODSThresholdLow"] = ConfigurationManager.AppSettings["MSODSThresholdLow"];
            Session["MSODSThresholdHigh"] = ConfigurationManager.AppSettings["MSODSThresholdHigh"];
            Session["MSODSTimeSpan"] = ConfigurationManager.AppSettings["MSODSTimeSpan"];
            return View();
        }
        [HttpPost]
        public ActionResult ReadForestAMetricsData(double Interval)
        {
            GetAccount(ref account);
            var readForestList = this.readMetricDataHandler.ReadMSODSMetricData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), Interval, "ForestA");
            var divList = GetMetricsGroupedData(readForestList, "ForestA");

            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadForest1AMetricsData(double Interval)
        {
            GetAccount(ref account);
            var readForestList = this.readMetricDataHandler.ReadMSODSMetricData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), Interval, "Forest1A");
            var divList = GetMetricsGroupedData(readForestList, "Forest1A");

            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadForest2AMetricsData(double Interval)
        {
            GetAccount(ref account);
            var readForestList = this.readMetricDataHandler.ReadMSODSMetricData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), Interval, "Forest2A");
            var divList = GetMetricsGroupedData(readForestList, "Forest2A");

            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadForest3AMetricsData(double Interval)
        {
            GetAccount(ref account);
            var readForestList = this.readMetricDataHandler.ReadMSODSMetricData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), Interval, "Forest3A");
            var divList = GetMetricsGroupedData(readForestList, "Forest3A");

            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadForest4AMetricsData(double Interval)
        {
            GetAccount(ref account);
            var readForestList = this.readMetricDataHandler.ReadMSODSMetricData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), Interval, "Forest4A");
            var divList = GetMetricsGroupedData(readForestList, "Forest4A");

            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadForestBMetricsData(double Interval)
        {
            GetAccount(ref account);
            var readForestList = this.readMetricDataHandler.ReadMSODSMetricData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), Interval, "ForestB");
            var divList = GetMetricsGroupedData(readForestList, "ForestB");

            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadForest1BMetricsData(double Interval)
        {
            GetFairfaxAccount(ref account);
            var readForestList = this.readMetricDataHandler.ReadMSODSMetricData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), Interval, "Forest1B");
            var divList = GetMetricsGroupedData(readForestList, "Forest1B");

            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadForestEMetricsData(double Interval)
        {
            GetAccount(ref account);
            var readForestList = this.readMetricDataHandler.ReadMSODSMetricData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), Interval, "ForestE");
            var divList = GetMetricsGroupedData(readForestList, "ForestE");

            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadForest1EMetricsData(double Interval)
        {
            GetAccount(ref account);
            var readForestList = this.readMetricDataHandler.ReadMSODSMetricData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), Interval, "Forest1E");
            var divList = GetMetricsGroupedData(readForestList, "Forest1E");

            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadForest2EMetricsData(double Interval)
        {
            GetAccount(ref account);
            var readForestList = this.readMetricDataHandler.ReadMSODSMetricData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), Interval, "Forest2E");
            var divList = GetMetricsGroupedData(readForestList, "Forest2E");

            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadForest3EMetricsData(double Interval)
        {
            GetAccount(ref account);
            var readForestList = this.readMetricDataHandler.ReadMSODSMetricData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), Interval, "Forest3E");
            var divList = GetMetricsGroupedData(readForestList, "Forest3E");

            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadForest4EMetricsData(double Interval)
        {
            GetAccount(ref account);
            var readForestList = this.readMetricDataHandler.ReadMSODSMetricData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), Interval, "Forest4E");
            var divList = GetMetricsGroupedData(readForestList, "Forest4E");

            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadForestFMetricsData(double Interval)
        {
            GetAccount(ref account);
            var readForestList = this.readMetricDataHandler.ReadMSODSMetricData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), Interval, "ForestF");
            var divList = GetMetricsGroupedData(readForestList, "ForestF");

            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadForest0GMetricsData(double Interval)
        {
            GetGallatinAccount(ref account);
            var readForestList = this.readMetricDataHandler.ReadMSODSMetricData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), Interval, "Forest0G");
            var divList = GetMetricsGroupedData(readForestList, "Forest0G");

            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadForest1GMetricsData(double Interval)
        {
            GetArlingtonAccount(ref account);
            var readForestList = this.readMetricDataHandler.ReadMSODSMetricData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), Interval, "Forest1G");
            var divList = GetMetricsGroupedData(readForestList, "Forest1G");

            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadForest2GMetricsData(double Interval)
        {
            GetArlingtonAccount(ref account);
            var readForestList = this.readMetricDataHandler.ReadMSODSMetricData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), Interval, "Forest2G");
            var divList = GetMetricsGroupedData(readForestList, "Forest2G");

            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadForest0MMetricsData(double Interval)
        {
            GetAccount(ref account);
            var readForestList = this.readMetricDataHandler.ReadMSODSMetricData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), Interval, "Forest0M");
            var divList = GetMetricsGroupedData(readForestList, "Forest0M");

            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadForest1MMetricsData(double Interval)
        {
            GetAccount(ref account);
            var readForestList = this.readMetricDataHandler.ReadMSODSMetricData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), Interval, "Forest1M");
            var divList = GetMetricsGroupedData(readForestList, "Forest1M");

            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadForestAN1MetricsData(double Interval)
        {
            GetAccount(ref account);
            var readForestList = this.readMetricDataHandler.ReadMSODSMetricData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), Interval, "ForestAN1");
            var divList = GetMetricsGroupedData(readForestList, "ForestAN1");
            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadForestAU1MetricsData(double Interval)
        {
            GetAccount(ref account);
            var readForestList = this.readMetricDataHandler.ReadMSODSMetricData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), Interval, "ForestAU1");
            var divList = GetMetricsGroupedData(readForestList, "ForestAU1");
            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadForestCA1MetricsData(double Interval)
        {
            GetAccount(ref account);
            var readForestList = this.readMetricDataHandler.ReadMSODSMetricData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), Interval, "ForestCA1");
            var divList = GetMetricsGroupedData(readForestList, "ForestCA1");
            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadForestDE1MetricsData(double Interval)
        {
            GetBlkFrstAccount(ref account);
            var readForestList = this.readMetricDataHandler.ReadMSODSMetricData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), Interval, "ForestDE1");
            var divList = GetMetricsGroupedData(readForestList, "ForestDE1");
            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadForestED1MetricsData(double Interval)
        {
            GetAccount(ref account);
            var readForestList = this.readMetricDataHandler.ReadMSODSMetricData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), Interval, "ForestED1");
            var divList = GetMetricsGroupedData(readForestList, "ForestED1");
            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadForestED2MetricsData(double Interval)
        {
            GetAccount(ref account);
            var readForestList = this.readMetricDataHandler.ReadMSODSMetricData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), Interval, "ForestED2");
            var divList = GetMetricsGroupedData(readForestList, "ForestED2");
            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadForestED3MetricsData(double Interval)
        {
            GetAccount(ref account);
            var readForestList = this.readMetricDataHandler.ReadMSODSMetricData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), Interval, "ForestED3");
            var divList = GetMetricsGroupedData(readForestList, "ForestED3");
            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadForestED4MetricsData(double Interval)
        {
            GetAccount(ref account);
            var readForestList = this.readMetricDataHandler.ReadMSODSMetricData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), Interval, "ForestED4");
            var divList = GetMetricsGroupedData(readForestList, "ForestED4");
            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadForestED5MetricsData(double Interval)
        {
            GetAccount(ref account);
            var readForestList = this.readMetricDataHandler.ReadMSODSMetricData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), Interval, "ForestED5");
            var divList = GetMetricsGroupedData(readForestList, "ForestED5");
            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadForestED6MetricsData(double Interval)
        {
            GetAccount(ref account);
            var readForestList = this.readMetricDataHandler.ReadMSODSMetricData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), Interval, "ForestED6");
            var divList = GetMetricsGroupedData(readForestList, "ForestED6");
            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadForestGB1MetricsData(double Interval)
        {
            GetAccount(ref account);
            var readForestList = this.readMetricDataHandler.ReadMSODSMetricData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), Interval, "ForestGB1");
            var divList = GetMetricsGroupedData(readForestList, "ForestGB1");
            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadForestIN1MetricsData(double Interval)
        {
            GetAccount(ref account);
            var readForestList = this.readMetricDataHandler.ReadMSODSMetricData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), Interval, "ForestIN1");
            var divList = GetMetricsGroupedData(readForestList, "ForestIN1");
            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadForestJP1MetricsData(double Interval)
        {
            GetAccount(ref account);
            var readForestList = this.readMetricDataHandler.ReadMSODSMetricData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), Interval, "ForestJP1");
            var divList = GetMetricsGroupedData(readForestList, "ForestJP1");
            return Json(divList);
        }
        [HttpPost]
        public ActionResult ReadForestKR1MetricsData(double Interval)
        {
            GetAccount(ref account);
            var readForestList = this.readMetricDataHandler.ReadMSODSMetricData(reader, account, Convert.ToDouble(ConfigurationManager.AppSettings["MSODSThresholdLow"]), Interval, "ForestKR1");
            var divList = GetMetricsGroupedData(readForestList, "ForestKR1");
            return Json(divList);
        }
        private void GetAccount(ref MetricIdentifier account)
        {
            account = new MetricIdentifier(ConfigurationManager.AppSettings["MsAccountName"].ToString(),
            ConfigurationManager.AppSettings["MsNameSpace"].ToString(), "SyncStreamBacklog_incrementalchangesusnbacklog");
        }
        private void GetFairfaxAccount(ref MetricIdentifier account)
        {
            account = new MetricIdentifier(ConfigurationManager.AppSettings["MsFrfxAccountName"].ToString(),
            ConfigurationManager.AppSettings["MsNameSpace"].ToString(), "SyncStreamBacklog_incrementalchangesusnbacklog");
        }
        private void GetGallatinAccount(ref MetricIdentifier account)
        {
            account = new MetricIdentifier(ConfigurationManager.AppSettings["MsGallatinAccountName"].ToString(),
            ConfigurationManager.AppSettings["MsNameSpace"].ToString(), "SyncStreamBacklog_incrementalchangesusnbacklog");
        }
        private void GetBlkFrstAccount(ref MetricIdentifier account)
        {
            account = new MetricIdentifier(ConfigurationManager.AppSettings["MsBlackForestAccountName"].ToString(),
            ConfigurationManager.AppSettings["MsNameSpace"].ToString(), "SyncStreamBacklog_incrementalchangesusnbacklog");
        }
        private void GetArlingtonAccount(ref MetricIdentifier account)
        {
            account = new MetricIdentifier(ConfigurationManager.AppSettings["MsArlingtonAccountName"].ToString(),
            ConfigurationManager.AppSettings["MsNameSpace"].ToString(), "PartitionSyncStreamBacklog_incrementalchangesusnbacklog");
        }
        private IGrouping<(string, string), Models.MsodsModel>[] GetMetricsGroupedData(List<MSODSData> divList, string forestName)
        {
            if (divList != null)
            {
                List<MsodsModel> groupedDataList = new List<MsodsModel>();
                foreach (var item in divList)
                {
                    List<Models.Datapoints> list = new List<Models.Datapoints>();
                    foreach (var i in item.Datapoints)
                    {
                        list.Add(new Models.Datapoints { TimestampUtc = i.TimestampUtc, Value = i.Value });
                    }
                    groupedDataList.Add(
                        new MsodsModel
                        {
                            Color = item.Color,
                            divId = item.divId,
                            ServiceInstance = item.ServiceInstance,
                            ServiceInstanceShortName = item.ServiceInstance.Split('/')[1],
                            ServiceType = item.ServiceType,
                            SyncStreamIdentifier = item.SyncStreamIdentifier,
                            DatapointsList = list,
                            EvaluatedResult = item.EvaluatedResult,
                            URL = "https://jarvis-west.dc.ad.msft.net/dashboard/SkypePerfProd/PerfCounters/Provisioning/MSODS/Service%2520Instances%2520(Prod)?overrides=[{%22query%22:%22//*[id=%27serviceinstance%27]%22,%22key%22:%22value%22,%22replacement%22:%22" + item.ServiceInstance + "%22},{%22query%22:%22//*[id=%27servicetype%27]%22,%22key%22:%22value%22,%22replacement%22:%22microsoftcommunicationsonline%22},{%22query%22:%22//*[id=%27syncstreamidentifier%27]%22,%22key%22:%22value%22,%22replacement%22:%22" + item.SyncStreamIdentifier + "%22},{%22query%22:%22//*[id=%27Forest%27]%22,%22key%22:%22value%22,%22replacement%22:%22" + forestName + "%22}]%20"
                        });
                }
                var groupedData = groupedDataList.OrderByDescending(a => a.Color).GroupBy(x => (x.ServiceInstance, x.Color)).ToArray();
                return groupedData;
            }
            return null;
        }

    }
}