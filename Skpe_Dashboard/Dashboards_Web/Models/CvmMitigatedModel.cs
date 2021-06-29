using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboards_Web.Models
{
    public class CvmMitigatedModel
    {
        public string IncidentId { get; set; }
        public int Severity { get; set; }
        public string Format { get; set; }
        public string OwningTeam { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public DateTime MitigatedDate { get; set; }
        public string Age { get; set; }
        public string Owner { get; set; }
        public object TTR { get; set; }
        public string MitigatedBy { get; set; }
        public string Mitigation { get; set; }
        public string TtrCategory { get; set; }
    }
}