using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
namespace SnowJson.BusinessObject
{
    public class TestCases
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class TestPlan
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public class Project
        {
            public string id { get; set; }
            public string name { get; set; }
            public string state { get; set; }
            public string visibility { get; set; }
            public DateTime lastUpdateTime { get; set; }
        }

        public class TestSuite
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public class WorkItemField
        {
            [JsonProperty("Microsoft.VSTS.TCM.Steps")]
            public string MicrosoftVSTSTCMSteps { get; set; }

            [JsonProperty("Microsoft.VSTS.Common.ActivatedBy")]
            public string MicrosoftVSTSCommonActivatedBy { get; set; }

            [JsonProperty("Microsoft.VSTS.Common.ActivatedDate")]
            public DateTime? MicrosoftVSTSCommonActivatedDate { get; set; }

            [JsonProperty("Microsoft.VSTS.TCM.AutomationStatus")]
            public string MicrosoftVSTSTCMAutomationStatus { get; set; }

            [JsonProperty("System.Description")]
            public string SystemDescription { get; set; }

            [JsonProperty("System.State")]
            public string SystemState { get; set; }

            [JsonProperty("System.AssignedTo")]
            public string SystemAssignedTo { get; set; }

            [JsonProperty("Microsoft.VSTS.Common.Priority")]
            public int? MicrosoftVSTSCommonPriority { get; set; }

            [JsonProperty("Microsoft.VSTS.Common.StateChangeDate")]
            public DateTime? MicrosoftVSTSCommonStateChangeDate { get; set; }

            [JsonProperty("System.WorkItemType")]
            public string SystemWorkItemType { get; set; }

            [JsonProperty("System.Rev")]
            public int? SystemRev { get; set; }
        }

        public class WorkItem
        {
            public int id { get; set; }
            public string name { get; set; }
            public List<WorkItemField> workItemFields { get; set; }
        }

        public class Avatar
        {
            public string href { get; set; }
        }

        public class Links
        {
            public Avatar avatar { get; set; }
        }

        public class Tester
        {
            public string displayName { get; set; }
            public string url { get; set; }
            public Links _links { get; set; }
            public string id { get; set; }
            public string uniqueName { get; set; }
            public string imageUrl { get; set; }
            public string descriptor { get; set; }
        }

        public class PointAssignment
        {
            public int id { get; set; }
            public string configurationName { get; set; }
            public Tester tester { get; set; }
            public int configurationId { get; set; }
        }

        public class TestPoints
        {
            public string href { get; set; }
        }

        public class Configuration
        {
            public string href { get; set; }
        }

        public class Self
        {
            public string href { get; set; }
        }

        public class SourcePlan
        {
            public string href { get; set; }
        }

        public class SourceSuite
        {
            public string href { get; set; }
        }

        public class SourceProject
        {
            public string href { get; set; }
        }

        public class Links2
        {
            public TestPoints testPoints { get; set; }
            public Configuration configuration { get; set; }
            public Self _self { get; set; }
            public SourcePlan sourcePlan { get; set; }
            public SourceSuite sourceSuite { get; set; }
            public SourceProject sourceProject { get; set; }
        }

        public class Value
        {
            public TestPlan testPlan { get; set; }
            public Project project { get; set; }
            public TestSuite testSuite { get; set; }
            public WorkItem workItem { get; set; }
            public List<PointAssignment> pointAssignments { get; set; }
            public Links links { get; set; }
            public int? order { get; set; }
        }

        public class Root
        {
            public List<Value> value { get; set; }
            public int count { get; set; }
        }


    }
}