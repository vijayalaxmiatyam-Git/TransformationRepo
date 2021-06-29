using System;
using System.Configuration;
using Microsoft.Cloud.Metrics.Client.Metrics;
using Microsoft.Cloud.Metrics.Client.Query;
using Microsoft.Online.Metrics.Serialization.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.Cloud.Metrics.Client;
using Newtonsoft.Json;
using Dashboards_BusinessLayer.Models;

namespace Dashboards_BusinessLayer
{
    public class ReadMetricDataHandler : IReadMetricDataHandler
    {
        [Obsolete]
        public List<DivData> ReadCPUMetricsData(MetricReader reader, MetricIdentifier id, double CPUThreshold, double Interval)
        {
            var dimensionFilters = new DimensionFilter[] { "Forest", DimensionFilter.CreateIncludeFilter("Role", "PRV"), "PoolFQDN", "Machine", "Instance" };
            List<DivData> divlist = new List<DivData>();

            IEnumerable<IQueryResult> results = reader.GetFilteredDimensionValuesAsync(
                id,
                dimensionFilters,
                DateTime.UtcNow.AddMinutes(-(Interval)),
                DateTime.UtcNow,
                SamplingType.Average,
                Reducer.Max,
                new QueryFilter(Operator.GreaterThanOrEqual, CPUThreshold),
                true,
                new SelectionClause(SelectionType.TopValues, 200, OrderBy.Descending)
                ).Result;

            CPUTimePerMachine ObjCPU = new CPUTimePerMachine();
            int i = 1;

            if (results.Count() > 0)
            {
                foreach (var r in results)
                {

                    ObjCPU.Forest = r.DimensionList[0].Value.ToString();
                    ObjCPU.Machine = r.DimensionList[2].Value.ToString();
                    ObjCPU.Metrics = "CPUTimePerMachine";
                    ObjCPU.PoolFQDN = r.DimensionList[3].Value.ToString();
                    ObjCPU.Datapoints = new List<DatapointsTimeWise>();


                    var definition = new TimeSeriesDefinition<MetricIdentifier>(
                    id,
                    new Dictionary<string, string> { { "Forest", ObjCPU.Forest }, { "Role", "PRV" }, { "Machine", ObjCPU.Machine }, { "PoolFQDN", ObjCPU.PoolFQDN } });// { "PoolFQDN", "*" }, { "Machine", "*" }, { "Instance", "*" } });

                    TimeSeries<MetricIdentifier, double?> result =
                        reader.GetTimeSeriesAsync(DateTime.UtcNow.AddMinutes(-Interval), DateTime.UtcNow, SamplingType.Average, definition).Result;


                    var re = JsonConvert.SerializeObject(result.Datapoints);

                    var thresholdWiseDatapoints = JsonConvert.DeserializeObject<List<DatapointsTimeWise>>(re).Where(val => val.Value >= CPUThreshold);
                    ObjCPU.Datapoints.AddRange(thresholdWiseDatapoints);
                    DivData list;
                    if (thresholdWiseDatapoints.Count() >= 50)// Interval 60 / 360 // Orange/ Red/ Green
                    {
                        //continous any where 6more than 50 occurence0 came or not by using 
                        list = new DivData { divId = "divCPU" + i, Forest = ObjCPU.Forest, Machine = ObjCPU.Machine, Datapoints = ObjCPU.Datapoints, PoolFQDN = ObjCPU.PoolFQDN, PercentageFree = null };
                        GetCPUDataByThreshold(ref list, JsonConvert.DeserializeObject<List<DatapointsTimeWise>>(re), Convert.ToDouble(ConfigurationManager.AppSettings["ADSThresholdLow"]), Convert.ToDouble(ConfigurationManager.AppSettings["ADSThresholdHigh"]), Convert.ToInt16(ConfigurationManager.AppSettings["CPUTimeSpan"]));
                    }
                    else // Green
                    {
                        list = new DivData { divId = "divCPU" + i, Forest = ObjCPU.Forest, Machine = ObjCPU.Machine, Datapoints = ObjCPU.Datapoints, PoolFQDN = ObjCPU.PoolFQDN, PercentageFree = null, DivColor = "green" };
                    }


                    //CreateDiv("divCPU" + i, ObjCPU.Forest, ObjCPU.Machine, ObjCPU.PoolFQDN, ObjCPU.Datapoints,null);
                    divlist.Add(list);

                    i++;
                }
                //CreateDivs(divlist);
            }
            else
            {
                //CreateDiv("divCPU" + i, "All Forest", ObjCPU.Machine, ObjCPU.PoolFQDN, ObjCPU.Datapoints, null, "green");
                //divlist.Add(new DivData { divId = "All Forest" + i, Forest = ObjCPU.Forest, Machine = ObjCPU.Machine, Datapoints = ObjCPU.Datapoints, PoolFQDN = ObjCPU.PoolFQDN, PercentageFree = null });
            }

            return divlist;
            //GetCPUDataByThreshold(ref divlist, CPUThreshold);
        }
        public void GetCPUDataByThreshold(ref DivData obj, IEnumerable<DatapointsTimeWise> datapointsTimeWise, double lowThrshld, double highThrshld, int timeSpan)
        {
            try
            {

                int i = 0;
                while (i < datapointsTimeWise.Count() / timeSpan)
                {
                    int j = i * timeSpan;
                    int dataCountOrng = 0;
                    int dataCountCrl = 0;
                    for (int lp = j; lp < j + timeSpan; lp++)
                    {

                        if (datapointsTimeWise.ToArray()[lp].Value > lowThrshld && datapointsTimeWise.ToArray()[lp].Value < highThrshld)
                        {
                            dataCountOrng++;
                        }
                        else if (datapointsTimeWise.ToArray()[lp].Value >= highThrshld)
                        {
                            dataCountCrl++;
                        }
                    }
                    if (dataCountOrng == timeSpan)
                    {
                        obj.DivColor = "darkorange";
                    }
                    if (dataCountCrl == timeSpan)
                    {
                        obj.DivColor = "red";
                    }
                    obj.DivColor = "green";
                    i++;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Obsolete]
        public List<DivData> ReadMemoryMetricsData(MetricReader reader, double MemoryThreshold, double Interval)
        {
            List<DivData> divlist = new List<DivData>();
            MetricIdentifier id = new MetricIdentifier(ConfigurationManager.AppSettings["MonitoringAccountName"].ToString(),
            ConfigurationManager.AppSettings["MetricNameSpace"].ToString(), "\\Memory\\Total Physical MBytes");


            var dimensionFilters = new DimensionFilter[] { DimensionFilter.CreateExcludeFilter("Forest", "ForestREDMOND"), "Machine", "PoolFQDN", DimensionFilter.CreateIncludeFilter("Role", "PRV") };

            IEnumerable<IQueryResult> results = reader.GetFilteredDimensionValuesAsync(
            id,
            dimensionFilters,
            DateTime.UtcNow.AddSeconds(-(1)),
            DateTime.UtcNow,
            SamplingType.Max,
            Reducer.Average,
            new QueryFilter(Operator.GreaterThanOrEqual, MemoryThreshold),
            false,
            new SelectionClause(SelectionType.TopValues, 40, OrderBy.Descending)
            ).Result;

            MetricIdentifier memid = new MetricIdentifier(ConfigurationManager.AppSettings["MonitoringAccountName"].ToString(),
            ConfigurationManager.AppSettings["MetricNameSpace"].ToString(), "\\Memory\\Available MBytes");

            Memory_Metrics ObjMem = new Memory_Metrics();
            int i = 1;
            if (results.Count() > 0)
            {
                foreach (var series in results)
                {

                    ObjMem.Forest = series.DimensionList[0].Value.ToString();
                    ObjMem.Machine = series.DimensionList[1].Value.ToString();
                    ObjMem.Metrics = "CPUTimePerMachine";
                    ObjMem.PoolFQDN = series.DimensionList[2].Value.ToString();
                    if (ObjMem.Forest == "ForestA")
                    {

                    }
                    if (series.EvaluatedResult != null)
                    {
                        ObjMem.TotalPhysicalMBytes = Convert.ToDouble(series.EvaluatedResult);
                    }

                    var dimensionFilters1 = new DimensionFilter[] { DimensionFilter.CreateIncludeFilter("Forest", ObjMem.Forest), DimensionFilter.CreateIncludeFilter("Role", "PRV"), DimensionFilter.CreateIncludeFilter("Machine", ObjMem.Machine), DimensionFilter.CreateIncludeFilter("PoolFQDN", ObjMem.PoolFQDN) };

                    IEnumerable<IQueryResult> TotalMBytesCount = reader.GetFilteredDimensionValuesAsync(
                    memid,
                    dimensionFilters1,
                    DateTime.UtcNow.AddMinutes(-(Interval)),
                    DateTime.UtcNow,
                    SamplingType.Count,
                    Reducer.Average,
                    new QueryFilter(Operator.GreaterThanOrEqual, 0),
                    false,
                    new SelectionClause(SelectionType.TopValues, 1, OrderBy.Descending)
                    ).Result;
                    if (TotalMBytesCount != null)
                    {
                        foreach (var series1 in TotalMBytesCount)
                        {

                            if (series1.EvaluatedResult != null)
                            {
                                ObjMem.MemoryAvailableMBytes = Convert.ToDouble(series1.EvaluatedResult) * 100;
                            }
                        }
                    }

                    IEnumerable<IQueryResult> AvailableMBytesAvg = reader.GetFilteredDimensionValuesAsync(
                   memid,
                   dimensionFilters1,
                   DateTime.UtcNow.AddMinutes(-(Interval)),
                   DateTime.UtcNow,
                   SamplingType.Average,
                   Reducer.Average,
                   new QueryFilter(Operator.GreaterThanOrEqual, 0),
                   true,
                   new SelectionClause(SelectionType.TopValues, 10, OrderBy.Descending)
                   ).Result;

                    if (AvailableMBytesAvg != null)
                    {
                        foreach (var series2 in AvailableMBytesAvg)
                        {

                            if (series2.EvaluatedResult != null)
                            {
                                ObjMem.MemoryAvailableMByteAVG = Convert.ToDouble(series2.EvaluatedResult);
                            }
                        }
                    }

                    if (ObjMem.MemoryAvailableMByteAVG > 0)
                    {
                        if (ObjMem.TotalPhysicalMBytes > 0)
                        {
                            ObjMem.PercentageFree = ((((ObjMem.TotalPhysicalMBytes > 0) && (ObjMem.MemoryAvailableMBytes > 0))) ? ((ObjMem.MemoryAvailableMByteAVG / ObjMem.TotalPhysicalMBytes)) : (1.0));
                        }
                        else
                        {
                            ObjMem.PercentageFree = ((((ObjMem.TotalPhysicalMBytes > 0) && (ObjMem.MemoryAvailableMBytes > 0))) ? ((ObjMem.MemoryAvailableMByteAVG / 1)) : (1.0));
                        }
                        ObjMem.PercentageFree = ObjMem.PercentageFree * 100;
                        DivData data = new DivData(); //= //new DivData { divId = "divMem" + i, Forest = ObjMem.Forest, Machine = ObjMem.Machine, Datapoints = null, PercentageFree = ObjMem.PercentageFree, PoolFQDN = ObjMem.PoolFQDN };

                        if (ObjMem != null && ObjMem.PercentageFree <= MemoryThreshold)
                        {
                            data = new DivData { divId = "divMem" + i, Forest = ObjMem.Forest, Machine = ObjMem.Machine, Datapoints = null, PercentageFree = ObjMem.PercentageFree, PoolFQDN = ObjMem.PoolFQDN };
                            GetMemoryThresholdData(ref data, ConfigurationManager.AppSettings["MemoryThresholdLow"].ToString(), ConfigurationManager.AppSettings["MemoryThresholdHigh"].ToString());
                        }
                        if (!string.IsNullOrEmpty(data.divId))
                            divlist.Add(data);
                    }
                }
                //if (divlist.Count() > 0)
                //{
                //    CreateDivs(divlist);
                //}
                //else
                //{
                //    CreateDiv("divMem" + i, "All Forest", ObjMem.Machine, ObjMem.PoolFQDN, null, ObjMem.PercentageFree, "green");
                //}

            }
            //else
            //{
            //    CreateDiv("divMem" + i, "All Forest", ObjMem.Machine, ObjMem.PoolFQDN, null, ObjMem.PercentageFree, "green");
            //    //divlist.Add(new DivData { divId = "divMem" + i, Forest = "All Forest", Machine = ObjMem.Machine, Datapoints = null, PercentageFree = ObjMem.PercentageFree, PoolFQDN = ObjMem.PoolFQDN });
            //}
            return divlist;
        }
        public void GetMemoryThresholdData(ref DivData data, string trshldLow, string trshldHigh)
        {
            try
            {
                if (data.PercentageFree > Convert.ToDouble(trshldLow))
                {
                    data.DivColor = "green";
                }
                else if (data.PercentageFree < Convert.ToDouble(trshldLow) && data.PercentageFree > Convert.ToDouble(trshldHigh))
                {
                    data.DivColor = "darkorange";
                }
                else if (data.PercentageFree <= Convert.ToDouble(trshldHigh))
                {
                    data.DivColor = "red";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Obsolete]
        public List<DivData> ReadADSaveLatency(MetricReader reader, MetricIdentifier id, double Threshold, double Interval)
        {
            var dimensionFilters = new DimensionFilter[] { "Forest", DimensionFilter.CreateIncludeFilter("Role", "PRV"), "PoolFQDN", DimensionFilter.CreateIncludeFilter("Instance", "_Total") };
            List<DivData> divlist = new List<DivData>();
            IEnumerable<IQueryResult> results = reader.GetFilteredDimensionValuesAsync(
                id,
                dimensionFilters,
                DateTime.UtcNow.AddMinutes(-(Interval)),
                DateTime.UtcNow,
                SamplingType.Average,
                Reducer.Average,
                new QueryFilter(Operator.GreaterThanOrEqual, Threshold),
                true,
                new SelectionClause(SelectionType.TopValues, 200, OrderBy.Descending)
                ).Result;


            CPUTimePerMachine ObjCPU = new CPUTimePerMachine();
            int i = 1;
            if (results.Count() > 0)
            {
                foreach (var r in results)
                {

                    ObjCPU.Forest = r.DimensionList[0].Value.ToString();
                    ObjCPU.Machine = "";
                    ObjCPU.Metrics = "AD Save Latency";
                    ObjCPU.PoolFQDN = r.DimensionList[2].Value.ToString();
                    ObjCPU.Datapoints = new List<DatapointsTimeWise>();
                    var definition = new TimeSeriesDefinition<MetricIdentifier>(
                    id,
                    new Dictionary<string, string> { { "Forest", ObjCPU.Forest }, { "Role", "PRV" }, { "PoolFQDN", ObjCPU.PoolFQDN } });

                    //if (ObjCPU.Forest == "Forest4A" || ObjCPU.Forest == "Forest3DE")
                    //{
                    //    int a = 0;
                    //}
                    TimeSeries<MetricIdentifier, double?> result =
                        reader.GetTimeSeriesAsync(DateTime.UtcNow.AddMinutes(-Interval), DateTime.UtcNow, SamplingType.Average, definition).Result;

                    var re = JsonConvert.SerializeObject(result.Datapoints);
                    var pageOfCPUMetrics = JsonConvert.DeserializeObject<List<DatapointsTimeWise>>(re).Where(val => val.Value >= Threshold);

                    DivData list;
                    ObjCPU.Datapoints.AddRange(pageOfCPUMetrics);

                    if (pageOfCPUMetrics.Count() >= 20)
                    {

                        //CreateDiv("divCPU" + i, ObjCPU.Forest, ObjCPU.Machine, ObjCPU.PoolFQDN, ObjCPU.Datapoints,null);
                        list = new DivData { divId = "divADS" + i, Forest = ObjCPU.Forest, Machine = ObjCPU.Machine, Datapoints = ObjCPU.Datapoints, PoolFQDN = ObjCPU.PoolFQDN, PercentageFree = null };
                        GetCPUDataByThreshold(ref list, JsonConvert.DeserializeObject<List<DatapointsTimeWise>>(re), Convert.ToDouble(ConfigurationManager.AppSettings["CPUThresholdLow"]), Convert.ToDouble(ConfigurationManager.AppSettings["CPUThresholdHigh"]), Convert.ToInt16(ConfigurationManager.AppSettings["ADSTimeSpan"]));
                    }
                    else
                    {
                        list = new DivData { divId = "divADS" + i, Forest = ObjCPU.Forest, Machine = ObjCPU.Machine, Datapoints = ObjCPU.Datapoints, PoolFQDN = ObjCPU.PoolFQDN, PercentageFree = null, DivColor = "green" };
                    }
                    divlist.Add(list);
                    i++;
                }
                //CreateDivs(divlist);
            }
            else
            {
                //CreateDiv("divADS" + i, "All Forest", ObjCPU.Machine, ObjCPU.PoolFQDN, ObjCPU.Datapoints, null, "green");
                //divlist.Add(new DivData { divId = "All Forest", Forest = ObjCPU.Forest, Machine = ObjCPU.Machine, Datapoints = ObjCPU.Datapoints, PoolFQDN = ObjCPU.PoolFQDN, PercentageFree = null });
            }
            return divlist;
        }
        [Obsolete]
        public List<DivData> TenantProvisionfailures(MetricReader reader, MetricIdentifier id, double Threshold, double Interval)
        {
            var dimensionFilters = new DimensionFilter[] { "Forest", DimensionFilter.CreateIncludeFilter("Role", "PRV"), "Instance" };

            IEnumerable<IQueryResult> results = reader.GetFilteredDimensionValuesAsync(
            id,
            dimensionFilters,
            DateTime.UtcNow.AddMinutes(-(Interval)),
            DateTime.UtcNow,
            SamplingType.Average,
            Reducer.Max,
            new QueryFilter(Operator.GreaterThanOrEqual, Threshold),
            true,
            new SelectionClause(SelectionType.TopValues, 40, OrderBy.Descending)
            ).Result;

            CPUTimePerMachine ObjCPU = new CPUTimePerMachine();
            int i = 1;
            List<DivData> divlist = new List<DivData>();

            if (results.Count() > 0)
            {
                foreach (var r in results)
                {

                    ObjCPU.Forest = r.DimensionList[0].Value.ToString();
                    ObjCPU.Metrics = "TenantProvisionFailures";
                    ObjCPU.Datapoints = new List<DatapointsTimeWise>();

                    var definition = new TimeSeriesDefinition<MetricIdentifier>(
                    id,
                    new Dictionary<string, string> { { "Forest", ObjCPU.Forest }, { "Role", "PRV" }, { "Instance", "_Total" } });// { "PoolFQDN", "*" }, { "Machine", "*" }, { "Instance", "*" } });

                    TimeSeries<MetricIdentifier, double?> result =
                   reader.GetTimeSeriesAsync(DateTime.UtcNow.AddMinutes(-Interval), DateTime.UtcNow, SamplingType.Average, definition).Result;

                    var re = JsonConvert.SerializeObject(result.Datapoints);

                    var pageOfCPUMetrics = JsonConvert.DeserializeObject<List<DatapointsTimeWise>>(re).Where(val => val.Value >= Threshold);

                    ObjCPU.Datapoints.AddRange(pageOfCPUMetrics);
                    DivData list;
                    if (pageOfCPUMetrics.Count() > 50)
                    {
                        //CreateDivs("divTPF" + i, ObjCPU.Forest, ObjCPU.Machine, ObjCPU.PoolFQDN, ObjCPU.Datapoints, null);
                        list = new DivData { divId = "divTPF" + i, Forest = ObjCPU.Forest, Machine = ObjCPU.Machine, Datapoints = ObjCPU.Datapoints, PoolFQDN = ObjCPU.PoolFQDN, PercentageFree = null };
                        GetCPUDataByThreshold(ref list, JsonConvert.DeserializeObject<List<DatapointsTimeWise>>(re), Convert.ToDouble(ConfigurationManager.AppSettings["TenantThresholdLow"]), Convert.ToDouble(ConfigurationManager.AppSettings["TenantThresholdHigh"]), Convert.ToInt16(ConfigurationManager.AppSettings["TenantTimeSpan"]));
                    }
                    else
                    {
                        list = new DivData { divId = "divTPF" + i, Forest = ObjCPU.Forest, Machine = ObjCPU.Machine, Datapoints = ObjCPU.Datapoints, PoolFQDN = ObjCPU.PoolFQDN, PercentageFree = null, DivColor = "green" };
                    }
                    divlist.Add(list);
                    i++;
                };
                //CreateDivs(divlist);
            }
            //else
            //{
            //    CreateDiv("divTPF" + i, "All Forest", ObjCPU.Machine, ObjCPU.PoolFQDN, ObjCPU.Datapoints, null, "green");
            //    //divlist.Add(new DivData { divId = "All Forest" + i, Forest = ObjCPU.Forest, Machine = ObjCPU.Machine, Datapoints = ObjCPU.Datapoints, PoolFQDN = ObjCPU.PoolFQDN, PercentageFree = null });

            //}
            return divlist;
        }
        [Obsolete]
        public List<DivData> TenantSubProvisionFailures(MetricReader reader, MetricIdentifier id, double Threshold, double Interval)
        {
            var dimensionFilters = new DimensionFilter[] { "Forest", DimensionFilter.CreateIncludeFilter("Instance", "_Total"), DimensionFilter.CreateIncludeFilter("Role", "PRV") };
            List<DivData> divlist = new List<DivData>();
            IEnumerable<IQueryResult> results = reader.GetFilteredDimensionValuesAsync(
            id,
            dimensionFilters,
            DateTime.UtcNow.AddMinutes(-(Interval)),
            DateTime.UtcNow,
            SamplingType.Max,
            Reducer.Max,
            new QueryFilter(Operator.GreaterThanOrEqual, Threshold),
            true,
            new SelectionClause(SelectionType.TopValues, 40, OrderBy.Descending)
            ).Result;

            CPUTimePerMachine ObjCPU = new CPUTimePerMachine();
            int i = 1;

            if (results.Count() > 0)
            {
                foreach (var r in results)
                {
                    ObjCPU.Forest = r.DimensionList[0].Value.ToString();
                    ObjCPU.Metrics = "TenantSubProvisionFailures";
                    ObjCPU.Datapoints = new List<DatapointsTimeWise>();

                    var definition = new TimeSeriesDefinition<MetricIdentifier>(
                    id,
                    new Dictionary<string, string> { { "Forest", ObjCPU.Forest }, { "Role", "PRV" }, { "Instance", "_Total" } });// { "PoolFQDN", "*" }, { "Machine", "*" }, { "Instance", "*" } });

                    TimeSeries<MetricIdentifier, double?> result =
                   reader.GetTimeSeriesAsync(DateTime.UtcNow.AddMinutes(-Interval), DateTime.UtcNow, SamplingType.Average, definition).Result;

                    var re = JsonConvert.SerializeObject(result.Datapoints);

                    var pageOfCPUMetrics = JsonConvert.DeserializeObject<List<DatapointsTimeWise>>(re).Where(val => val.Value >= Threshold);

                    ObjCPU.Datapoints.AddRange(pageOfCPUMetrics);
                    DivData list;
                    if (pageOfCPUMetrics.Count() > 50)
                    {
                        list = new DivData { divId = "divTSPF" + i, Forest = ObjCPU.Forest, Machine = ObjCPU.Machine, Datapoints = ObjCPU.Datapoints, PoolFQDN = ObjCPU.PoolFQDN, PercentageFree = null };
                        GetCPUDataByThreshold(ref list, JsonConvert.DeserializeObject<List<DatapointsTimeWise>>(re), Convert.ToDouble(ConfigurationManager.AppSettings["TenantThresholdLow"]), Convert.ToDouble(ConfigurationManager.AppSettings["TenantThresholdHigh"]), Convert.ToInt16(ConfigurationManager.AppSettings["TenantTimeSpan"]));
                    }
                    else
                    {
                        list = new DivData { divId = "divTSPF" + i, Forest = ObjCPU.Forest, Machine = ObjCPU.Machine, Datapoints = ObjCPU.Datapoints, PoolFQDN = ObjCPU.PoolFQDN, PercentageFree = null, DivColor = "green" };
                    }
                    divlist.Add(list);
                    //CreateDiv("divTSPF" + i, ObjCPU.Forest, ObjCPU.Machine, ObjCPU.PoolFQDN, ObjCPU.Datapoints, null);

                    i++;
                };
                //CreateDivs(divlist);
            }
            //else
            //{
            //    CreateDiv("divTSPF" + i, "All Forest", ObjCPU.Machine, ObjCPU.PoolFQDN, ObjCPU.Datapoints, null, "green");
            //}
            return divlist;
        }
        [Obsolete]
        public List<DivData> TenantUserpublishfailures(MetricReader reader, MetricIdentifier id, double Threshold, double Interval)
        {
            var dimensionFilters = new DimensionFilter[] { "Forest", DimensionFilter.CreateIncludeFilter("Instance", "_Total"), DimensionFilter.CreateIncludeFilter("Role", "PRV") };
            List<DivData> divlist = new List<DivData>();
            IEnumerable<IQueryResult> results = reader.GetFilteredDimensionValuesAsync(
            id,
            dimensionFilters,
            DateTime.UtcNow.AddMinutes(-(Interval)),
            DateTime.UtcNow,
            SamplingType.Max,
            Reducer.Max,
            new QueryFilter(Operator.GreaterThanOrEqual, Threshold),
            true,
            new SelectionClause(SelectionType.TopValues, 40, OrderBy.Descending)
            ).Result;

            CPUTimePerMachine ObjCPU = new CPUTimePerMachine();
            int i = 1;

            if (results.Count() > 0)
            {
                foreach (var r in results)
                {
                    ObjCPU.Forest = r.DimensionList[0].Value.ToString();
                    ObjCPU.Metrics = "TenantUserpublishfailures";
                    ObjCPU.Datapoints = new List<DatapointsTimeWise>();

                    var definition = new TimeSeriesDefinition<MetricIdentifier>(
                    id,
                    new Dictionary<string, string> { { "Forest", ObjCPU.Forest }, { "Role", "PRV" }, { "Instance", "_Total" } });// { "PoolFQDN", "*" }, { "Machine", "*" }, { "Instance", "*" } });

                    TimeSeries<MetricIdentifier, double?> result =
                   reader.GetTimeSeriesAsync(DateTime.UtcNow.AddMinutes(-Interval), DateTime.UtcNow, SamplingType.Average, definition).Result;

                    var re = JsonConvert.SerializeObject(result.Datapoints);

                    var pageOfCPUMetrics = JsonConvert.DeserializeObject<List<DatapointsTimeWise>>(re).Where(val => val.Value >= Threshold);

                    ObjCPU.Datapoints.AddRange(pageOfCPUMetrics);
                    DivData list;
                    if (pageOfCPUMetrics.Count() > 50)
                    {
                        list = new DivData { divId = "divTUPF" + i, Forest = ObjCPU.Forest, Machine = ObjCPU.Machine, Datapoints = ObjCPU.Datapoints, PoolFQDN = ObjCPU.PoolFQDN, PercentageFree = null };
                        GetCPUDataByThreshold(ref list, JsonConvert.DeserializeObject<List<DatapointsTimeWise>>(re), Convert.ToDouble(ConfigurationManager.AppSettings["TenantThresholdLow"]), Convert.ToDouble(ConfigurationManager.AppSettings["TenantThresholdHigh"]), Convert.ToInt16(ConfigurationManager.AppSettings["TenantTimeSpan"]));
                    }
                    else
                    {
                        list = new DivData { divId = "divTUPF" + i, Forest = ObjCPU.Forest, Machine = ObjCPU.Machine, Datapoints = ObjCPU.Datapoints, PoolFQDN = ObjCPU.PoolFQDN, PercentageFree = null, DivColor = "green" };
                    }
                    divlist.Add(list);
                    i++;
                }
                //CreateDivs(divlist);
            }
            //else
            //{
            //    CreateDiv("divTUPF" + i, "All Forest", ObjCPU.Machine, ObjCPU.PoolFQDN, ObjCPU.Datapoints, null, "green");
            //}
            return divlist;
        }
        [Obsolete]
        public List<DivData> UserProvisionFailures(MetricReader reader, MetricIdentifier provId, MetricIdentifier pkMode, MetricIdentifier nPkMode, double Threshold, double Interval)
        {
            List<DivData> divList = new List<DivData>();
            var dimensionFilters = new DimensionFilter[] { "Forest", DimensionFilter.CreateIncludeFilter("Role", "PRV"), DimensionFilter.CreateIncludeFilter("Instance", "_Total") };
            IEnumerable<IQueryResult> results = reader.GetFilteredDimensionValuesAsync(
                provId,
                dimensionFilters,
                DateTime.UtcNow.AddMinutes(-(Interval)),
                DateTime.UtcNow,
                SamplingType.Max,
                Reducer.Max,
                new QueryFilter(Operator.GreaterThanOrEqual, Threshold),
                true,
                new SelectionClause(SelectionType.TopValues, 40, OrderBy.Descending)
                ).Result;

            IEnumerable<IQueryResult> pkResult = reader.GetFilteredDimensionValuesAsync(
                pkMode,
                dimensionFilters,
                DateTime.UtcNow.AddMinutes(-(Interval)),
                DateTime.UtcNow,
                SamplingType.Max,
                Reducer.Max,
                new QueryFilter(Operator.GreaterThanOrEqual, Threshold),
                true,
                new SelectionClause(SelectionType.TopValues, 40, OrderBy.Descending)
                ).Result;

            IEnumerable<IQueryResult> npkResults = reader.GetFilteredDimensionValuesAsync(
                nPkMode,
                dimensionFilters,
                DateTime.UtcNow.AddMinutes(-(Interval)),
                DateTime.UtcNow,
                SamplingType.Max,
                Reducer.Max,
                new QueryFilter(Operator.GreaterThanOrEqual, Threshold),
                true,
                new SelectionClause(SelectionType.TopValues, 40, OrderBy.Descending)
                ).Result;

            CPUTimePerMachine ObjCPU = new CPUTimePerMachine();
            int i = 1;
            if (results.Count() > 0)
            {
                foreach (var item in results)
                {
                    var provSuccCnt = pkResult.FirstOrDefault(a => a.DimensionList[0].Value == item.DimensionList[0].Value);
                    var provFailCnt = npkResults.FirstOrDefault(a => a.DimensionList[0].Value == item.DimensionList[0].Value);
                    DivData list = null;
                    if (provSuccCnt != null || provFailCnt != null)
                    {
                        double? failedResult = (item.EvaluatedResult / (provSuccCnt.EvaluatedResult + provFailCnt.EvaluatedResult)) * 100;
                        if (failedResult >= Convert.ToDouble(ConfigurationManager.AppSettings["UserProvisionEvalValue"]))
                        {
                            list = new DivData { divId = "divUPF" + i++, Forest = item.DimensionList[0].Value, Role = item.DimensionList[2].Value, Result = failedResult, DivColor = "red" };
                        }
                    }
                    else
                    {
                        list = new DivData { divId = "divUPF" + i++, Forest = item.DimensionList[0].Value, Role = item.DimensionList[2].Value, Result = item.EvaluatedResult, DivColor = "red" };
                    }
                    if (list != null)
                    {
                        divList.Add(list);
                        i++;
                    }
                }
            }
            return divList;
        }
        [Obsolete]
        public List<DivData> UserSubProvisionFailures(MetricReader reader, MetricIdentifier id, double Threshold, double Interval)
        {
            var dimensionFilters = new DimensionFilter[] { "Forest", DimensionFilter.CreateIncludeFilter("Instance", "_Total"), DimensionFilter.CreateIncludeFilter("Role", "PRV") };
            List<DivData> divList = new List<DivData>();
            IEnumerable<IQueryResult> results = reader.GetFilteredDimensionValuesAsync(
                id,
                dimensionFilters,
                DateTime.UtcNow.AddMinutes(-(Interval)),
                DateTime.UtcNow,
                SamplingType.Max,
                Reducer.Max,
                new QueryFilter(Operator.GreaterThanOrEqual, Threshold),
                true,
                new SelectionClause(SelectionType.TopValues, 40, OrderBy.Descending)
                ).Result;

            CPUTimePerMachine ObjCPU = new CPUTimePerMachine();
            int i = 1;
            if (results.Count() > 0)
            {
                foreach (var r in results)
                {
                    ObjCPU.Forest = r.DimensionList[0].Value.ToString();
                    ObjCPU.Metrics = "UserSubProvisionFailures";
                    ObjCPU.Datapoints = new List<DatapointsTimeWise>();
                    var definition = new TimeSeriesDefinition<MetricIdentifier>(
                    id,
                    new Dictionary<string, string> { { "Forest", ObjCPU.Forest }, { "Role", "PRV" }, { "Instance", "_Total" } });// { "PoolFQDN", "*" }, { "Machine", "*" }, { "Instance", "*" } });
                    TimeSeries<MetricIdentifier, double?> result =
                        reader.GetTimeSeriesAsync(DateTime.UtcNow.AddMinutes(-Interval), DateTime.UtcNow, SamplingType.Average, definition).Result;

                    var re = JsonConvert.SerializeObject(result.Datapoints);
                    var thresholdWiseDatapoints = JsonConvert.DeserializeObject<List<DatapointsTimeWise>>(re).Where(val => val.Value >= Threshold);

                    ObjCPU.Datapoints.AddRange(thresholdWiseDatapoints);
                    DivData list;
                    if (thresholdWiseDatapoints.Count() >= 100)// Interval 60 / 360 // Orange/ Red/ Green
                    {
                        //continous any where 6more than 50 occurence0 came or not by using 
                        list = new DivData { divId = "divUSPF" + i, Forest = ObjCPU.Forest, Machine = ObjCPU.Machine, Datapoints = ObjCPU.Datapoints, PoolFQDN = ObjCPU.PoolFQDN, PercentageFree = null };
                        GetCPUDataByThreshold(ref list, JsonConvert.DeserializeObject<List<DatapointsTimeWise>>(re), Convert.ToDouble(ConfigurationManager.AppSettings["UserSubProvisionThresholdLow"]), Convert.ToDouble(ConfigurationManager.AppSettings["UserSubProvisionThresholdHigh"]), Convert.ToInt16(ConfigurationManager.AppSettings["UserSubProvisionTimeSpan"]));
                    }
                    else // Green
                    {
                        list = new DivData { divId = "divUSPF" + i, Forest = ObjCPU.Forest, Machine = ObjCPU.Machine, Datapoints = ObjCPU.Datapoints, PoolFQDN = ObjCPU.PoolFQDN, PercentageFree = null, DivColor = "green" };
                    }
                    //CreateDiv("divUSPF" + i, ObjCPU.Forest, ObjCPU.Machine, ObjCPU.PoolFQDN, ObjCPU.Datapoints, null,"green");
                    divList.Add(list);
                    i++;
                }
                // CreateDivs(divList);
            }
            //else
            //{
            //    CreateDiv("divUSPF" + i, "All Forest", ObjCPU.Machine, ObjCPU.PoolFQDN, ObjCPU.Datapoints, null, "green");
            //}
            return divList;
        }
        [Obsolete]
        public List<DivData> ReadNumberofMNCUsersFailed(MetricReader reader, MetricIdentifier Fid, MetricIdentifier Successid, double Threshold, double Interval)
        {
            var dimensionFilters = new DimensionFilter[] { "Forest", DimensionFilter.CreateIncludeFilter("Role", "PRV"), DimensionFilter.CreateIncludeFilter("Instance", "_Total") };
            List<DivData> divList = new List<DivData>();
            IEnumerable<IQueryResult> resultsSucc = reader.GetFilteredDimensionValuesAsync(
           Successid,
           dimensionFilters,
           DateTime.UtcNow.AddMinutes(-(Interval)),
           DateTime.UtcNow,
           SamplingType.Max,
           Reducer.Max,
           new QueryFilter(Operator.GreaterThanOrEqual, Threshold),
           false,
           new SelectionClause(SelectionType.TopValues, 40, OrderBy.Descending)
           ).Result;

            IEnumerable<IQueryResult> results = reader.GetFilteredDimensionValuesAsync(
            Fid,
            dimensionFilters,
            DateTime.UtcNow.AddMinutes(-(Interval)),
            DateTime.UtcNow,
            SamplingType.Max,
            Reducer.Max,
            new QueryFilter(Operator.GreaterThanOrEqual, Threshold),
            false,
            new SelectionClause(SelectionType.TopValues, 40, OrderBy.Descending)
            ).Result;


            if (resultsSucc.Count() > 0 && results.Count() > 0)
            {
                int count = 0;
                // if Move Failure / (Move Success + Move Failure) *100
                foreach (var item in resultsSucc)
                {
                    int itemCnt = 0;
                    DivData data = null;
                    foreach (var fail in results)
                    {
                        if (item.DimensionList[0].Value == fail.DimensionList[0].Value)
                        {
                            var failedResult = (fail.EvaluatedResult / (fail.EvaluatedResult + item.EvaluatedResult)) * 100;
                            data = new DivData { divId = "divMnsUsr" + itemCnt++, Forest = item.DimensionList[0].Value, Role = item.DimensionList[2].Value, Result = failedResult };
                            if (failedResult > Convert.ToInt32(ConfigurationManager.AppSettings["MNCThresholdLow"]) && failedResult < Convert.ToInt32(ConfigurationManager.AppSettings["MNCThresholdHigh"]))
                            {
                                data.DivColor = "darkorange";
                                //orange
                                break;
                            }
                            else if (failedResult >= Convert.ToInt32(ConfigurationManager.AppSettings["MNCThresholdHigh"]))
                            {
                                data.DivColor = "red";
                                //red
                                break;
                            }
                            count++;
                            if (count == results.Count())
                            {
                                data = new DivData { divId = "divMnsUsr" + itemCnt++, Forest = item.DimensionList[0].Value, Role = item.DimensionList[2].Value, Result = failedResult, DivColor = "red" };
                            }
                        }
                    }

                    if (data != null && data.DivColor != null)
                    {
                        divList.Add(data);
                    }
                }
            }
            return divList;
        }
        [Obsolete]
        public List<DivData> ProvisioningQueue(MetricReader reader, MetricIdentifier id, double Threshold, double Interval)
        {
            List<DivData> divList = new List<DivData>();
            var dimensionFilters = new DimensionFilter[] { "Forest", DimensionFilter.CreateIncludeFilter("Role", "PRV"), DimensionFilter.CreateIncludeFilter("Instance", "_Total") };
            IEnumerable<IQueryResult> results = reader.GetFilteredDimensionValuesAsync(
                id,
                dimensionFilters,
                DateTime.UtcNow.AddMinutes(-(Interval)),
                DateTime.UtcNow,
                SamplingType.Average,
                Reducer.Average,
                new QueryFilter(Operator.GreaterThanOrEqual, 0),
                false,
                new SelectionClause(SelectionType.TopValues, 40, OrderBy.Descending)
                ).Result;

            CPUTimePerMachine ObjCPU = new CPUTimePerMachine();
            int i = 0;
            if (results.Count() > 0)
            {
                var list = results.Where(a => a.EvaluatedResult == Threshold).ToList();
                if (list.Count() > 0)
                {
                    foreach (var item in list)
                    {
                        divList.Add(new DivData { divId = "divPQ" + i++, DivColor = "red", Forest = item.DimensionList[0].Value, Result = item.EvaluatedResult, Role = item.DimensionList[2].Value });
                    }
                }
            }
            return divList;
        }
        [Obsolete]
        public List<DivData> SubProvisioningQueue(MetricReader reader, MetricIdentifier id, double Threshold, double Interval)
        {
            List<DivData> divList = new List<DivData>();
            var dimensionFilters = new DimensionFilter[] { "Forest", DimensionFilter.CreateIncludeFilter("Role", "PRV"), DimensionFilter.CreateIncludeFilter("Instance", "_Total") };
            IEnumerable<IQueryResult> results = reader.GetFilteredDimensionValuesAsync(
                id,
                dimensionFilters,
                DateTime.UtcNow.AddMinutes(-(Interval)),
                DateTime.UtcNow,
                SamplingType.Average,
                Reducer.Average,
                new QueryFilter(Operator.GreaterThanOrEqual, 0),
                false,
                new SelectionClause(SelectionType.TopValues, 40, OrderBy.Descending)
                ).Result;

            CPUTimePerMachine ObjCPU = new CPUTimePerMachine();
            int i = 0;
            if (results.Count() > 0)
            {
                var list = results.Where(a => a.EvaluatedResult == Threshold && (a.DimensionList[0].Equals("Forest4E") || a.DimensionList[0].Equals("Forest5A"))).ToList();
                if (list.Count() > 0)
                {
                    foreach (var item in list)
                    {
                        divList.Add(new DivData { divId = "divSPQ" + i++, DivColor = "red", Forest = item.DimensionList[0].Value, Result = item.EvaluatedResult, Role = item.DimensionList[2].Value });
                    }
                }
            }
            return divList;
        }
        [Obsolete]
        public List<DivData> PublishingQueue(MetricReader reader, MetricIdentifier id, double Threshold, double Interval)
        {
            List<DivData> divList = new List<DivData>();
            var dimensionFilters = new DimensionFilter[] { "Forest", DimensionFilter.CreateIncludeFilter("Role", "PRV"), DimensionFilter.CreateIncludeFilter("Instance", "_Total") };
            IEnumerable<IQueryResult> results = reader.GetFilteredDimensionValuesAsync(
                id,
                dimensionFilters,
                DateTime.UtcNow.AddMinutes(-(Interval)),
                DateTime.UtcNow,
                SamplingType.Average,
                Reducer.Average,
                new QueryFilter(Operator.GreaterThanOrEqual, 0),
                false,
                new SelectionClause(SelectionType.TopValues, 40, OrderBy.Descending)
                ).Result;
            CPUTimePerMachine ObjCPU = new CPUTimePerMachine();
            int i = 1;
            if (results.Count() > 0)
            {
                var list = results.Where(a => a.EvaluatedResult == Threshold).ToList();
                if (list.Count() > 0)
                {
                    foreach (var item in list)
                    {
                        divList.Add(new DivData { divId = "divPubQ" + i++, DivColor = "red", Forest = item.DimensionList[0].Value, Result = item.EvaluatedResult, Role = item.DimensionList[2].Value });
                    }
                }
            }
            return divList;
        }

        [Obsolete]
        public List<MachineStatus> ReadCPUStatusData(MetricReader reader, MetricIdentifier cpuid, double threshold, double interval)
        {
            List<MachineStatus> dataList = new List<MachineStatus>(), list = new List<MachineStatus>();
            try
            {
                var dimensionFilters = new DimensionFilter[] { DimensionFilter.CreateIncludeFilter("Environment", "Prod"), "Forest", "Machine", "PoolFQDN", DimensionFilter.CreateIncludeFilter("Role", "PRV"), DimensionFilter.CreateIncludeFilter("ServiceName", "RtcProv"), "ServiceStatus" };
                IEnumerable<IQueryResult> results = reader.GetFilteredDimensionValuesAsync(
                    cpuid,
                    dimensionFilters,
                    DateTime.UtcNow.AddSeconds(-(interval)),
                    DateTime.UtcNow,
                    SamplingType.Average,
                    Reducer.Average,
                    new QueryFilter(Operator.GreaterThanOrEqual, threshold),
                    true,
                    new SelectionClause(SelectionType.TopValues, interval <= 60 ? 200 : 500, OrderBy.Descending)
                    ).Result;

                //var data = results.Where(a => !a.DimensionList[6].Value.Equals("Running")).ToList();
                if (results.Count() > 0)
                {
                    foreach (var item in results)
                    {
                        var definition = new TimeSeriesDefinition<MetricIdentifier>(
                        cpuid,
                        new Dictionary<string, string> { { "Environment", "Prod" }, { "Forest", item.DimensionList[1].Value }, 
                            { "Machine", item.DimensionList[2].Value }, { "PoolFQDN", item.DimensionList[3].Value }, 
                            { "Role", "PRV" }, { "ServiceName", "RtcProv" }, { "ServiceStatus", item.DimensionList[6].Value } });
                        TimeSeries<MetricIdentifier, double?> result =
                            reader.GetTimeSeriesAsync(DateTime.UtcNow.AddSeconds(-interval), DateTime.UtcNow, SamplingType.Average, definition).Result;
                        //var re = JsonConvert.SerializeObject(result.Datapoints);
                        var dataPts = result.Datapoints.ToList();
                        dataList.Add(new MachineStatus
                        {
                            Environment = item.DimensionList[0].Value,
                            ForestName = item.DimensionList[1].Value,
                            Machine = item.DimensionList[2].Value,
                            PoolFqdn = item.DimensionList[3].Value,
                            Status = item.DimensionList[6].Value,
                            Role = item.DimensionList[4].Value,
                            ServiceName = item.DimensionList[5].Value,
                            EvaluatedResult = Convert.ToDouble(item.EvaluatedResult),
                            LastUpdatedDate = Convert.ToDateTime(dataPts.OrderByDescending(a => a.TimestampUtc).First().TimestampUtc),
                            LastUpdatedValue = Convert.ToDouble(dataPts.OrderByDescending(a => a.Value).First().Value)
                        }); 
                    }
                }
                if (dataList.Count()>1)
                {
                    var forestGrp = dataList.GroupBy(a => a.ForestName).ToList();
                    foreach (var item in forestGrp)
                    {
                        var data = dataList.Where(a => a.ForestName == item.Key).ToList();
                        if(data.Count() == 1)
                        {
                            list.Add(data.First());
                        }
                        else if (data.Count() > 1)
                        {
                            GetDataByMachine(data, ref list);
                        }
                    }

                }
                else
                {
                    list = dataList;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return list;
        }

        private void GetDataByMachine(List<MachineStatus> data, ref List<MachineStatus> list)
        {
            try
            {
                var machineGrp = data.GroupBy(a => a.Machine).ToList();
                foreach (var item in machineGrp)
                {
                    list.Add(data.Where(a => a.Machine == item.Key).OrderByDescending(k => k.LastUpdatedDate).First());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
