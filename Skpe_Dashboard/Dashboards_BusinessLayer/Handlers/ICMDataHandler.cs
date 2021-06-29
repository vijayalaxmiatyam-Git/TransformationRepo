using Dashboards_DataLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Web;
using static Dashboards_BusinessLayer.ICM_Incidents;

namespace Dashboards_BusinessLayer
{
    public class ICMDataHandler : IICMDataHandler
    {
        IcmDAL dalObj = new IcmDAL();
        public ICMDataHandler()
        {

        }
        public string GetIncident(string filter, string thumbprint)
        {

            try
            {
                string jsonContent = null;
                HttpWebResponse result = dalObj.GetIncident(filter, thumbprint);

                if (result == null)
                {
                    throw new Exception("HTTP response error.");
                }
                // read out the response stream as text
                using (Stream data = result.GetResponseStream())
                {
                    if (data != null)
                    {
                        TextReader tr = new StreamReader(data);
                        jsonContent = tr.ReadToEnd();
                    }
                }
                if (string.IsNullOrWhiteSpace(jsonContent))
                {
                    throw new InvalidOperationException("IcM API returned no response");
                }
                return jsonContent;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable ExecuteKustoQuery(string query)
        {
            return dalObj.ExecuteKustoQuery(query);
        }

        public DataTable ExecuteKustoQueryForTest(string query)
        {
            return dalObj.ExecuteKustoQueryForTest(query);
        }
        public List<ICM_Incidents.IcmIncident> GetTTADataForCvm(List<ICM_Incidents.IcmIncident> incidentsList, string track = "")
        {
            List<ICM_Incidents.IcmIncident> ttaList = new List<ICM_Incidents.IcmIncident>();
            var ttaLst = incidentsList.Where(a => a.AcknowledgementData.IsAcknowledged == "false" && !string.IsNullOrEmpty(a.CreateDate)).ToList();
            foreach (var item in ttaLst)
            {

                var dur = DateTime.UtcNow.Subtract(Convert.ToDateTime(item.CreateDate));
                Double time = 0;
                time = Convert.ToDouble(ConfigurationManager.AppSettings["SLA_CVMTTA"]);
                if (item.OwningTeamId.ToUpper().Contains("BUSINESSVOICEBVD") && !string.IsNullOrEmpty(item.RaisingLocation.DeviceGroup))
                {
                    if (item.RaisingLocation.DeviceGroup.ToUpper().Equals("IMP"))
                    {
                        //item.TTA = GetValueHrsOrDays(dur, 0);
                        item.TTACategory = "BVD";
                        item.Customer = "YES";
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (item.OwningTeamId.ToUpper().Contains("BUSINESSVOICEBVD") && string.IsNullOrEmpty(item.RaisingLocation.DeviceGroup))
                {
                    if (!string.IsNullOrEmpty(item.RaisingLocation.DeviceName) && item.RaisingLocation.DeviceName.ToUpper().Equals("VIEWPOINT"))
                    {
                        item.TTACategory = "BVD";
                        item.Customer = "YES";
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    item.TTA = GetValueHrsOrDays(dur, time);
                    item.TTACategory = GetTTACategory(dur.TotalMinutes, time);
                }

                item.Duration = GetDuration(dur);
                ttaList.Add(item);
            }
            return ttaList;
        }
        public List<ICM_Incidents.IcmIncident> GetTTEForCvm(List<ICM_Incidents.IcmIncident> incidentsList, string track = "")
        {
            List<ICM_Incidents.IcmIncident> tteList = new List<ICM_Incidents.IcmIncident>();
            foreach (var item in incidentsList)
            {
                Double time = 0;
                time = Convert.ToDouble(ConfigurationManager.AppSettings["SLA_CVMTTR"]);

                if (item.AcknowledgementData.IsAcknowledged == "true" && !string.IsNullOrEmpty(item.AcknowledgeDate) && !item.OwningTeamId.ToUpper().Contains("BUSINESSVOICEBVD"))
                {
                    var dur = DateTime.UtcNow.Subtract(Convert.ToDateTime(item.AcknowledgeDate));
                    item.TTA = GetValueHrsOrDays(dur, time);
                    item.TTACategory = GetTTACategory(dur.TotalMinutes, time);
                    item.Duration = GetDuration(dur);
                    tteList.Add(item);
                }

                else if (item.AcknowledgementData.IsAcknowledged == "true" && !string.IsNullOrEmpty(item.AcknowledgementData.AcknowledgeDate) &&
                   item.OwningTeamId.ToUpper().Contains("BUSINESSVOICEBVD"))
                {
                    if ((!string.IsNullOrEmpty(item.RaisingLocation.DeviceGroup) && item.RaisingLocation.DeviceGroup.ToUpper().Equals("IMP"))
                        || (string.IsNullOrEmpty(item.RaisingLocation.DeviceGroup) && !string.IsNullOrEmpty(item.RaisingLocation.DeviceName) && item.RaisingLocation.DeviceName.ToUpper().Equals("VIEWPOINT")))
                    {
                        var utcDate = GetUtcTimeFrmStrng(item.AcknowledgementData.AcknowledgeDate);
                        var dur = DateTime.UtcNow.Subtract(utcDate);
                        item.Customer = "YES";
                        item.TTACategory = "BVD";
                        item.AcknowledgeDate = utcDate.ToString();
                        item.Duration = GetDuration(dur);
                        tteList.Add(item);
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (item.AcknowledgementData.IsAcknowledged == "true" && string.IsNullOrEmpty(item.AcknowledgeDate) &&
                    !string.IsNullOrEmpty(item.AcknowledgementData.AcknowledgeDate))
                {
                    var utcDate = GetUtcTimeFrmStrng(item.AcknowledgementData.AcknowledgeDate);
                    item.AcknowledgeDate = utcDate.ToString();
                    var dur = DateTime.UtcNow.Subtract(utcDate);
                    item.TTACategory = GetTTACategory(dur.TotalMinutes, time);
                    item.Duration = GetDuration(dur);
                    item.Customer = item.Title.Contains("Online") ? "YES" : "NO";
                    item.TTA = GetValueHrsOrDays(dur, time);
                    tteList.Add(item);
                }
            }
            return tteList;
        }
        public List<ICM_Incidents.IcmIncident> GetTTAForSOC(List<ICM_Incidents.IcmIncident> incidentsList)
        {
            List<ICM_Incidents.IcmIncident> ttaList = new List<ICM_Incidents.IcmIncident>();
            var ttaLst = incidentsList.Where(a => a.AcknowledgementData.IsAcknowledged == "false" && !string.IsNullOrEmpty(a.CreateDate)).ToList();
            foreach (var item in ttaLst)
            {
                var dur = DateTime.UtcNow.Subtract(Convert.ToDateTime(item.CreateDate));
                Double time = 0;
                time = Convert.ToDouble(ConfigurationManager.AppSettings["SOC_TTA"]);
                item.TTA = GetValueHrsOrDays(dur, time);
                item.TTACategory = GetTTACategory(dur.TotalMinutes, time);
                item.Duration = GetDuration(dur);
                ttaList.Add(item);
            }
            return ttaList;
        }
        public List<ICM_Incidents.IcmIncident> GetTTEForSOC(List<ICM_Incidents.IcmIncident> incidentsList)
        {
            List<ICM_Incidents.IcmIncident> tteList = new List<ICM_Incidents.IcmIncident>();
            foreach (var item in incidentsList)
            {
                if (item.AcknowledgementData.IsAcknowledged == "true" && (!string.IsNullOrEmpty(item.AcknowledgeDate) || !string.IsNullOrEmpty(item.AcknowledgementData.AcknowledgeDate)))
                {
                    string ackDate = string.IsNullOrEmpty(item.AcknowledgeDate) ? GetUtcTimeFrmStrng(item.AcknowledgementData.AcknowledgeDate).ToString() : item.AcknowledgeDate;
                    var dur = DateTime.UtcNow.Subtract(Convert.ToDateTime(ackDate));
                    Double time = 0;

                    if (item.Severity == 0 || item.Severity == 1)
                        time = Convert.ToDouble(ConfigurationManager.AppSettings["SOC_TTR01"]);
                    else if (item.Severity == 2)
                        time = Convert.ToDouble(ConfigurationManager.AppSettings["SOC_TTR2"]);
                    else if (item.Severity == 3 || item.Severity == 4)
                        time = Convert.ToDouble(ConfigurationManager.AppSettings["SOC_TTR34"]);

                    item.TTA = GetValueHrsOrDays(dur, time);
                    item.TTACategory = GetTTACategory(dur.TotalMinutes, time);
                    item.Duration = GetDuration(dur);
                    item.AcknowledgeDate = ackDate;
                    tteList.Add(item);
                }
            }
            return tteList;
        }
        public List<ICM_Incidents.IcmIncident> GetTTEForSd(List<ICM_Incidents.IcmIncident> incidentsList, string track = "")
        {
            List<ICM_Incidents.IcmIncident> tteList = new List<ICM_Incidents.IcmIncident>();
            foreach (var item in incidentsList)
            {
                Double time = 0;
                if (item.Severity == 0)
                    time = Convert.ToDouble(ConfigurationManager.AppSettings["SLA0_TTE"]);
                else if (item.Severity == 1)
                    time = Convert.ToDouble(ConfigurationManager.AppSettings["SLA1_TTE"]);
                else if (item.Severity == 2)
                    time = Convert.ToDouble(ConfigurationManager.AppSettings["SLA2_TTE"]);
                else if (item.Severity == 3)
                    time = Convert.ToDouble(ConfigurationManager.AppSettings["SLA3_TTE"]);
                else if (item.Severity == 4)
                    time = Convert.ToDouble(ConfigurationManager.AppSettings["SLA4_TTE"]);

                if (item.AcknowledgementData.IsAcknowledged == "true" && !string.IsNullOrEmpty(item.AcknowledgeDate) && !item.OwningTeamId.ToUpper().Contains("BUSINESSVOICEBVD"))
                {
                    var dur = DateTime.UtcNow.Subtract(Convert.ToDateTime(item.AcknowledgeDate));
                    item.TTA = GetValueHrsOrDays(dur, time);
                    item.TTACategory = GetTTACategory(dur.TotalMinutes, time);
                    item.Duration = GetDuration(dur);
                    tteList.Add(item);
                }

                else if (item.AcknowledgementData.IsAcknowledged == "true" && !string.IsNullOrEmpty(item.AcknowledgementData.AcknowledgeDate) &&
                   item.OwningTeamId.ToUpper().Contains("BUSINESSVOICEBVD"))
                {
                    if ((!string.IsNullOrEmpty(item.RaisingLocation.DeviceGroup) && item.RaisingLocation.DeviceGroup.ToUpper().Equals("IMP"))
                        || (string.IsNullOrEmpty(item.RaisingLocation.DeviceGroup) && !string.IsNullOrEmpty(item.RaisingLocation.DeviceName) && item.RaisingLocation.DeviceName.ToUpper().Equals("VIEWPOINT")))
                    {
                        var utcDate = GetUtcTimeFrmStrng(item.AcknowledgementData.AcknowledgeDate);
                        var dur = DateTime.UtcNow.Subtract(utcDate);
                        //item.TTA = GetValueHrsOrDays(dur, 0);
                        item.Customer = "YES";
                        item.TTACategory = "BVD";
                        item.AcknowledgeDate = utcDate.ToString();
                        item.Duration = GetDuration(dur);
                        tteList.Add(item);
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (item.AcknowledgementData.IsAcknowledged == "true" && string.IsNullOrEmpty(item.AcknowledgeDate) &&
                    !string.IsNullOrEmpty(item.AcknowledgementData.AcknowledgeDate))
                {
                    var utcDate = GetUtcTimeFrmStrng(item.AcknowledgementData.AcknowledgeDate);
                    item.AcknowledgeDate = utcDate.ToString();
                    var dur = DateTime.UtcNow.Subtract(utcDate);
                    item.Duration = GetDuration(dur);
                    item.Customer = item.Title.Contains("Online") ? "YES" : "NO";
                    item.TTA = GetValueHrsOrDays(dur, time);
                    item.TTACategory = GetTTACategory(dur.TotalMinutes, time);
                    tteList.Add(item);
                }
            }
            return tteList;
        }
        public List<ICM_Incidents.IcmIncident> GetTTARefinedData(List<ICM_Incidents.IcmIncident> incidentsList, string track = "")
        {
            List<ICM_Incidents.IcmIncident> ttaList = new List<ICM_Incidents.IcmIncident>();
            var ttaLst = incidentsList.Where(a => a.AcknowledgementData.IsAcknowledged == "false" && !string.IsNullOrEmpty(a.CreateDate)).ToList();
            foreach (var item in ttaLst)
            {
                //if (!string.IsNullOrEmpty(item.CreateDate))
                //{
                var dur = DateTime.UtcNow.Subtract(Convert.ToDateTime(item.CreateDate));
                Double time = 0;

                if (item.Severity == 0)
                    time = Convert.ToDouble(ConfigurationManager.AppSettings["SLA0_TTA"]);
                else if (item.Severity == 1)
                    time = Convert.ToDouble(ConfigurationManager.AppSettings["SLA1_TTA"]);
                else if (item.Severity == 2)
                    time = Convert.ToDouble(ConfigurationManager.AppSettings["SLA2_TTA"]);
                else if (item.Severity == 3)
                    time = Convert.ToDouble(ConfigurationManager.AppSettings["SLA3_TTA"]);
                else if (item.Severity == 4)
                    time = Convert.ToDouble(ConfigurationManager.AppSettings["SLA4_TTA"]);

                //if (item.OwningTeamId.ToUpper().Contains("BUSINESSVOICEBVD") && !string.IsNullOrEmpty(item.RaisingLocation.DeviceGroup))
                //{
                //    if (item.RaisingLocation.DeviceGroup.ToUpper().Equals("IMP"))
                //    {
                //        //item.TTA = GetValueHrsOrDays(dur, 0);
                //        item.TTACategory = "BVD";
                //        item.Customer = "YES";
                //    }
                //    else
                //    {
                //        continue;
                //    }
                //}
                //else if (item.OwningTeamId.ToUpper().Contains("BUSINESSVOICEBVD") && string.IsNullOrEmpty(item.RaisingLocation.DeviceGroup))
                //{
                //    if (!string.IsNullOrEmpty(item.RaisingLocation.DeviceName) && item.RaisingLocation.DeviceName.ToUpper().Equals("VIEWPOINT"))
                //    {
                //        item.TTACategory = "BVD";
                //        item.Customer = "YES";
                //    }
                //    else
                //    {
                //        continue;
                //    }
                //}
                ////else
                //{
                item.TTA = GetValueHrsOrDays(dur, time);
                item.TTACategory = GetTTACategory(dur.TotalMinutes, time);
                //}

                item.Duration = GetDuration(dur);
                ttaList.Add(item);
                //}
            }
            return ttaList;
        }
        public List<ICM_Incidents.IcmIncident> GetTTERefinedData(List<ICM_Incidents.IcmIncident> incidentsList, string track = "")
        {
            List<ICM_Incidents.IcmIncident> tteList = new List<ICM_Incidents.IcmIncident>();
            foreach (var item in incidentsList)
            {

                Double time = 0;
                if (item.Severity == 0)
                    time = Convert.ToDouble(ConfigurationManager.AppSettings["SLA0_TTE"]);
                else if (item.Severity == 1)
                    time = Convert.ToDouble(ConfigurationManager.AppSettings["SLA1_TTE"]);
                else if (item.Severity == 2)
                    time = Convert.ToDouble(ConfigurationManager.AppSettings["SLA2_TTE"]);
                else if (item.Severity == 3)
                    time = Convert.ToDouble(ConfigurationManager.AppSettings["SLA3_TTE"]);
                else if (item.Severity == 4)
                    time = Convert.ToDouble(ConfigurationManager.AppSettings["SLA4_TTE"]);

                if (item.AcknowledgementData.IsAcknowledged == "true" && !string.IsNullOrEmpty(item.AcknowledgeDate)) //&& !item.OwningTeamId.ToUpper().Contains("BUSINESSVOICEBVD"))
                {
                    var dur = DateTime.UtcNow.Subtract(Convert.ToDateTime(item.AcknowledgeDate));
                    item.TTA = GetValueHrsOrDays(dur, time);
                    item.TTACategory = GetTTACategory(dur.TotalMinutes, time);
                    item.Duration = GetDuration(dur);
                    tteList.Add(item);
                }

                //else if (item.AcknowledgementData.IsAcknowledged == "true" && !string.IsNullOrEmpty(item.AcknowledgementData.AcknowledgeDate) &&
                //   item.OwningTeamId.ToUpper().Contains("BUSINESSVOICEBVD"))
                //{
                //    if ((!string.IsNullOrEmpty(item.RaisingLocation.DeviceGroup) && item.RaisingLocation.DeviceGroup.ToUpper().Equals("IMP"))
                //        || (string.IsNullOrEmpty(item.RaisingLocation.DeviceGroup) && !string.IsNullOrEmpty(item.RaisingLocation.DeviceName) && item.RaisingLocation.DeviceName.ToUpper().Equals("VIEWPOINT")))
                //    {
                //        var utcDate = GetUtcTimeFrmStrng(item.AcknowledgementData.AcknowledgeDate);
                //        var dur = DateTime.UtcNow.Subtract(utcDate);
                //        //item.TTA = GetValueHrsOrDays(dur, 0);
                //        item.Customer = "YES";
                //        item.TTACategory = "BVD";
                //        item.AcknowledgeDate = utcDate.ToString();
                //        item.Duration = GetDuration(dur);
                //        tteList.Add(item);
                //    }
                //    else
                //    {
                //        continue;
                //    }
                //}
                else if (item.AcknowledgementData.IsAcknowledged == "true" && string.IsNullOrEmpty(item.AcknowledgeDate) &&
                    !string.IsNullOrEmpty(item.AcknowledgementData.AcknowledgeDate))
                {
                    var utcDate = GetUtcTimeFrmStrng(item.AcknowledgementData.AcknowledgeDate);
                    item.AcknowledgeDate = utcDate.ToString();
                    var dur = DateTime.UtcNow.Subtract(utcDate);
                    item.Duration = GetDuration(dur);
                    item.Customer = item.Title.Contains("Online") ? "YES" : "NO";
                    item.TTA = GetValueHrsOrDays(dur, time);
                    item.TTACategory = GetTTACategory(dur.TotalMinutes, time);
                    tteList.Add(item);
                }
            }
            return tteList;
        }
        private string IsCustomerOrNot(ICM_Incidents.RaisingLocation data)
        {
            string result = "NO";
            if (string.IsNullOrEmpty(data.DeviceGroup))
            {
                if (string.IsNullOrEmpty(data.DeviceName))
                    result = "NO";
                else if (data.DeviceName.ToUpper().Equals("VIEWPOINT"))
                    result = "YES";
            }
            else if (data.DeviceGroup.ToUpper().Equals("IMP"))
                result = "YES";
            else
                result = "NO";
            return result;
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

        private string GetValueHrsOrDays(TimeSpan duration, double tta)
        {
            string result = string.Empty;
            double timeData;
            if (tta >= duration.TotalMinutes)
            {
                timeData = tta - duration.TotalMinutes;
                if (timeData >= 0 && timeData < 60)
                    result = Convert.ToInt32(timeData) + " minutes left";
                else if (timeData > 60 && timeData < 1440)
                    result = Convert.ToInt32(timeData / 60) + " hours left";
                else
                    result = Convert.ToInt32((timeData / 60) / 24) + " days left";
            }
            else
            {
                timeData = duration.TotalMinutes - tta;
                if (timeData >= 0 && timeData < 60)
                    result = Convert.ToInt32(timeData) + " minutes overdue";
                else if (timeData > 60 && timeData < 1440)
                    result = Convert.ToInt32(timeData / 60) + " hours overdue";
                else
                    result = Convert.ToInt32((timeData / 60) / 24) + " days overdue";
            }
            return result;
        }
        private string GetTTACategory(double totalTm, double tta)
        {
            if (tta == 0)
            {
                return "REDYELLOW";
            }
            if (totalTm < tta)
            {
                if (totalTm < (tta * 0.5))
                    return "GREEN";
                else if (totalTm >= (tta * 0.5) && totalTm < tta)
                    return "ORANGE";
                else
                    return "RED";
            }
            else
            {
                return "RED";
            }
        }

        public void GetTTAForCVM(ref List<ICM_Incidents.IcmIncident> incidentList)
        {
            try
            {
                double time = Convert.ToDouble(ConfigurationManager.AppSettings["CvmMitigatedThreshold"]);
                foreach (var item in incidentList)
                {
                    var dur = DateTime.UtcNow.Subtract(Convert.ToDateTime(item.MitigationData.Date));
                    item.TTA = GetValueHrsOrDays(dur, time);
                    item.TTACategory = GetTTACategory(dur.TotalMinutes, time);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetDuration(TimeSpan dur)
        {
            string timeDur;
            if (Convert.ToInt32(dur.TotalDays) > 0)
                timeDur = Convert.ToInt32(dur.TotalDays) + " Days";
            else if (Convert.ToInt32(dur.TotalHours) > 0)
                timeDur = Convert.ToInt32(dur.TotalHours) + " Hours";
            else
                timeDur = Convert.ToInt32(dur.TotalMinutes) + " Mins";
            return timeDur;
        }
        public List<IcmIncident> GetRefinedHistoryData(DataTable data, string team)
        {
            List<IcmIncident> refinedList = new List<IcmIncident>();
            var list = (from DataRow dt in data.Rows
                        select new IcmIncident
                        {
                            Id = dt["IncidentId"].ToString(),
                            ModifiedDate = dt["ModifiedDate"].ToString(),
                            OwningTeamId = dt["OwningTeamName"].ToString().Replace("\\", "*").Split('*')[1],

                        }).ToList();

            var incGrp = list.GroupBy(a => a.Id).ToList();
            foreach (var item in incGrp)
            {
                IcmIncident incident = new IcmIncident();
                var history = list.Where(a => a.Id.Equals(item.Key)).ToList();
                var grp = history.GroupBy(a => a.OwningTeamId).ToList();
                bool isData = false;
                var histArr = history.ToArray();
                for (int i = 0; i < histArr.Count(); i++)
                {
                    if (!isData)
                    {
                        if ((team == "OCDM" && (histArr[i].OwningTeamId.Contains("IC3ProvisioningTriage") || histArr[i].OwningTeamId.Contains("RTCAPPSRE")))
                            || (team == "BVD" && (histArr[i].OwningTeamId.Contains("BusinessVoiceBVD") || histArr[i].OwningTeamId.Contains("IC3RuntimeStoreTriage"))))
                        {
                            isData = true;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    if (i == histArr.Count() - 1)
                    {
                        refinedList.Add(new IcmIncident { Id = histArr[i].Id, OwningTeamId = histArr[i].OwningTeamId, ModifiedDate = histArr[i].ModifiedDate });
                    }
                    else
                    {
                        if (histArr[i].OwningTeamId == histArr[i + 1].OwningTeamId)
                        {
                            continue;
                        }
                        else
                        {
                            refinedList.Add(new IcmIncident { Id = histArr[i].Id, OwningTeamId = histArr[i].OwningTeamId, ModifiedDate = histArr[i].ModifiedDate });
                        }
                    }
                }
            }
            return refinedList;
        }

        public string GetShiftData(string ddlShift, ref string shiftInfo)
        {
            string dateTime = string.Empty;
            string FromDate = "", ToDate = "", FromTm = "", ToTm = "";
            TimeZoneInfo US_ZONE = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            if (ddlShift.Contains("SHIFT A"))
            {
                // India Morning: 5 PM - 1 AM PST sys_created_on>=2018-06-18 17:00:00 And sys_created_on<2018-06-19 01:00:00
                FromDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow.AddDays(-1), US_ZONE).ToString("yyyy-MM-dd");
                ToDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow, US_ZONE).ToString("yyyy-MM-dd");// + " 09:00:00";
                FromTm = "17:00:00";
                ToTm = "01:00:00";
            }
            else if (ddlShift.Contains("SHIFT B"))
            {
                //Redmond early morning - 1 AM - 9 AM PST - India evening
                FromDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow, US_ZONE).ToString("yyyy-MM-dd");// + " 09:00:00";
                ToDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow, US_ZONE).ToString("yyyy-MM-dd");// + " 17:00:00";
                FromTm = "01:00:00";
                ToTm = "09:00:00";
            }
            else if (ddlShift.Contains("SHIFT C"))
            {
                FromDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow, US_ZONE).ToString("yyyy-MM-dd");// + " 01:00:00";
                ToDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow, US_ZONE).ToString("yyyy-MM-dd");// + " 01:00:00";
                FromTm = "09:00:00";
                ToTm = "17:00:00";
                //dateTime = From + "T17:00:00Z|" + To + "T01:00:00Z";
            }
            shiftInfo = FromDate + ", " + FromTm + " To " + ToTm;
            dateTime = FromDate + "T" + FromTm + "Z|" + ToDate + "T" + ToTm + "Z";
            return dateTime;
        }

    }
}
