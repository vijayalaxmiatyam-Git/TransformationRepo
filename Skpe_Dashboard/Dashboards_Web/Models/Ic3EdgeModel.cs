using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dashboards_Web.Models
{
    public class Ic3EdgeModel
    {
        [Required(ErrorMessage ="Enter test plan id")]
        public int TestPlanId { get; set; }
        [Required(ErrorMessage = "Enter test suite id")]
        public int TestSuiteId { get; set; }
        public List<TestCaseModel> TestCaseData { get; set; }
        public List<TestCaseModel> TestDbData { get; set; }
        public List<PipelineModel> Ic3BuildData { get; set; }
        public List<ReleaseModel> Ic3ReleaseData { get; set; }
        public List<ReleaseModel> Ic3ReleaseDefData { get; set; }
    }
}