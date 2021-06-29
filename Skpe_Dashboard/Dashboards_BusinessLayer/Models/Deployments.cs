using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dashboards_BusinessLayer.Models
{
    public class Deployments
    {
        public class ArtifactSourceDefinitionUrl
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class Branches
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class BuildUri
        {
            public string id { get; set; }
            public object name { get; set; }
        }

        public class Definition
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class IsMultiDefinitionType
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class IsXamlBuildArtifactType
        {
            public string id { get; set; }
            public object name { get; set; }
        }

        public class Project
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class RepositoryProvider
        {
            public string id { get; set; }
            public object name { get; set; }
        }

        public class Repository
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class RequestedFor
        {
            public string id { get; set; }
            public object name { get; set; }
            public string displayName { get; set; }
            public string url { get; set; }
            public Links _links { get; set; }
            public string uniqueName { get; set; }
            public string imageUrl { get; set; }
            public string descriptor { get; set; }
        }

        public class RequestedForId
        {
            public string id { get; set; }
            public object name { get; set; }
        }

        public class SourceVersion
        {
            public string id { get; set; }
            public object name { get; set; }
        }

        public class Version
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class ArtifactSourceVersionUrl
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class Branch
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class DefinitionReference
        {
            public ArtifactSourceDefinitionUrl artifactSourceDefinitionUrl { get; set; }
            public Branches branches { get; set; }
            public BuildUri buildUri { get; set; }
            public Definition definition { get; set; }
            public IsMultiDefinitionType IsMultiDefinitionType { get; set; }
            public IsXamlBuildArtifactType IsXamlBuildArtifactType { get; set; }
            public Project project { get; set; }

            [JsonProperty("repository.provider")]
            public RepositoryProvider RepositoryProvider { get; set; }
            public Repository repository { get; set; }
            public RequestedFor requestedFor { get; set; }
            public RequestedForId requestedForId { get; set; }
            public SourceVersion sourceVersion { get; set; }
            public Version version { get; set; }
            public ArtifactSourceVersionUrl artifactSourceVersionUrl { get; set; }
            public Branch branch { get; set; }
        }

        public class Artifact
        {
            public string sourceId { get; set; }
            public string type { get; set; }
            public string alias { get; set; }
            public DefinitionReference definitionReference { get; set; }
            public bool isPrimary { get; set; }
            public bool isRetained { get; set; }
        }

        public class Self
        {
            public string href { get; set; }
        }

        public class Web
        {
            public string href { get; set; }
        }

        public class Links
        {
            public Self self { get; set; }
            public Web web { get; set; }
            public Avatar avatar { get; set; }
        }

        public class Release
        {
            public int id { get; set; }
            public string name { get; set; }
            public string url { get; set; }
            public List<Artifact> artifacts { get; set; }
            public string webAccessUri { get; set; }
            public Links _links { get; set; }
        }

        public class ProjectReference
        {
            public string id { get; set; }
            public object name { get; set; }
        }

        public class ReleaseDefinition
        {
            public int id { get; set; }
            public string name { get; set; }
            public string path { get; set; }
            public ProjectReference projectReference { get; set; }
            public string url { get; set; }
            public Links _links { get; set; }
        }

        public class ReleaseEnvironment
        {
            public int id { get; set; }
            public string name { get; set; }
            public string url { get; set; }
            public Links _links { get; set; }
        }

        public class Avatar
        {
            public string href { get; set; }
        }

        public class RequestedBy
        {
            public string displayName { get; set; }
            public string url { get; set; }
            public Links _links { get; set; }
            public string id { get; set; }
            public string uniqueName { get; set; }
            public string imageUrl { get; set; }
            public string descriptor { get; set; }
        }

        public class LastModifiedBy
        {
            public string displayName { get; set; }
            public string id { get; set; }
            public string uniqueName { get; set; }
            public string descriptor { get; set; }
        }

        public class PreDeployApproval
        {
            public int id { get; set; }
            public int revision { get; set; }
            public string approvalType { get; set; }
            public DateTime createdOn { get; set; }
            public DateTime modifiedOn { get; set; }
            public string status { get; set; }
            public string comments { get; set; }
            public bool isAutomated { get; set; }
            public bool isNotificationOn { get; set; }
            public int trialNumber { get; set; }
            public int attempt { get; set; }
            public int rank { get; set; }
            public Release release { get; set; }
            public ReleaseDefinition releaseDefinition { get; set; }
            public ReleaseEnvironment releaseEnvironment { get; set; }
            public string url { get; set; }
        }

        public class PostDeployApproval
        {
            public int id { get; set; }
            public int revision { get; set; }
            public string approvalType { get; set; }
            public DateTime createdOn { get; set; }
            public DateTime modifiedOn { get; set; }
            public string status { get; set; }
            public string comments { get; set; }
            public bool isAutomated { get; set; }
            public bool isNotificationOn { get; set; }
            public int trialNumber { get; set; }
            public int attempt { get; set; }
            public int rank { get; set; }
            public Release release { get; set; }
            public ReleaseDefinition releaseDefinition { get; set; }
            public ReleaseEnvironment releaseEnvironment { get; set; }
            public string url { get; set; }
        }

        public class Value
        {
            public int id { get; set; }
            public Release release { get; set; }
            public ReleaseDefinition releaseDefinition { get; set; }
            public ReleaseEnvironment releaseEnvironment { get; set; }
            public object projectReference { get; set; }
            public int definitionEnvironmentId { get; set; }
            public int attempt { get; set; }
            public string reason { get; set; }
            public string deploymentStatus { get; set; }
            public string operationStatus { get; set; }
            public RequestedBy requestedBy { get; set; }
            public RequestedFor requestedFor { get; set; }
            public DateTime queuedOn { get; set; }
            public DateTime startedOn { get; set; }
            public DateTime completedOn { get; set; }
            public DateTime lastModifiedOn { get; set; }
            public LastModifiedBy lastModifiedBy { get; set; }
            public List<object> conditions { get; set; }
            public List<PreDeployApproval> preDeployApprovals { get; set; }
            public List<PostDeployApproval> postDeployApprovals { get; set; }
            public Links _links { get; set; }
        }

        public class Root
        {
            public int count { get; set; }
            public List<Value> value { get; set; }
        }
    }
}
