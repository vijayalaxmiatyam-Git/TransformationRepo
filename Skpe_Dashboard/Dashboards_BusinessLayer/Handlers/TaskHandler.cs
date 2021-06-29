using Dashboards_BusinessLayer.Interfaces;
using Dashboards_BusinessLayer.Models;
using Dashboards_DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Dashboards_BusinessLayer.Handlers
{
    public class TaskHandler:ITaskHandler
    {
        TaskDAL dalObj = new TaskDAL();
        public TaskHandler()
        {

        }
        public DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            try
            {
                return dalObj.ConvertCSVtoDataTable(strFilePath);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable ConvertXSLXtoDataTable(string strFilePath, string connString)
        {
            try
            {
                return dalObj.ConvertXSLXtoDataTable(strFilePath, connString);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public string GetMachines(string description)
        {
            string newStr = string.Empty;
            try
            {
                var data = description.Split(' ').Where(a => a.ToUpper().Contains("FAIL") || a.ToUpper().Contains("TIMEOUT") || a.ToUpper().Contains("PASS")).ToList();
                foreach (var item in data)
                {
                    var refVal = System.Text.RegularExpressions.Regex.Replace(item, @"[\s]", "");
                    if (refVal.ToUpper().Contains("FAIL"))
                    {
                        refVal = refVal.ToUpper().Replace("FAIL", " ").Split(' ')[0];
                    }
                    if (refVal.ToUpper().Contains("PASS"))
                    {
                        refVal = refVal.ToUpper().Replace("PASS", " ").Split(' ')[0];
                    }
                    if (refVal.ToUpper().Contains("TIMEOUT"))
                    {
                        refVal = refVal.ToUpper().Replace("TIMEOUT", " ").Split(' ')[0];
                    }
                    if(refVal.Contains(":"))
                    {
                        refVal = refVal.Split(':')[refVal.Split(':').ToArray().Count()-1];
                        refVal = Regex.Replace(refVal, @"[0-9]{2}[S]", "");
                    }
                    if(refVal.Contains("-"))
                    {
                        refVal = refVal.Split('-')[1];
                    }
                    
                    if (!string.IsNullOrEmpty(refVal) && Regex.IsMatch(refVal, @"[a-zA-Z0-9]") && refVal.ToCharArray().Count()<=13)
                    {
                        newStr += string.IsNullOrEmpty(newStr) ? refVal : "|" + refVal;
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return newStr;
        }

        public bool AddVsoData(List<TaskData> data)
        {
            try
            {
                foreach (var item in data)
                {
                    DateTime dt = GetUtcTimeFrmStrng(item.CreatedDate);
                    dalObj.InsertVSOsData(item.Id, item.Title, dt, item.Description, item.State, item.Machines, item.Machines.Split('|').Count());
                    //DataTable tableData = CreateDataTable(data);

                }
                //return dalObj.ExecuteImagingProcedure(tableData);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private DataTable CreateDataTable(List<TaskData> data)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Title", typeof(string));
            table.Columns.Add("Created Date", typeof(DateTime));
            table.Columns.Add("Description", typeof(string));
            table.Columns.Add("State", typeof(string));
            table.Columns.Add("Machines", typeof(string));
            table.Columns.Add("Machine Count", typeof(int));
            foreach (var item in data)
            {
                table.Rows.Add(item.Id, item.Title, GetUtcTimeFrmStrng(item.CreatedDate), item.Description, item.State, item.Machines, item.Machines.Split('|').Count());
            }
            return table;
        }
        public List<TaskData> GetVsoData()
        {
            List<TaskData> tasks = new List<TaskData>();
            try
            {
                DataTable dt = dalObj.GetVsoData();

                tasks = (from DataRow d in dt.Rows
                         select new TaskData
                         {
                             Id = Convert.ToInt32(d["Id"]),
                             CreatedDate = d["CreatedDate"].ToString(),
                             Machines = d["Machines"].ToString(),
                             Title = d["Title"].ToString(),
                             MachineCount = Convert.ToInt32(d["Machinecount"])
                         }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
            return tasks;
        }
        private DateTime GetUtcTimeFrmStrng(string time)
        {
            try
            {
                time = Regex.Replace(time, "[a-zA-z]", " ").Split('.')[0].Trim();
                DateTime dtUtc = DateTime.ParseExact(time, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                return DateTime.SpecifyKind(dtUtc, DateTimeKind.Utc);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
