using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dashboards_Web.Models
{
    public class StatusModel
    {
        public List<MachineStatusModel> Machines { get; set; }
        public Int32 Page { get; set; }
        public Int32 PageSize { get; set; }
        public String Sort { get; set; }
        public String SortDir { get; set; }
        public Int32 TotalRecords { get; set; }

        [Display(Name = "Forest Name")]
        public String ForestName { get; set; }

        [Display(Name = "Machine")]
        public string Machine { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Service Name")]
        public string ServiceName { get; set; }

        public StatusModel()
        {
            Page = 1;
            PageSize = 50;
            Sort = "Status";
            SortDir = "DESC";
        }
    }
}