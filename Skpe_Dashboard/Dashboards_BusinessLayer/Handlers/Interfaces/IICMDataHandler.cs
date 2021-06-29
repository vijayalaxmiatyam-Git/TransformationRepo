using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Dashboards_BusinessLayer
{
    public interface IICMDataHandler
    {
        string GetIncident(string thumbprint, string sessionValue);
        DataTable ExecuteKustoQuery(string query);
        DataTable ExecuteKustoQueryForTest(string query);
        string GetDuration(TimeSpan dur);
        List<ICM_Incidents.IcmIncident> GetTTARefinedData(List<ICM_Incidents.IcmIncident> incidentsList, string track = "");
        List<ICM_Incidents.IcmIncident> GetTTERefinedData(List<ICM_Incidents.IcmIncident> incidentsList, string track = "");
        List<ICM_Incidents.IcmIncident> GetRefinedHistoryData(DataTable data,string team);
        void GetTTAForCVM(ref List<ICM_Incidents.IcmIncident> incidentList);
        string GetShiftData(string ddlShift, ref string shiftInfo);
        List<ICM_Incidents.IcmIncident> GetTTAForSOC(List<ICM_Incidents.IcmIncident> incidentsList);
        List<ICM_Incidents.IcmIncident> GetTTEForSOC(List<ICM_Incidents.IcmIncident> incidentsList);
        List<ICM_Incidents.IcmIncident> GetTTADataForCvm(List<ICM_Incidents.IcmIncident> incidentsList, string track = "");
        List<ICM_Incidents.IcmIncident> GetTTEForCvm(List<ICM_Incidents.IcmIncident> incidentsList, string track = "");
        List<ICM_Incidents.IcmIncident> GetTTEForSd(List<ICM_Incidents.IcmIncident> incidentsList, string track = "");
    }
}
