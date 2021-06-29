using System;
using System.Collections.Generic;
using System.Text;
using static Dashboards_BusinessLayer.ICM_Incidents;

namespace Dashboards_BusinessLayer.Models
{
    public class TestIcm
    {
        public class Source
        {
            public string SourceId { get; set; }
            public string Origin { get; set; }
            public string CreatedBy { get; set; }
            public string CreateDate { get; set; }
            public string IncidentId { get; set; }
            public string ModifiedDate { get; set; }
            public string Revision { get; set; }
        }

        public class RaisingLocation
        {
            public string Environment { get; set; }
            public string DataCenter { get; set; }
            public string DeviceGroup { get; set; }
            public string DeviceName { get; set; }
            public string ServiceInstanceId { get; set; }
        }

        public class IncidentLocation
        {
            public string Environment { get; set; }
            public string DataCenter { get; set; }
            public string DeviceGroup { get; set; }
            public string DeviceName { get; set; }
            public string ServiceInstanceId { get; set; }
        }

        public class MitigationData
        {
            public string Date { get; set; }
            public string ChangedBy { get; set; }
            public string Mitigation { get; set; }
        }

        public class AcknowledgementData
        {
            public string IsAcknowledged { get; set; }
            public string AcknowledgeDate { get; set; }
            public string AcknowledgeContactAlias { get; set; }
            public string NotificationId { get; set; }
        }

        public class CustomFieldsItem
        {
            public string Name { get; set; }
            public string DisplayName { get; set; }
            public string Value { get; set; }
            public string Type { get; set; }
        }

        public class CustomFieldGroupsItem
        {
            public string PublicId { get; set; }
            public string ContainerId { get; set; }
            public string GroupType { get; set; }
            public List<CustomFieldsItem> CustomFields { get; set; }
        }

        public class Root
        {
            //public string odata.metadata { get; set; }
            public string Id { get; set; }
            public string Title { get; set; }
            public string ImpactStartDate { get; set; }
            public AcknowledgementData AcknowledgementData { get; set; }
            public int Severity { get; set; }
            public string CreateDate { get; set; }
            public string ModifiedDate { get; set; }
            public string OwningContactAlias { get; set; }
            public string[] ImpactedServicesIds { get; set; }
            public string ImpactedServicesNames { get; set; }
            public string Status { get; set; }
            public MitigationResolutionData MitigationData { get; set; }
            public MitigationResolutionData ResolutionData { get; set; }
            //public List<CustomFieldGroup> CustomFieldGroups { get; set; }
            public Outages Outage { get; set; }
            public string lastCommDate { get; set; }
            public string lastCommsStatus { get; set; }
            public string IncidentManager { get; set; }
            public string EngagedTeams { get; set; }
            public string initialSeverity { get; set; }
            public string BridgeConfID { get; set; }
            public string BridgePhoneNumber { get; set; }
            public string BridgeUri { get; set; }
            public string TTA { get; set; }
            public string TTACategory { get; set; }
            public string Customer { get; set; }
            public string AcknowledgeDate { get; set; }
            public string Owner { get; set; }
            public string Duration { get; set; }
            public RaisingLocation RaisingLocation { get; set; }
            public string OwningTeamId { get; set; }
            public string IncidentType { get; set; }
            public List<CustomFieldGroupsItem> CustomFieldGroups { get; set; }
            public Source Source { get; set; }
            public string CorrelationId { get; set; }
            public string RoutingId { get; set; }
            public IncidentLocation IncidentLocation { get; set; }
            public string ParentIncidentId { get; set; }
            public string RelatedLinksCount { get; set; }
            public string ExternalLinksCount { get; set; }
            public string LastCorrelationDate { get; set; }
            public string HitCount { get; set; }
            public string ChildCount { get; set; }
            public string ReproSteps { get; set; }
            public string OwningTenantId { get; set; }
            public string IsCustomerImpacting { get; set; }
            public string IsNoise { get; set; }
            public string IsSecurityRisk { get; set; }
            public string TsgId { get; set; }
            public string CustomerName { get; set; }
            public string CommitDate { get; set; }
            public string Keywords { get; set; }
            public string Component { get; set; }
            public string OriginatingTenantId { get; set; }
            public string SubscriptionId { get; set; }
            public string SupportTicketId { get; set; }
            public string MonitorId { get; set; }
            public string IncidentSubType { get; set; }
            public string HowFixed { get; set; }
            public string TsgOutput { get; set; }
            public string SourceOrigin { get; set; }
            public string ResponsibleTenantId { get; set; }
            public string ResponsibleTeamId { get; set; }
            public List<string> ImpactedTeamsPublicIds { get; set; }
            public List<string> ImpactedComponents { get; set; }
            public string NewDescriptionEntry { get; set; }
            public string ReactivationData { get; set; }
            public List<string> ExternalIncidents { get; set; }
            public string SiloId { get; set; }
            public string IncidentManagerContactId { get; set; }
            public string ExecutiveIncidentManagerContactId { get; set; }
            public string CommunicationsManagerContactId { get; set; }
            public string SiteReliabilityContactId { get; set; }
            public string HealthResourceId { get; set; }
            public string DiagnosticsLink { get; set; }
            public string ChangeList { get; set; }
            public string IsOutage { get; set; }
            public string OutageImpactLevel { get; set; }
            public string Summary { get; set; }
            public List<string> Tags { get; set; }
        }
    }
}
