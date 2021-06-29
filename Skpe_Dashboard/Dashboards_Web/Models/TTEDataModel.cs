using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboards_Web.Models
{
    public class TTEDataModel
    {
        public int IncidentNo { get; set; }
        public string Title { get; set; }
        public string OwningContactAlias { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime AcknowledgeDate { get; set; }
        public object TTE { get; set; }
        public string IsCustomer { get; set; }
        public object TicketAge { get; set; }
        public string OwningTeam { get; set; }
        public string Status { get; set; }
        public int Severity { get; set; }
        public string TTECategory { get; set; }

    }
}