using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboards_BusinessLayer
{
    public class ICM_Incidents
    {
        public class IcmIncident
        {
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
            public List<CustomFields> CustomFieldGroups { get; set; }
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
            public string ParentIncidentId { get; set; }

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
        public class AcknowledgementData
        {

            public string IsAcknowledged { get; set; }
            
            public string AcknowledgeDate { get; set; }
           
            public string AcknowledgeContactAlias { get; set; }
           
            public string NotificationId { get; set; }
            public string Duration { get; set; }
        }
        public class MitigationResolutionData
        {
            public string Date { get; set; }
            public string ChangedBy { get; set; }
            public string Mitigation { get; set; }

        }
        public class ResolutionData
        {
            public string Date { get; set; }
            //public string ChangedBy { get; set; }
        }

        public class IcmPayload
        {
            public List<CustomFieldGroup> CustomFieldGroups { get; set; }
        }

        public class CustomFieldGroup
        {
            public string PublicId { get; set; }
            public string ContainerId { get; set; }
            public string GroupType { get; set; }
            public List<CustomFields> CustomFields { get; set; }
        }

        public class CustomFields
        {
            public string Name { get; set; }
            public string DisplayName { get; set; }
            public string Value { get; set; }
            public string Type { get; set; }
        }

        public class CustomFieldValue
        {
            public string IcMName { get; set; }
            public string IcMDisplayName { get; set; }
            public string Source { get; set; }
            public string Template { get; set; }
            public string Field { get; set; }
            public string Value { get; set; }
            public string Type { get; set; }
        }

        public class UpdateMitigatedResolvedDate
        {
            public MitigationResolutionData MitigationData { get; set; }
            public MitigationResolutionData ResolutionData { get; set; }
        }

        public class Outages
        {
            public OutageDeatils detail { get; set; }
            public ImpactDetails impact { get; set; }
            public List<ImpactTracker> impactTrackers { get; set; }
        }

        public class OutageDeatils
        {
            public string outageId { get; set; }

            public string icmIncidentId { get; set; }

            public string currentStatus { get; set; }

            public Symptoms symptoms { get; set; }
        }

        public class Symptoms
        {
            public List<ImpactTracker> impactedServices { get; set; }
        }

        public class ImpactDetails
        {
            public string impactSummary { get; set; }
        }

        public class ImpactTracker
        {
            public string serviceId { get; set; }
            public string serviceName { get; set; }
        }

        public class test
        {
            public string id { get; set; }
            public string manager { get; set; }
            public string[] teamNames { get; set; }
        }

        public class Tenant
        {
            public string Id { get; set; }
            public string PublicId { get; set; }
            public string TenantGuid { get; set; }
            public string Name { get; set; }
            public string Desciption { get; set; }
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

        //public class Root
        //{
        //    public List<CustomFieldGroupsItem> CustomFieldGroups { get; set; }
        //}

    }
}
