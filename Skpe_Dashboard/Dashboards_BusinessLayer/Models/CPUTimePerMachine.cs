using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboards_BusinessLayer
{
    public class CPUTimePerMachine
    {
        public string Metrics { get; set; } //CPU time per machine (percentage used)
        public string Forest { get; set; } // ForestName
        
        public string Machine { get; set; } // MachineName
     
        public string PoolFQDN { get; set; } // PoolFQDN Name

        //public string TimeStamp { get; set; } // On What time hiegest value came

        //public decimal maxvalue { get; set; } // Max percentage

        public List<DatapointsTimeWise> Datapoints { get; set; }

    }
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
}
