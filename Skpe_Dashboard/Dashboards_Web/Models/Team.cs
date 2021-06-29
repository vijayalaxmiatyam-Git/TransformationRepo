using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboards_Web.Models
{
    public class Team
    {
        public string TeamName { get; set; }
        public string TeamVal { get; set; }
        public string FriendlyName { get; set; }
        public int Sev1ActiveCount { get; set; }
        public int Sev2ActiveCount { get; set; }
        public int Sev3ActiveCount { get; set; }
        public int Sev4ActiveCount { get; set; }
        public int Sev1MitigatedCount { get; set; }
        public int Sev2MitigatedCount { get; set; }
        public int Sev3MitigatedCount { get; set; }
        public int Sev4MitigatedCount { get; set; }
        public int Sev1ResolvedCount { get; set; }
        public int Sev2ResolvedCount { get; set; }
        public int Sev3ResolvedCount { get; set; }
        public int Sev4ResolvedCount { get; set; }
        public int Sev1CFActiveCount { get; set; }
        public int Sev2CFActiveCount { get; set; }
        public int Sev3CFActiveCount { get; set; }
        public int Sev4CFActiveCount { get; set; }
        public int ActiveTotal { get; set; }
        public int MitigatedTotal { get; set; }
        public int ResolvedTotal { get; set; }
        public int CFActiveTotal { get; set; }
        public int TeamTotal { get; set; }
        public int Sev1Total { get; set; }
        public int Sev2Total { get; set; }
        public int Sev3Total { get; set; }
        public int Sev4Total { get; set; }
    }
}