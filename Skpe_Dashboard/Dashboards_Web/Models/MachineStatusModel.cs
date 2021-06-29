using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboards_Web.Models
{
    public class MachineStatusModel
    {
        public string ForestName { get; set; }
        public string Machine { get; set; }
        public string PoolFqdn { get; set; }
        public string Environment { get; set; }
        public string Status { get; set; }
        public string Role { get; set; }
        public string ServiceName { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public Double LastUpdatedValue { get; set; }
        public double EvaluatedResult { get; set; }
    }
}