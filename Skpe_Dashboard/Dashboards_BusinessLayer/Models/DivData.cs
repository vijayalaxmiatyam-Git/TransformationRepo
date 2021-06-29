using System;
using System.Collections.Generic;
using System.Text;

namespace Dashboards_BusinessLayer.Models
{
   public class DivData
    {
        public string divId { get; set; }
        public string Forest { get; set; }
        public string Machine { get; set; }
        public string PoolFQDN { get; set; }
#pragma warning disable CS0436 // Type conflicts with imported type
        public List<DatapointsTimeWise> Datapoints { get; set; }
#pragma warning restore CS0436 // Type conflicts with imported type
        public double? PercentageFree { get; set; }
        public string DivColor { get; set; }
        public string Role { get; set; }
        public double? Result { get; set; }
    }
}
