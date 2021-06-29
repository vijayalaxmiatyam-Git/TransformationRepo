using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboards_BusinessLayer
{
    public class Incident
    {

        public class Source
        {

            /// <summary>
            /// 
            /// </summary>
            public string SourceId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Origin { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string CreatedBy { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string CreateDate { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string IncidentId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ModifiedDate { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Revision { get; set; }
        }

        public class RaisingLocation
        {
            public string IsCustomer { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Environment { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string DataCenter { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string DeviceGroup { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string DeviceName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ServiceInstanceId { get; set; }
        }

        public class IncidentLocation
        {

            /// <summary>
            /// 
            /// </summary>
            public string Environment { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string DataCenter { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string DeviceGroup { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string DeviceName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ServiceInstanceId { get; set; }
        }

        public class AcknowledgementData
        {

            /// <summary>
            /// 
            /// </summary>
            public string IsAcknowledged { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string AcknowledgeDate { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string AcknowledgeContactAlias { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string NotificationId { get; set; }
            public string Duration { get; set; }
        }

        public class ValueItem
        {
            public string TTACategory { get; set; }
            public string TTA { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int Severity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Status { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string CreateDate { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ModifiedDate { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Source Source { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string CorrelationId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string RoutingId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public RaisingLocation RaisingLocation { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public IncidentLocation IncidentLocation { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ParentIncidentId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string RelatedLinksCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ExternalLinksCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string LastCorrelationDate { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string HitCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ChildCount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Title { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ReproSteps { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string OwningContactAlias { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string OwningTenantId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string OwningTeamId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string MitigationData { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ResolutionData { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string IsCustomerImpacting { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string IsNoise { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string IsSecurityRisk { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string TsgId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string CustomerName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string CommitDate { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Keywords { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Component { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string IncidentType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ImpactStartDate { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string OriginatingTenantId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string SubscriptionId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string SupportTicketId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string MonitorId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string IncidentSubType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string HowFixed { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string TsgOutput { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string SourceOrigin { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ResponsibleTenantId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ResponsibleTeamId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> ImpactedServicesIds { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> ImpactedTeamsPublicIds { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> ImpactedComponents { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string NewDescriptionEntry { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public AcknowledgementData AcknowledgementData { get; set; }
            public string IsAcknowledged { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string ReactivationData { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> CustomFieldGroups { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> ExternalIncidents { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string SiloId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string IncidentManagerContactId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ExecutiveIncidentManagerContactId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string CommunicationsManagerContactId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string SiteReliabilityContactId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string HealthResourceId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string DiagnosticsLink { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ChangeList { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string IsOutage { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string OutageImpactLevel { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Summary { get; set; }
        }

        public class Root
        {

            /// <summary>
            /// 
            /// </summary>
            //public string odata.metadata { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<ValueItem> value { get; set; }

            //public List<Result> result { get; set; }
        }


    }
}