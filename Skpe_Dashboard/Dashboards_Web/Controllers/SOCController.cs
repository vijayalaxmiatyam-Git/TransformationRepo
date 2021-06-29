using Dashboards_BusinessLayer;
using Dashboards_Web.Models;
using Dashboards_Web.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Dashboards_Web.Controllers
{
    public class SOCController : Controller
    {
        // GET: SOC
        readonly IICMDataHandler SocDataHandler;
        string thumbprint = "";
        public SOCController(IICMDataHandler socDataHandler)
        {
            this.SocDataHandler = socDataHandler;
            thumbprint = ConfigurationManager.AppSettings["Thumbprint"];
        }
        public ActionResult SOCHandOffReport(SocDataModel model, string Severity, string Status, string TeamName, string IsCarry = "NO")
        {
            List<string> filter = new List<string>();
            bool flag = false;
            SocDataModel newModel = new SocDataModel();
            if (!string.IsNullOrEmpty(model.ttaSort) && !string.IsNullOrEmpty(model.ttaSortDir))
            {
                var temp = (SocDataModel)Session["ModelData"];
                newModel.Shift = temp.Shift;
                newModel.TtaList = temp.TtaList;
                newModel.TeamList = temp.TeamList;
                newModel.ShiftInfo = temp.ShiftInfo;
                newModel.TeamList = temp.TeamList;
                newModel.TtaList = temp.TtaList;
                newModel.SOCCorp = temp.SOCCorp;
                newModel.SOCDedicated = temp.SOCDedicated;
                newModel.SOCItar = temp.SOCItar;
                newModel.SOCMultiTenant = temp.SOCMultiTenant;
                newModel.SOCTriage = temp.SOCTriage;
                newModel.SOCWBWW = temp.SOCWBWW;
                newModel.AllTrackSoc = temp.AllTrackSoc;
                IQueryable<TTADataModel> tta = SortIQueryable<TTADataModel>(newModel.TtaList.AsQueryable(), model.ttaSort, model.ttaSortDir);
                newModel.TtaList = tta.ToList();
                ModelState.Remove("AllTrackSoc");
                ModelState.Remove("SOCTriage");
                ModelState.Remove("SOCWBWW");
                ModelState.Remove("SOCDedicated");
                ModelState.Remove("SOCCorp");
                ModelState.Remove("SOCMultiTenant");
                ModelState.Remove("SOCItar");
            }

            if (string.IsNullOrEmpty(Severity) && string.IsNullOrEmpty(Status) && string.IsNullOrEmpty(TeamName) && string.IsNullOrEmpty(model.ttaSortDir))
            {
                Session["ModelData"] = null;
            }
            else if (!string.IsNullOrEmpty(model.ttaSortDir) || (!string.IsNullOrEmpty(Severity) && !string.IsNullOrEmpty(Status) && !string.IsNullOrEmpty(TeamName)))
            {
                flag = true;
                var temp = (SocDataModel)Session["ModelData"];
                model.Shift = temp.Shift;
                model.TtaList = temp.TtaList;
                model.TeamList = temp.TeamList;
                model.ShiftInfo = temp.ShiftInfo;
                model.SOCCorp = temp.SOCCorp;
                model.SOCDedicated = temp.SOCDedicated;
                model.SOCItar = temp.SOCItar;
                model.SOCMultiTenant = temp.SOCMultiTenant;
                model.SOCTriage = temp.SOCTriage;
                model.SOCWBWW = temp.SOCWBWW;
                model.AllTrackSoc = temp.AllTrackSoc;
            }
            if (!string.IsNullOrEmpty(model.Shift) && flag == false)
            {
                model.TtaList = new List<TTADataModel>();

                GetConfig(ref filter, ref model);
                foreach (var item in filter)
                {
                    bool isCarry = false;
                    var i = item;
                    if (i.Contains("@CarryForward"))
                    {
                        i = i.Replace("@CarryForward", string.Empty);
                        isCarry = true;
                    }
                    List<ICM_Incidents.IcmIncident> incidentList = new List<ICM_Incidents.IcmIncident>();
                    string data = SocDataHandler.GetIncident(i, thumbprint);
                    var obj = JsonConvert.DeserializeObject(data) as dynamic;
                    var pageOfIncidents = JsonConvert.DeserializeObject<List<ICM_Incidents.IcmIncident>>(obj.value.ToString());
                    incidentList.AddRange(pageOfIncidents);
                    FillModel(isCarry, ref model, incidentList);
                }
                string kustoError = "";
                ViewBag.KustoError = kustoError;

                var groupData = model.TtaList.GroupBy(a => a.IncidentNo).Select(a => a.First());
                model.TtaList = new List<TTADataModel>();
                foreach (var item in groupData)
                {
                    model.TtaList.Add(item);
                }
                Session["ModelData"] = model;
            }
            if (model.TeamList != null)
            {
                foreach (var item in model.TeamList)
                {
                    item.Sev1ActiveCount = model.TtaList.Count(a => a.Severity.Equals(1) && a.IsCarryForward.Equals("NO") && a.Status.ToUpper().Equals("ACTIVE") && a.OwningTeam.Contains(item.TeamVal));
                    item.Sev2ActiveCount = model.TtaList.Count(a => a.Severity.Equals(2) && a.IsCarryForward.Equals("NO") && a.Status.ToUpper().Equals("ACTIVE") && a.OwningTeam.Contains(item.TeamVal));
                    item.Sev3ActiveCount = model.TtaList.Count(a => a.Severity.Equals(3) && a.IsCarryForward.Equals("NO") && a.Status.ToUpper().Equals("ACTIVE") && a.OwningTeam.Contains(item.TeamVal));
                    item.Sev4ActiveCount = model.TtaList.Count(a => a.Severity.Equals(4) && a.IsCarryForward.Equals("NO") && a.Status.ToUpper().Equals("ACTIVE") && a.OwningTeam.Contains(item.TeamVal));
                    item.Sev1MitigatedCount = model.TtaList.Count(a => a.Severity.Equals(1) && a.Status.ToUpper().Equals("MITIGATED") && a.OwningTeam.Contains(item.TeamVal));
                    item.Sev2MitigatedCount = model.TtaList.Count(a => a.Severity.Equals(2) && a.Status.ToUpper().Equals("MITIGATED") && a.OwningTeam.Contains(item.TeamVal));
                    item.Sev3MitigatedCount = model.TtaList.Count(a => a.Severity.Equals(3) && a.Status.ToUpper().Equals("MITIGATED") && a.OwningTeam.Contains(item.TeamVal));
                    item.Sev4MitigatedCount = model.TtaList.Count(a => a.Severity.Equals(4) && a.Status.ToUpper().Equals("MITIGATED") && a.OwningTeam.Contains(item.TeamVal));
                    item.Sev1ResolvedCount = model.TtaList.Count(a => a.Severity.Equals(1) && a.Status.ToUpper().Equals("RESOLVED") && a.OwningTeam.Contains(item.TeamVal));
                    item.Sev2ResolvedCount = model.TtaList.Count(a => a.Severity.Equals(2) && a.Status.ToUpper().Equals("RESOLVED") && a.OwningTeam.Contains(item.TeamVal));
                    item.Sev3ResolvedCount = model.TtaList.Count(a => a.Severity.Equals(3) && a.Status.ToUpper().Equals("RESOLVED") && a.OwningTeam.Contains(item.TeamVal));
                    item.Sev4ResolvedCount = model.TtaList.Count(a => a.Severity.Equals(4) && a.Status.ToUpper().Equals("RESOLVED") && a.OwningTeam.Contains(item.TeamVal));
                    item.Sev1CFActiveCount = model.TtaList.Count(a => a.Severity.Equals(1) && a.IsCarryForward.Equals("YES") && a.Status.ToUpper().Equals("ACTIVE") && a.OwningTeam.Contains(item.TeamVal));
                    item.Sev2CFActiveCount = model.TtaList.Count(a => a.Severity.Equals(2) && a.IsCarryForward.Equals("YES") && a.Status.ToUpper().Equals("ACTIVE") && a.OwningTeam.Contains(item.TeamVal));
                    item.Sev3CFActiveCount = model.TtaList.Count(a => a.Severity.Equals(3) && a.IsCarryForward.Equals("YES") && a.Status.ToUpper().Equals("ACTIVE") && a.OwningTeam.Contains(item.TeamVal));
                    item.Sev4CFActiveCount = model.TtaList.Count(a => a.Severity.Equals(4) && a.IsCarryForward.Equals("YES") && a.Status.ToUpper().Equals("ACTIVE") && a.OwningTeam.Contains(item.TeamVal));
                    item.ActiveTotal = model.TtaList.Count(a => a.IsCarryForward.Equals("NO") && a.Status.ToUpper().Equals("ACTIVE") && a.OwningTeam.Contains(item.TeamVal));
                    item.MitigatedTotal = model.TtaList.Count(a => a.Status.ToUpper().Equals("MITIGATED") && a.OwningTeam.Contains(item.TeamVal));
                    item.ResolvedTotal = model.TtaList.Count(a => a.Status.ToUpper().Equals("RESOLVED") && a.OwningTeam.Contains(item.TeamVal));
                    item.CFActiveTotal = model.TtaList.Count(a => a.IsCarryForward.Equals("YES") && a.Status.ToUpper().Equals("ACTIVE") && a.OwningTeam.Contains(item.TeamVal));
                    item.TeamTotal = model.TtaList.Count(a => a.OwningTeam.Contains(item.TeamVal));
                }
                ViewBag.TeamData = model.TeamList;
                ViewBag.GrandTotal = model.TeamList.Sum(a => a.TeamTotal);
                model.HandOffEmail = ConfigurationManager.AppSettings["SOCHandoffEmail"];
            }

            if (newModel == null || newModel.TtaList == null)
                newModel = model;

            if (!string.IsNullOrEmpty(Severity) && !string.IsNullOrEmpty(TeamName) && !string.IsNullOrEmpty(Status))
            {
                if (Severity.Equals("Total"))
                {
                    newModel.TtaList = newModel.TtaList.Where(a => a.OwningTeam.Contains(TeamName) && a.Status.Equals(Status) && a.IsCarryForward.Equals(IsCarry)).ToList();
                    if (IsCarry == "YES")
                    {
                        ViewBag.IcmType = newModel.TeamList.First(a => a.TeamVal.Equals(TeamName)).FriendlyName + " Carry Forwarded";
                    }
                    else
                    {
                        ViewBag.IcmType = newModel.TeamList.First(a => a.TeamVal.Equals(TeamName)).FriendlyName + " " + Status;
                    }
                }
                else
                {
                    if (IsCarry == "YES")
                    {
                        ViewBag.IcmType = newModel.TeamList.First(a => a.TeamVal.Equals(TeamName)).FriendlyName + " Carry Forwarded Sev" + Severity;
                        newModel.TtaList = newModel.TtaList.Where(a => a.IsCarryForward.Equals(IsCarry) && a.OwningTeam.Equals(TeamName) && a.Status.Equals(Status) && a.Severity.Equals(Convert.ToInt16(Severity))).ToList();
                    }
                    else
                    {
                        ViewBag.IcmType = newModel.TeamList.First(a => a.TeamVal.Equals(TeamName)).FriendlyName + " " + Status + " Sev" + Severity;
                        newModel.TtaList = newModel.TtaList.Where(a => a.Severity.Equals(Convert.ToInt16(Severity)) && a.OwningTeam.Equals(TeamName) && a.Status.Equals(Status) && a.IsCarryForward.Equals(IsCarry)).ToList();
                    }
                }
            }
            if (string.IsNullOrEmpty(model.ttaSort) && string.IsNullOrEmpty(model.ttaSortDir) && newModel.TtaList != null && newModel.TtaList.Count() > 1)
            {
                newModel.TtaList = newModel.TtaList.OrderByDescending(a => a.ModifiedDate).ToList();
            }
            newModel.Type = "SOC";
            return View(newModel);
        }
        public static IQueryable<T> SortIQueryable<T>(IQueryable<T> data, string fieldName, string sortOrder)
        {
            if (string.IsNullOrWhiteSpace(fieldName)) return data;
            if (string.IsNullOrWhiteSpace(sortOrder)) return data;

            var param = System.Linq.Expressions.Expression.Parameter(typeof(T), "i");
            System.Linq.Expressions.Expression conversion = System.Linq.Expressions.Expression.Convert(System.Linq.Expressions.Expression.Property(param, fieldName), typeof(object));
            var mySortExpression = System.Linq.Expressions.Expression.Lambda<Func<T, object>>(conversion, param);

            return (sortOrder.ToLower() == "desc") ? data.OrderByDescending(mySortExpression)
                : data.OrderBy(mySortExpression);
        }
        private void GetEffortData(ref SocDataModel model, ref string kustoError)
        {
            try
            {
                string file;
                string reader = $"~/Content/Efforts.txt";//Need to change

                using (StreamReader readFl = new StreamReader(Server.MapPath(reader)))
                {
                    file = readFl.ReadToEnd();
                }
                if (model.TtaList.Count() > 0)
                {
                    for (var item = 0; item < model.TtaList.Count(); item++)
                    {
                        if (item == model.TtaList.Count() - 1)
                        {
                            file = file.Replace("@IncidentIds", model.TtaList[item].IncidentNo.ToString());
                        }
                        else
                        {
                            file = file.Replace("@IncidentIds", model.TtaList[item].IncidentNo.ToString() + ",@IncidentIds");
                        }
                    }
                    var dataTable = new DataTable();
                    dataTable = SocDataHandler.ExecuteKustoQueryForTest(file);
                    ExtractData(dataTable, ref model);
                }
            }
            catch (Exception)
            {
                kustoError = "Currently Kusto is unable to bring the Efforts and ICM comments data";
            }
        }
        private void ExtractData(DataTable data, ref SocDataModel model)
        {
            if (data.Rows != null && data.Rows.Count > 0)
            {
                foreach (var item in model.TtaList)
                {
                    var results = data.AsEnumerable()
                                  .Where(a => a.ItemArray[0].ToString() == item.IncidentNo.ToString())
                                  .Select(a => new
                                  {
                                      ID = a.Field<Int64>("IncidentId"),
                                      Name = a.Field<string>("Name"),
                                      Value = a.Field<string>("Value")
                                  }).ToList();


                    if (results.Count() > 0)
                    {
                        double effort = 0;
                        string comment = string.Empty;
                        foreach (var field in results)
                        {
                            if (field.Name.Contains("_Comments"))
                            {
                                if (string.IsNullOrEmpty(comment))
                                    comment = comment + field.Value;
                                else
                                {
                                    comment = comment + " | " + field.Value;
                                }
                            }
                            else if (field.Name.Contains("Efforts"))
                            {
                                effort = effort + Convert.ToDouble(field.Value);
                            }
                        }
                        item.IcmComment = comment;
                        item.Efforts = effort;
                    }
                }
            }
        }
        private void FillModel(bool isCarryForward, ref SocDataModel model, List<ICM_Incidents.IcmIncident> incidentList)
        {
            //List<SelectListItem> items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Text = "Pending", Value = "Pending" });
            //items.Add(new SelectListItem { Text = "Ongoing", Value = "Ongoing" });
            //items.Add(new SelectListItem { Text = "Follow-up", Value = "Follow-up" });
            //items.Add(new SelectListItem { Text = "Actionable", Value = "Actionable" });
            //items.Add(new SelectListItem { Text = "Non-Actionable", Value = "Non-Actionable" });
            foreach (var item in incidentList)
            {
                if (item.Status.ToUpper().Equals("MITIGATED"))
                {
                    model.TtaList.Add(new TTADataModel
                    {
                        IncidentNo = Convert.ToInt32(item.Id),
                        CreateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Parse(item.CreateDate), "Pacific Standard Time"),
                        OwningContactAlias = item.OwningContactAlias,
                        OwningTeam = item.OwningTeamId.Replace("\\", "*").Split('*')[1],
                        Severity = item.Severity,
                        Status = item.Status,
                        Title = item.Title,
                        MitigatedDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Parse(item.MitigationData.Date), "Pacific Standard Time"),
                        Mitigation = item.MitigationData.Mitigation,
                        ChangedBy = item.MitigationData.ChangedBy,
                        ModifiedDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Parse(item.ModifiedDate), "Pacific Standard Time"),
                        IsCarryForward = isCarryForward ? "YES" : "NO",
                        TicketAge = SocDataHandler.GetDuration(DateTime.UtcNow.Subtract(Convert.ToDateTime(item.CreateDate))),
                        IncidentRelation = item.Id.Equals(item.ParentIncidentId) || string.IsNullOrEmpty(item.ParentIncidentId) ? "Parent" : "Child",
                        TTAMet = string.IsNullOrEmpty(item.AcknowledgementData.AcknowledgeDate) ? "NACK" : (IsTtaMet(DateTime.Parse(item.CreateDate), DateTime.Parse(item.AcknowledgementData.AcknowledgeDate)) ? "Yes" : "No")
                        //ShiftStatus = items
                    });
                }
                else
                {
                    model.TtaList.Add(new TTADataModel
                    {
                        IncidentNo = Convert.ToInt32(item.Id),
                        CreateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Parse(item.CreateDate), "Pacific Standard Time"),
                        OwningContactAlias = item.OwningContactAlias,
                        OwningTeam = item.OwningTeamId.Replace("\\", "*").Split('*')[1],
                        Severity = item.Severity,
                        Status = item.Status,
                        Title = item.Title,
                        MitigatedDate = null,
                        ModifiedDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Parse(item.ModifiedDate), "Pacific Standard Time"),
                        IsCarryForward = isCarryForward ? "YES" : "NO",
                        TicketAge = SocDataHandler.GetDuration(DateTime.UtcNow.Subtract(Convert.ToDateTime(item.CreateDate))),
                        IncidentRelation = item.Id.Equals(item.ParentIncidentId) || string.IsNullOrEmpty(item.ParentIncidentId) ? "Parent" : "Child",
                        TTAMet = string.IsNullOrEmpty(item.AcknowledgementData.AcknowledgeDate) ? "NACK" : (IsTtaMet(DateTime.Parse(item.CreateDate), DateTime.Parse(item.AcknowledgementData.AcknowledgeDate)) ? "Yes" : "No")
                        //ShiftStatus = items
                    });
                }

            }
        }
        private bool IsTtaMet(DateTime createdDate, DateTime assignedDate)
        {
            var pstCdate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(createdDate, "Pacific Standard Time");
            var pstackdate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(assignedDate, "Pacific Standard Time");
            if (pstCdate.Subtract(pstackdate).TotalMinutes > 10)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private void GetConfig(ref List<string> filter, ref SocDataModel model)
        {
            string shift = string.Empty;
            string dateTm = SocDataHandler.GetShiftData(model.Shift, ref shift);
            model.ShiftInfo = shift;
            string Config;
            model.TeamList = new List<Team>();
            if (model.SOCCorp)
            {
                Config = ConfigurationManager.AppSettings["HandOffReport"];
                Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\SOCCorp");
                model.TeamList.Add(new Team { TeamName = "SOC Corp", TeamVal = "SOCCorp", FriendlyName = "SOC Corp" });
                filter.Add(Config);
                Config = ConfigurationManager.AppSettings["CarryForward"];
                Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\SOCCorp");
                filter.Add(Config);
            }
            if (model.SOCTriage)
            {
                Config = ConfigurationManager.AppSettings["HandOffReport"];
                Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\Triage");
                model.TeamList.Add(new Team { TeamName = "*SOC Triage", TeamVal = "Triage", FriendlyName = "*SOC Triage" });
                filter.Add(Config);
                Config = ConfigurationManager.AppSettings["CarryForward"];
                Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\Triage");
                filter.Add(Config);
            }
            if (model.SOCWBWW)
            {
                Config = ConfigurationManager.AppSettings["HandOffReport"];
                Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\SkypeOperationsCenter");
                model.TeamList.Add(new Team { TeamName = "SOC (WB and WW only)", TeamVal = "SkypeOperationsCenter", FriendlyName = "SOC WB and WW" });
                filter.Add(Config);
                Config = ConfigurationManager.AppSettings["CarryForward"];
                Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\SkypeOperationsCenter");
                filter.Add(Config);
            }
            if (model.SOCDedicated)
            {
                Config = ConfigurationManager.AppSettings["HandOffReport"];
                Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\SOCDedicated");
                model.TeamList.Add(new Team { TeamName = "SOC Dedicated", TeamVal = "SOCDedicated", FriendlyName = "SOC Dedicated" });
                filter.Add(Config);
                Config = ConfigurationManager.AppSettings["CarryForward"];
                Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\SOCDedicated");
                filter.Add(Config);
            }
            if (model.SOCItar)
            {
                Config = ConfigurationManager.AppSettings["HandOffReport"];
                Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\SOCITAR");
                model.TeamList.Add(new Team { TeamName = "SOC ITAR", TeamVal = "SOCITAR", FriendlyName = "SOC ITAR" });
                filter.Add(Config);
                Config = ConfigurationManager.AppSettings["CarryForward"];
                Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\SOCITAR");
                filter.Add(Config);
            }
            if (model.SOCMultiTenant)
            {
                Config = ConfigurationManager.AppSettings["HandOffReport"];
                Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\SOCMulti-Tenant");
                model.TeamList.Add(new Team { TeamName = "SOC Multi-Tenant", TeamVal = "SOCMulti-Tenant", FriendlyName = "SOC Multi-Tenant" });
                filter.Add(Config);
                Config = ConfigurationManager.AppSettings["CarryForward"];
                Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\SOCMulti-Tenant");
                filter.Add(Config);
            }
            filter = filter.Select(a => a.Replace("@FromDate", dateTm.Split('|')[0])).ToList();
            filter = filter.Select(a => a.Replace("@ToDate", dateTm.Split('|')[1])).ToList();

        }
        public ActionResult SocHome(IcmDataModel model, string Days = "90")
        {
            try
            {
                string kustoErr = string.Empty;
                if ((!string.IsNullOrEmpty(model.ttaSortDir) || !string.IsNullOrEmpty(model.tteSortDir)) && Session["DataToSort"] != null)
                {
                    var sortModel = (IcmDataModel)Session["DataToSort"];
                    foreach (var item in sortModel.TtaList)
                    {
                        item.TicketAge = TimeDuration.ConvertTicketAgeDurationToMins(item.TicketAge.ToString());
                        item.TTA = TimeDuration.ConvertTTETTADurationToMinutes(item.TTA.ToString());
                    }
                    foreach (var item in sortModel.TteList)
                    {
                        item.TicketAge = TimeDuration.ConvertTicketAgeDurationToMins(item.TicketAge.ToString());
                        item.TTE = TimeDuration.ConvertTTETTADurationToMinutes(item.TTE.ToString());
                    }
                    model.TtaList = sortModel.TtaList;
                    model.TteList = sortModel.TteList;
                }
                else
                {
                    FillSocModel(ref model, Days, ref kustoErr);
                }
                ViewBag.TotalCount = model.TtaList.Count() + model.TteList.Count();
                ViewBag.TriageCount = model.TtaList.Count();
                ViewBag.AsignedCount = model.TteList.Count();
                //ViewBag.SEV1Count = model.TtaList.Count(a => a.Severity == 1) + model.TteList.Count(a => a.Severity == 1);
                //ViewBag.SEV2Count = model.TtaList.Count(a => a.Severity == 2) + model.TteList.Count(a => a.Severity == 2);
                ViewBag.SEV3Count = model.TtaList.Count(a => a.Severity == 3) + model.TteList.Count(a => a.Severity == 3);
                ViewBag.SEV4Count = model.TtaList.Count(a => a.Severity == 4) + model.TteList.Count(a => a.Severity == 4);
                ViewBag.CustomerCount = model.TtaList.Count(a => a.IsCustomer != null && a.IsCustomer.Equals("YES")) + model.TteList.Count(a => a.IsCustomer != null && a.IsCustomer.Equals("YES"));
                ViewBag.NonCustomerCount = model.TtaList.Count(a => a.IsCustomer != null && a.IsCustomer.Equals("NO")) + model.TteList.Count(a => a.IsCustomer != null && a.IsCustomer.Equals("NO"));
                
                if (string.IsNullOrEmpty(model.ttaSort) || string.IsNullOrEmpty(model.tteSort) || model.ttaSort.Equals("TTA") || model.ttaSort.Equals("TTE") || model.ttaSort.Equals("TicketAge"))
                {
                    if (model.ttaSort == null)
                    {
                        model.ttaSort = "TTA";
                    }

                    if (model.tteSort == null)
                    {
                        model.tteSort = "TTE";
                    }
                    TimeDuration.ToggleSortingOrder(model.ttaSortDir, model.tteSortDir);
                    IQueryable<TTADataModel> tta = SortIQueryable<TTADataModel>(model.TtaList.AsQueryable(), model.ttaSort);
                    IQueryable<TTEDataModel> tte = SortIQueryable<TTEDataModel>(model.TteList.AsQueryable(), model.tteSort);
                    model.TtaList = tta.ToList();
                    model.TteList = tte.ToList();

                    for (int i = 0; i < model.TteList.Count; i++)
                    {
                        model.TteList[i].TicketAge = TimeDuration.CovertTicketAgeMinsToDuration((double)model.TteList[i].TicketAge).ToString();
                    }
                    for (int i = 0; i < model.TtaList.Count; i++)
                    {
                        model.TtaList[i].TicketAge = TimeDuration.CovertTicketAgeMinsToDuration((double)model.TtaList[i].TicketAge).ToString();
                    }

                    ViewBag.KustoReminder = kustoErr;

                    for (int i = 0; i < model.TteList.Count; i++)
                    {
                        model.TteList[i].TTE = TimeDuration.CovertTTETTAMinsToDuration((double)model.TteList[i].TTE).ToString();
                    }

                    for (int i = 0; i < model.TtaList.Count; i++)
                    {
                        model.TtaList[i].TTA = TimeDuration.CovertTTETTAMinsToDuration((double)model.TtaList[i].TTA).ToString();
                    }

                    if (model.tteSort != null && model.tteSort.ToLower().Contains("tte"))
                    {
                        var modelTTEList = model.TteList;
                        model.TteList = null;
                        model.TteList = GetTTESortedData(modelTTEList);
                    }
                    if (model.ttaSort != null && model.ttaSort.ToLower().Contains("tta"))
                    {
                        var modelTTEList = model.TtaList;
                        model.TtaList = null;
                        model.TtaList = GetTTASortedData(modelTTEList);
                    }
                }
                else
                {
                    IQueryable<TTADataModel> tta = SortIQueryable(model.TtaList.AsQueryable(), model.ttaSort, model.ttaSortDir);
                    IQueryable<TTEDataModel> tte = SortIQueryable(model.TteList.AsQueryable(), model.tteSort, model.tteSortDir);
                    model.TtaList = tta.ToList();
                    model.TteList = tte.ToList();
                    ViewBag.KustoReminder = kustoErr;
                }
                Session["DataToSort"] = model;
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<TTEDataModel> GetTTESortedData(List<TTEDataModel> modelData)
        {
            var overDueList = modelData.Where(x => x.TTE.ToString().ToLower().Contains("overdue")).ToList();
            var leftList = modelData.Where(x => x.TTE.ToString().ToLower().Contains("left")).ToList();
            if (TimeDuration.SortingOrder.Contains(TimeDuration.AscSort))
            {
                //TODO null check
                //leftList.Reverse();
                overDueList.Reverse();
                leftList.AddRange(overDueList);
                modelData = leftList;
            }
            else
            {
                //TODO null check
                //leftList.Reverse();
                overDueList.Reverse();
                overDueList.AddRange(leftList);
                modelData = overDueList;
            }
            return modelData;
        }
        private List<TTADataModel> GetTTASortedData(List<TTADataModel> modelData)
        {
            var overDueList = modelData.Where(x => x.TTA.ToString().ToLower().Contains("overdue")).ToList();
            var leftList = modelData.Where(x => x.TTA.ToString().ToLower().Contains("left")).ToList();
            if (TimeDuration.SortingOrder.Contains(TimeDuration.AscSort))
            {
                //TODO null check
                overDueList.Reverse();
                leftList.AddRange(overDueList);
                modelData = leftList;
            }
            else
            {
                //TODO null check
                //leftList.Reverse();
                overDueList.Reverse();
                overDueList.AddRange(leftList);
                modelData = overDueList;
            }
            return modelData;
        }
        public static IQueryable<T> SortIQueryable<T>(IQueryable<T> data, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(fieldName)) return data;

            var param = System.Linq.Expressions.Expression.Parameter(typeof(T), "i");
            System.Linq.Expressions.Expression conversion = System.Linq.Expressions.Expression.Convert(System.Linq.Expressions.Expression.Property(param, fieldName), typeof(object));
            var mySortExpression = System.Linq.Expressions.Expression.Lambda<Func<T, object>>(conversion, param);

            if (TimeDuration.SortingOrder.Contains(TimeDuration.DescSort))
            {
                return data.OrderByDescending(mySortExpression);
            }
            else if (TimeDuration.SortingOrder.Contains(TimeDuration.AscSort))
            {
                return data.OrderBy(mySortExpression);
            }
            else
            {
                return data;
            }
        }

        private void FillSocModel(ref IcmDataModel model, string days, ref string kustoError)
        {
            model.TtaList = new List<TTADataModel>();
            model.TteList = new List<TTEDataModel>();
            var incidents = SetIncidentData(days, ref kustoError);
            var ttaData = SocDataHandler.GetTTAForSOC(incidents);
            ttaData = ttaData.OrderByDescending(c => c.TTACategory).ToList();
            foreach (var item in ttaData)
            {
                double ticketAge = TimeDuration.ConvertTicketAgeDurationToMins(item.Duration);
                double tta = TimeDuration.ConvertTTETTADurationToMinutes(item.TTA);
                model.TtaList.Add(new TTADataModel
                {
                    IncidentNo = Convert.ToInt32(item.Id),
                    CreateDate = Convert.ToDateTime(item.CreateDate),
                    IsCustomer = item.Customer,
                    OwningContactAlias = item.OwningContactAlias,
                    OwningTeam = item.OwningTeamId.Replace("\\", "*").Split('*')[1],
                    Severity = item.Severity,
                    Status = item.Status,
                    TicketAge = ticketAge, //item.Duration,
                    Title = item.Title,
                    TTA = tta, //item.TTA,
                    TTACategory = item.TTACategory
                });
            }
            var tteData = SocDataHandler.GetTTEForSOC(incidents);
            if (tteData != null)
            {
                tteData = tteData.OrderByDescending(c => c.TTACategory).ToList();
                foreach (var item in tteData)
                {
                    double ticketAge = TimeDuration.ConvertTicketAgeDurationToMins(item.Duration);
                    double tta = TimeDuration.ConvertTTETTADurationToMinutes(item.TTA);
                    model.TteList.Add(new TTEDataModel
                    {
                        IncidentNo = Convert.ToInt32(item.Id),
                        CreateDate = Convert.ToDateTime(item.CreateDate),
                        IsCustomer = item.Customer,
                        OwningContactAlias = item.OwningContactAlias,
                        OwningTeam = item.OwningTeamId.Replace("\\", "*").Split('*')[1],
                        TicketAge = ticketAge, //item.Duration,
                        Title = item.Title,
                        AcknowledgeDate = Convert.ToDateTime(item.AcknowledgeDate),
                        TTE = tta, //item.TTA,
                        Status = item.Status,
                        Severity = item.Severity,
                        TTECategory = item.TTACategory
                    });
                }

            }
        }
        private List<ICM_Incidents.IcmIncident> SetIncidentData(string noOfDays, ref string kustoError)
        {
            List<ICM_Incidents.IcmIncident> incidentList = new List<ICM_Incidents.IcmIncident>();
            try
            {
                var reqDate = noOfDays == null ? DateTime.UtcNow.AddDays(-1).ToString("o") : DateTime.UtcNow.AddDays(-Convert.ToInt32(noOfDays)).ToString("o");
                List<string> filter = new List<string>();
                GetIcmConfig(ref filter, reqDate);
                foreach (var item in filter)
                {
                    string data = SocDataHandler.GetIncident(item, thumbprint);
                    var obj = JsonConvert.DeserializeObject(data) as dynamic;
                    var pageOfIncidents = JsonConvert.DeserializeObject<List<ICM_Incidents.IcmIncident>>(obj.value.ToString());
                    incidentList.AddRange(pageOfIncidents);
                }
                if (!GetIncidentDates(ref incidentList, noOfDays))
                {
                    kustoError = "Currently Kusto is down. So TTE details will not be available";
                }
            }
            catch (Exception)
            {

            }
            return incidentList;
        }
        private void GetIcmConfig(ref List<string> filter, string reqDate)
        {
            filter.Add(ConfigurationManager.AppSettings["SocSearch1"].Replace("@reqDate", reqDate));
            filter.Add(ConfigurationManager.AppSettings["SocSearch2"].Replace("@reqDate", reqDate));
            filter.Add(ConfigurationManager.AppSettings["SocSearch3"].Replace("@reqDate", reqDate));
            filter.Add(ConfigurationManager.AppSettings["SocSearch4"].Replace("@reqDate", reqDate));
            filter.Add(ConfigurationManager.AppSettings["SocSearch5"].Replace("@reqDate", reqDate));
            filter.Add(ConfigurationManager.AppSettings["SocSearch6"].Replace("@reqDate", reqDate));
        }
        private bool GetIncidentDates(ref List<ICM_Incidents.IcmIncident> incidents, string requDate)
        {
            try
            {
                var dataTable = new DataTable();
                string kustoQuery = GetQuery($"~/Content/SOCDashboard.txt");
                kustoQuery = kustoQuery.Replace("@reqDays", requDate);
                dataTable = SocDataHandler.ExecuteKustoQuery(kustoQuery);
                if (dataTable.Rows.Count > 0)
                {
                    foreach (var item in incidents)
                    {
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            if (dataTable.Rows[i].ItemArray[0].ToString().Trim().Equals(item.Id))
                            {
                                item.CreateDate = dataTable.Rows[i].ItemArray[1].ToString();
                                item.AcknowledgeDate = dataTable.Rows[i].ItemArray[2].ToString();
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private string GetQuery(string path)
        {
            try
            {
                string file;
                using (StreamReader readFl = new StreamReader(Server.MapPath(path)))
                {
                    file = readFl.ReadToEnd();
                }
                return file;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult E911Dashboard(List<SocE911Model> model, string fDate = "", string tDate = "", string sort = "", string sortDir = "")
        {
            try
            {
                List<ICM_Incidents.IcmIncident> incidentList = new List<ICM_Incidents.IcmIncident>();
                ViewData["fromDate"] = fDate;
                ViewData["toDate"] = tDate;
                fDate = string.IsNullOrEmpty(fDate) ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd") : fDate;
                tDate = string.IsNullOrEmpty(tDate) ? DateTime.Now.Date.ToString("yyyy-MM-dd") : tDate;
                if (!string.IsNullOrEmpty(fDate) && !string.IsNullOrEmpty(tDate) && string.IsNullOrEmpty(sort) && string.IsNullOrEmpty(sortDir))
                {
                    List<string> filter = new List<string>();
                    //filter.Add(ConfigurationManager.AppSettings["SocE911"].Replace("@FromDate", fDate).Replace("@ToDate",tDate));
                    //foreach (var item in filter)
                    //{
                    //    string data = SocDataHandler.GetIncident(item, thumbprint);
                    //    var obj = JsonConvert.DeserializeObject(data) as dynamic;
                    //    var pageOfIncidents = JsonConvert.DeserializeObject<List<ICM_Incidents.IcmIncident>>(obj.value.ToString());
                    //    incidentList.AddRange(pageOfIncidents);
                    //}
                    string query = GetQuery($"~/Content/E911_Data.txt");
                    query = query.Replace("@FromDate", fDate).Replace("@ToDate", tDate);
                    DataTable dataTable = SocDataHandler.ExecuteKustoQuery(query);
                    model = new List<SocE911Model>();

                    if (dataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            SocE911Model data = new SocE911Model();
                            data.IncidentId = dataTable.Rows[i].ItemArray[0].ToString();
                            data.CreatedDate = Convert.ToDateTime(dataTable.Rows[i].ItemArray[2].ToString());
                            data.OwningTeam = dataTable.Rows[i].ItemArray[4].ToString().Replace("\\", "*").Split('*')[1];
                            data.Severity = Convert.ToInt32(dataTable.Rows[i].ItemArray[10]);
                            data.Status = dataTable.Rows[i].ItemArray[8].ToString();
                            data.Title = dataTable.Rows[i].ItemArray[1].ToString();
                            data.Owner = dataTable.Rows[i].ItemArray[9].ToString();
                            data.LastModifiedOn = Convert.ToDateTime(dataTable.Rows[i].ItemArray[2].ToString());
                            model.Add(data);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(sortDir))
                {
                    model = (List<SocE911Model>)Session["ExportData"];
                    var sortedData = SortIQueryable<SocE911Model>(model.AsQueryable(), sort, sortDir);
                    model = sortedData.ToList();
                }
                else
                {
                    model = model.OrderByDescending(a => a.CreatedDate).ToList();
                }
                Session["ExportData"] = model;
                return View(model);
            }
            catch (Exception)
            {
                model = null;
                return View(model);
            }
        }
        public void ExportExcel_E911Data()
        {
            var list = (List<SocE911Model>)Session["ExportData"];
            var grid = new System.Web.UI.WebControls.GridView();

            grid.DataSource = list;
            grid.DataBind();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=SOC_E911.xls");
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }

    }
}