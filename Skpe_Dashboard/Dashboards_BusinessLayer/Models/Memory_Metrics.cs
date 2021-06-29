using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboards_BusinessLayer
{
    public class Memory_Metrics
    {
      
            public string Metrics { get; set; } //CPU time per machine (percentage used)
            public string Forest { get; set; } // ForestName

            public string Machine { get; set; } // MachineName

            public string PoolFQDN { get; set; } // PoolFQDN Name
            public double TotalPhysicalMBytes { get; set; }

        public double MemoryAvailableMBytes { get; set; }

        public double MemoryAvailableMByteAVG { get; set; }
        public double PercentageFree { get; set; }
        public List<DatapointsTimeWise> Datapoints { get; set; }

     
        public class DatapointsTimeWise
        {

            /// <summary>
            /// 
            /// </summary>
            public DateTime? TimestampUtc { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? Value { get; set; }
        }

        //public class MemDetails
        //{

        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string Key { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string value { get; set; }
        //}
    }
}