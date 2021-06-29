using Dashboards_BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dashboards_BusinessLayer.Handlers.Interfaces
{
    public interface IBvdHandler
    {
        List<Pipeline.Value> GetBVDPipelines();
        List<ReleaseDetail.Value> GetReleasesByBuild(int bldDefId);
        List<Deployments.Value> GetReleaseData(List<ReleaseDetail.Value> refinedList);
    }
}
