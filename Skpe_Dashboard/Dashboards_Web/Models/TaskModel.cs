using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboards_Web.Models
{
    public class TaskModel
    {
        public Int32 Id { get; set; }
        public string Title { get; set; }
        public string CreatedDate { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
        public string Machines { get; set; }
        public int MachineCount { get; set; }
    }
}