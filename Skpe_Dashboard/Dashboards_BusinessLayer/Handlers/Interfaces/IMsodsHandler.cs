using Dashboards_BusinessLayer.Models;
using Microsoft.Cloud.Metrics.Client.Metrics;
using Microsoft.Online.Metrics.Serialization.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dashboards_BusinessLayer.Handlers.Interfaces
{
    public interface IMsodsHandler
    {
        List<MSODSData> ReadMSODSMetricData(MetricReader reader, MetricIdentifier id, double threshold, double Interval, string metricName);
        
    }
}
