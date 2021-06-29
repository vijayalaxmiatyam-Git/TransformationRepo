using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboards_Web.Models
{
    public class IcmDataModel
    {
        public List<TTADataModel> TtaList { get; set; }
        public List<TTEDataModel> TteList { get; set; }
        public string Page { get; set; }
        public Int32 PageSize { get; set; }
        public String tteSort { get; set; }
        public String tteSortDir { get; set; }
        public String ttaSort { get; set; }
        public String ttaSortDir { get; set; }
        public Int32 TotalRecords { get; set; }
        public bool IsIc3Prov { get; set; }
        public bool IsBusinessBVD { get; set; }
        public bool IsRtcAppSre { get; set; }
        public bool IsIc3RunTime { get; set; }
        public bool IsGpa { get; set; }
        public bool IsIC3TenantPolicies { get; set; }
        public bool IsTeamsLROS { get; set; }
        public bool IsIC3AdminControls { get; set; }
        public bool ServiceDelivery { get; set; }
        public bool SFBUserMoves { get; set; }
        public bool SFBMindtreeDeliveryTeam { get; set; }
        public bool SFBMindtreeSD { get; set; }
        public bool SkypeMindtreeDelivery { get; set; }
        public bool SBVVa { get; set; }
        public bool BVOrRouting { get; set; }
        public bool BVLis { get; set; }
        public string IcmType { get; set; }
        public string Shift { get; set; }
        public string ShiftInfo { get; set; }
        public List<Team> TeamList { get; set; }
        public bool AllTrackSre { get; set; }
        public bool AllTrackSd { get; set; }
        public bool AllTrackCvm { get; set; }
        public string HandOffEmail { get; set; }
    }
}