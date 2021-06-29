using System;
using System.Collections.Generic;
using System.Text;

namespace Dashboards_BusinessLayer.Models
{
    public class TestResult
    {
        public int TestId { get; set; }
        public string Result { get; set; }
        public int TestPointId { get; set; }
        public int TestPlanId { get; set; }
        public int TestSuiteId { get; set; }

        public bool IsProcessed { get; set; }

    }
}
