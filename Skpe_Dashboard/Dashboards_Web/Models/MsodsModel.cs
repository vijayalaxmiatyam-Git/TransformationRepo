using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboards_Web.Models
{
    public class MsodsModel
    {
        public string ServiceInstance { get; set; }
        public string ServiceInstanceShortName { get; set; }
        public string ServiceType { get; set; }
        public string SyncStreamIdentifier { get; set; }
        public double EvaluatedResult { get; set; }
        public string Color { get; set; }
        public string divId { get; set; }
        public string URL { get; set; }
        public List<Datapoints> DatapointsList { get; set; }

    }
    public class Datapoints
    {
        public DateTime? TimestampUtc { get; set; }
        public double? Value { get; set; }
    }
}