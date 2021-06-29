using System;
using System.Collections.Generic;
using System.Text;

namespace Dashboards_BusinessLayer.Models
{
    public class ReleaseDetail
    {
        public class Avatar
        {
            public string href { get; set; }
        }

        public class Links
        {
            public Avatar avatar { get; set; }
            public Self self { get; set; }
            public Web web { get; set; }
        }

        public class ModifiedBy
        {
            public string displayName { get; set; }
            public string url { get; set; }
            public Links _links { get; set; }
            public string id { get; set; }
            public string uniqueName { get; set; }
            public string imageUrl { get; set; }
            public string descriptor { get; set; }
        }

        public class CreatedBy
        {
            public string displayName { get; set; }
            public string url { get; set; }
            public Links _links { get; set; }
            public string id { get; set; }
            public string uniqueName { get; set; }
            public string imageUrl { get; set; }
            public string descriptor { get; set; }
        }

        public class CreatedFor
        {
            public string displayName { get; set; }
            public string url { get; set; }
            public Links _links { get; set; }
            public string id { get; set; }
            public string uniqueName { get; set; }
            public string imageUrl { get; set; }
            public string descriptor { get; set; }
        }

        public class Variables
        {
        }

        public class Self
        {
            public string href { get; set; }
        }

        public class Web
        {
            public string href { get; set; }
        }

        public class ReleaseDefinition
        {
            public int id { get; set; }
            public string name { get; set; }
            public string path { get; set; }
            public object projectReference { get; set; }
            public string url { get; set; }
            public Links _links { get; set; }
        }

        public class ProjectReference
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class Properties
        {
        }

        public class Value
        {
            public int id { get; set; }
            public string name { get; set; }
            public string status { get; set; }
            public DateTime createdOn { get; set; }
            public DateTime modifiedOn { get; set; }
            public ModifiedBy modifiedBy { get; set; }
            public CreatedBy createdBy { get; set; }
            public CreatedFor createdFor { get; set; }
            public Variables variables { get; set; }
            public List<object> variableGroups { get; set; }
            public ReleaseDefinition releaseDefinition { get; set; }
            public int releaseDefinitionRevision { get; set; }
            public string description { get; set; }
            public string reason { get; set; }
            public string releaseNameFormat { get; set; }
            public bool keepForever { get; set; }
            public int definitionSnapshotRevision { get; set; }
            public string logsContainerUrl { get; set; }
            public string url { get; set; }
            public Links _links { get; set; }
            public List<object> tags { get; set; }
            public object triggeringArtifactAlias { get; set; }
            public ProjectReference projectReference { get; set; }
            public Properties properties { get; set; }
        }

        public class Root
        {
            public int count { get; set; }
            public List<Value> value { get; set; }
        }
    }
}
