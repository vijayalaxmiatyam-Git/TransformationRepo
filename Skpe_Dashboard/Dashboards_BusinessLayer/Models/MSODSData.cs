using System;
using System.Collections.Generic;
using System.Text;

namespace Dashboards_BusinessLayer.Models
{
    public class MSODSData
    {
        //public string ServiceDescriptorSuffix { get; set; }
        public string ServiceInstance { get; set; }
        public string ServiceType { get; set; }
        public string SyncStreamIdentifier { get; set; }
        public double EvaluatedResult { get; set; }
        public string Color { get; set; }
        public string divId { get; set; }
        public List<Datapoints> Datapoints { get; set; }

    }
    public class Datapoints
    {
        public DateTime? TimestampUtc { get; set; }
        public double? Value { get; set; }
    }
}
