using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboards_Web.Models
{
    public class TicketHistoryModel
    {
        public int IncidentId { get; set; }
        public DateTime CreateDate { get; set; }
        public int Severity { get; set; }
        public string Format { get; set; }
        public string OwningTeam { get; set; }
        public DateTime ChangeDate { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        
    }
}