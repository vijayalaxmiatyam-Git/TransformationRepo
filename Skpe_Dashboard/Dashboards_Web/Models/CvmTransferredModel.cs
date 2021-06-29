using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboards_Web.Models
{
    public class CvmTransferredModel
    {
        public string IncidentId { get; set; }
        public string OwningTeam { get; set; }
        public DateTime TransferredDate { get; set; }
        public string TransferComments { get; set; }
    }
}