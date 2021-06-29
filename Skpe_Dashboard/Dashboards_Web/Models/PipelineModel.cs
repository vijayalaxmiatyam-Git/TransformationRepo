using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboards_Web.Models
{
    public class PipelineModel
    {
        public int BuildId { get; set; }
        public string BuildNumber { get; set; }
        public string status { get; set; }
        public string result { get; set; }
        public DateTime finishTime { get; set; }
        public string url { get; set; }
        //public Definition definition { get; set; }
        public int BuildNumberRevision { get; set; }
        //public Project project { get; set; }
        public string uri { get; set; }
        public string sourceBranch { get; set; }
        public string sourceVersion { get; set; }
        //public Queue queue { get; set; }
        public string priority { get; set; }
        public string reason { get; set; }
        public string requestedFor { get; set; }
        public string requestedBy { get; set; }
        public DateTime LastChangedDate { get; set; }
        public string LastChangedBy { get; set; }
        public string parameters { get; set; }
        //public OrchestrationPlan orchestrationPlan { get; set; }
        public string logs { get; set; }
        //public Repository repository { get; set; }
        public bool KeepForever { get; set; }
        public bool RetainedByRelease { get; set; }
        public object TriggeredByBuild { get; set; }
        public string PipelineName { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public int DefinitionId { get; set; }
        public string DefinitionName { get; set; }
        public string ServiceName { get; set; }
        public string CrntDu { get; set; }
        public string Environment { get; set; }
        
    }
}