using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboards_Web.Models
{
    public class SocDataModel
    {
        public bool SOCTriage { get; set; }
        public bool SOCWBWW { get; set; }
        public bool SOCDedicated { get; set; }
        public bool SOCCorp { get; set; }
        public bool SOCMultiTenant { get; set; }
        public bool SOCItar { get; set; }
        public bool AllTrackSoc { get; set; }
        public List<Team> TeamList { get; set; }
        public List<TTADataModel> TtaList { get; set; }
        public List<TTEDataModel> TteList { get; set; }
        public string Page { get; set; }
        public Int32 PageSize { get; set; }
        public String tteSort { get; set; }
        public String tteSortDir { get; set; }
        public String ttaSort { get; set; }
        public String ttaSortDir { get; set; }
        public Int32 TotalRecords { get; set; }
        public string Shift { get; set; }
        public string ShiftInfo { get; set; }
        public string HandOffEmail { get; set; }
        public string Type { get; set; }
    }
}