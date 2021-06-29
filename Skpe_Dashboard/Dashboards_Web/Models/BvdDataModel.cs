using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboards_Web.Models
{
    public class BvdDataModel
    {
        public List<PipelineModel> PipelineModelData { get; set; }
        public List<ReleaseModel> ReleaseModelData { get; set; }
    }
}