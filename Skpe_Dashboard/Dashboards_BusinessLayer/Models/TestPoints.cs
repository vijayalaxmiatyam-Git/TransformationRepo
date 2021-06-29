using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Dashboards_BusinessLayer.Models
{
    public class TestPoints
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
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

        public class Configuration
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

        public class TestPlan
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public class TestSuite
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public class LastUpdatedBy
        {
            public string displayName { get; set; }
            public string url { get; set; }
            public Links _links { get; set; }
            public string id { get; set; }
            public string uniqueName { get; set; }
            public string imageUrl { get; set; }
            public string descriptor { get; set; }
        }

        public class RunBy
        {
            public string displayName { get; set; }
            public string id { get; set; }
        }

        public class LastResultDetails
        {
            public int duration { get; set; }
            public DateTime dateCompleted { get; set; }
            public RunBy runBy { get; set; }
        }

        public class Results
        {
            public LastResultDetails lastResultDetails { get; set; }
            public int lastResultId { get; set; }
            public string lastRunBuildNumber { get; set; }
            public string state { get; set; }
            public string lastResultState { get; set; }
            public string outcome { get; set; }
            public int lastTestRunId { get; set; }
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

        public class TestCases
        {
            public string href { get; set; }
        }

        public class Run
        {
            public string href { get; set; }
        }

        public class Result
        {
            public string href { get; set; }
        }

        public class Links3
        {
            public Self _self { get; set; }
            public SourcePlan sourcePlan { get; set; }
            public SourceSuite sourceSuite { get; set; }
            public SourceProject sourceProject { get; set; }
            public TestCases testCases { get; set; }
            public Run run { get; set; }
            public Result result { get; set; }
        }

        public class TestCaseReference
        {
            public int id { get; set; }
            public string name { get; set; }
            public string state { get; set; }
        }

        public class Value
        {
            public int id { get; set; }
            public Tester tester { get; set; }
            public Configuration configuration { get; set; }
            public bool isAutomated { get; set; }
            public Project project { get; set; }
            public TestPlan testPlan { get; set; }
            public TestSuite testSuite { get; set; }
            public LastUpdatedBy lastUpdatedBy { get; set; }
            public DateTime lastUpdatedDate { get; set; }
            public Results results { get; set; }
            public DateTime lastResetToActive { get; set; }
            public bool isActive { get; set; }
            public Links links { get; set; }
            public TestCaseReference testCaseReference { get; set; }
        }

        public class Root
        {
            public List<Value> value { get; set; }
            public int count { get; set; }
        }


    }
}