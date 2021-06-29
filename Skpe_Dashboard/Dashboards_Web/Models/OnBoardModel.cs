using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboards_Web.Models
{
    public class OnBoardModel
    {
        public string ServiceName { get; set; }
        public string ServiceOwner { get; set; }
        public bool IncludeTask { get; set; }
        public string TaskName { get; set; }
        public string SubTasks { get; set; }
        public string TargetDate { get; set; }
        public string TaskOwner { get; set; }
    }
    public class OnBoardVSOModel
    {
        public string TaskID { get; set; }
        public string Title { get; set; }
        public string Track { get; set; }
        public string CreatedOn { get; set; }
        public string Owner { get; set; }
    }
}