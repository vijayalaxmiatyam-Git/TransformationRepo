using System;
using System.Collections.Generic;
using System.Text;

namespace Dashboards_BusinessLayer.Models
{
    public class MachineStatus
    {
        public string ForestName { get; set; }
        public string Machine { get; set; }
        public string PoolFqdn { get; set; }
        public string Environment { get; set; }
        public string Status { get; set; }
        public string Role { get; set; }
        public string ServiceName { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public double LastUpdatedValue { get; set; }
        public double EvaluatedResult { get; set; }

    }
}
