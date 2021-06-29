using Dashboards_BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Dashboards_BusinessLayer.Interfaces
{
    public interface ITaskHandler
    {
        DataTable ConvertCSVtoDataTable(string strFilePath);
        DataTable ConvertXSLXtoDataTable(string strFilePath, string connString);
        string GetMachines(string description);
        bool AddVsoData(List<TaskData> data);

        List<TaskData> GetVsoData();
    }
}
