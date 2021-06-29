using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dashboards_Web.Models
{
    public class TTADataModel
    {
        public int IncidentNo { get; set; }
        public string Title { get; set; }
        public string OwningContactAlias { get; set; }
        public DateTime CreateDate { get; set; }
        public object TTA { get; set; }
        public string IsCustomer { get; set; }
        public object TicketAge { get; set; }
        public string OwningTeam { get; set; }
        public string Status { get; set; }
        public int Severity { get; set; }
        public string TTACategory { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string IsCarryForward { get; set; }

        public string IncidentType { get; set; }
        public DateTime? MitigatedDate { get; set; }
        public string ChangedBy { get; set; }
        public string Mitigation { get; set; }
        public double Efforts { get; set; }
        public string IcmComment { get; set; }
        public string IncidentRelation { get; set; }
        public List<SelectListItem> ShiftStatus { get; set; }
        public string TTAMet { get; set; }
    }
}