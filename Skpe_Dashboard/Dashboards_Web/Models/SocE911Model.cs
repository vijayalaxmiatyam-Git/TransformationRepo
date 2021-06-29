using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboards_Web.Models
{
    public class SocE911Model
    {
        public string IncidentId { get; set; }
        public int Severity { get; set; }
        public string OwningTeam { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Owner { get; set; }
        public string Comments { get; set; }
        public DateTime LastModifiedOn { get; set; }
    }
}