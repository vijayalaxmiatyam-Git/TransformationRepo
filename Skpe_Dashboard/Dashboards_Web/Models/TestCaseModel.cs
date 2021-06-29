using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboards_Web.Models
{
    public class TestCaseModel
    {
        public int TestId { get; set; }
        public string Result { get; set; }
        public int TestPointId { get; set; }
        public string TesterName { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public DateTime LastResetToActive { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }
        public bool IsProcessed { get; set; }
    }
}