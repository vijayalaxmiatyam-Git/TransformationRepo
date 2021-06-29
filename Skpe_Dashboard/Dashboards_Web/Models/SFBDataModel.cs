using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dashboards_Web.Models
{
    public class SFBDataModel
    {
        public string FriendlyName { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Category { get; set; }
        public string Track { get; set; }
        public string ModalTrack { get; set; }
        public bool IsFrequentlyUsed { get; set; }
        public string FrequentlyUsed { get; set; }
        public string LinkType { get; set; }
        public string ImagePath { get; set; }

        public Int64 SfbLibId { get; set; }
    }
    public class DDLOptions
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }

        public string ImgPath { get; set; }

    }
}