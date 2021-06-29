using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboards_Web.Models
{
    public class ReleaseModel
    {
        public int Id { get; set; }
        public object ProjectReference { get; set; }
        public int ReleaseDefId { get; set; }
        public int Attempt { get; set; }
        public string Reason { get; set; }
        public string DeploymentStatus { get; set; }
        public string OperationStatus { get; set; }
        public string RequestedBy { get; set; }
        public DateTime QueuedOn { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime CompletedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public List<object> conditions { get; set; }
        public int ReleaseId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string BuilDefdId { get; set; }
        public string BuildefName { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}