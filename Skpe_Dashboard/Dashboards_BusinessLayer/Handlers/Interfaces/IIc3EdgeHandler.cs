using Dashboards_BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Dashboards_BusinessLayer.Handlers.Interfaces
{
    public interface IIc3EdgeHandler
    {
        bool UpdateTestCaseDetais(DataTable dt, List<TestPoints.Value> rootVal);
        List<TestPoints.Value> GetTestCaseDetais(int testPlnId, int suitId);
        DataTable GetTestcasesFromDB(int suitId);
        List<Pipeline.Value> GetIc3EdgeBuildData();
        List<ReleaseDetail.Value> GetReleasesByBuild(int bldDefId);
        List<Deployments.Value> GetReleaseData(List<ReleaseDetail.Value> refinedList);
    }
}
