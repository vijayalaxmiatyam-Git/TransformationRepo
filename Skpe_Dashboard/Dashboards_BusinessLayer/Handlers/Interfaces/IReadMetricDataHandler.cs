using Dashboards_BusinessLayer.Models;
using Microsoft.Cloud.Metrics.Client.Metrics;
using Microsoft.Online.Metrics.Serialization.Configuration;
using System.Collections.Generic;

namespace Dashboards_BusinessLayer
{
    public interface IReadMetricDataHandler
    {
        List<DivData> ReadCPUMetricsData(MetricReader reader, MetricIdentifier id, double CPUThreshold, double Interval);
        void GetCPUDataByThreshold(ref DivData obj, IEnumerable<DatapointsTimeWise> datapointsTimeWise, double lowThrshld, double highThrshld, int timeSpan);
        List<DivData> ReadMemoryMetricsData(MetricReader reader, double MemoryThreshold, double Interval);
        void GetMemoryThresholdData(ref DivData data, string trshldLow, string trshldHigh);
        List<DivData> ReadADSaveLatency(MetricReader reader, MetricIdentifier id, double Threshold, double Interval);
        List<DivData> TenantProvisionfailures(MetricReader reader, MetricIdentifier id, double Threshold, double Interval);
        List<DivData> TenantSubProvisionFailures(MetricReader reader, MetricIdentifier id, double Threshold, double Interval);
        List<DivData> TenantUserpublishfailures(MetricReader reader, MetricIdentifier id, double Threshold, double Interval);
        List<DivData> UserSubProvisionFailures(MetricReader reader, MetricIdentifier id, double Threshold, double Interval);
        List<DivData> UserProvisionFailures(MetricReader reader, MetricIdentifier provId, MetricIdentifier pkMode, MetricIdentifier nPkMode, double Threshold, double Interval);
        List<DivData> ReadNumberofMNCUsersFailed(MetricReader reader, MetricIdentifier Fid, MetricIdentifier Successid, double Threshold, double Interval);
        List<DivData> ProvisioningQueue(MetricReader reader, MetricIdentifier id, double Threshold, double Interval);
        List<DivData> SubProvisioningQueue(MetricReader reader, MetricIdentifier id, double Threshold, double Interval);
        List<DivData> PublishingQueue(MetricReader reader, MetricIdentifier id, double Threshold, double Interval);
        List<MachineStatus> ReadCPUStatusData(MetricReader reader, MetricIdentifier cpuid, double threshold, double interval);
    }
}
