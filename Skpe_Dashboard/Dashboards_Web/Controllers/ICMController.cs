using Dashboards_BusinessLayer;
using Dashboards_Web.Models;
using Dashboards_Web.Utilities;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web.Mvc;

namespace Dashboards_Web.Controllers
{
    public class ICMController : Controller
    {
        readonly IICMDataHandler iCMDataHandler;
        string thumbprint = "";
        public ICMController(IICMDataHandler iCMDataHandler)
        {
            this.iCMDataHandler = iCMDataHandler;
            thumbprint = ConfigurationManager.AppSettings["Thumbprint"];
        }
        // GET: ICM
        public ActionResult SREIncidents(IcmDataModel model, string Days = "90")
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
                FillModel(ref model, Days, "SRE", ref kustoErr);
            }
            
            ViewBag.TotalCount = model.TtaList.Count() + model.TteList.Count();
            ViewBag.TriageCount = model.TtaList.Count();
            ViewBag.AsignedCount = model.TteList.Count();
            ViewBag.SEV1Count = model.TtaList.Count(a => a.Severity == 1) + model.TteList.Count(a => a.Severity == 1);
            ViewBag.SEV2Count = model.TtaList.Count(a => a.Severity == 2) + model.TteList.Count(a => a.Severity == 2);
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
        private void FillModel(ref IcmDataModel model, string days, string icmType, ref string kustoError)
        {
            model.TtaList = new List<TTADataModel>();
            model.TteList = new List<TTEDataModel>();
            ViewBag.ICMTP = icmType;
            var incidents = SetIncidentData(days, icmType, ref kustoError);
            var ttaData = iCMDataHandler.GetTTARefinedData(incidents);
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
                    TicketAge = ticketAge,
                    Title = item.Title,
                    TTA = tta,
                    TTACategory = item.TTACategory
                });
            }
            List<ICM_Incidents.IcmIncident> tteData;
            if (icmType == "SD")
                tteData = iCMDataHandler.GetTTEForSd(incidents);
            else
                tteData = iCMDataHandler.GetTTERefinedData(incidents);
            if (tteData != null)
            {
                tteData = tteData.OrderByDescending(c => c.TTACategory).ToList();
                foreach (var item in tteData)
                {
                    double ticketAge = TimeDuration.ConvertTicketAgeDurationToMins(item.Duration);
                    double tte = TimeDuration.ConvertTTETTADurationToMinutes(item.TTA);
                    model.TteList.Add(new TTEDataModel
                    {
                        IncidentNo = Convert.ToInt32(item.Id),
                        CreateDate = Convert.ToDateTime(item.CreateDate),
                        IsCustomer = item.Customer,
                        OwningContactAlias = item.OwningContactAlias,
                        OwningTeam = item.OwningTeamId.Replace("\\", "*").Split('*')[1],
                        TicketAge = ticketAge,
                        Title = item.Title,
                        AcknowledgeDate = Convert.ToDateTime(item.AcknowledgeDate),
                        TTE = tte,
                        Status = item.Status,
                        Severity = item.Severity,
                        TTECategory = item.TTACategory
                    });
                }
            }
        }
        public ActionResult SDIncidents(IcmDataModel model, string Days = "90")
        {
            String kustoErr = string.Empty;
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
                FillModel(ref model, Days, "SD", ref kustoErr);
            }
            ViewBag.TotalCount = model.TtaList.Count() + model.TteList.Count();
            ViewBag.TriageCount = model.TtaList.Count();
            ViewBag.AsignedCount = model.TteList.Count();
            ViewBag.SEV1Count = model.TtaList.Count(a => a.Severity == 1) + model.TteList.Count(a => a.Severity == 1);
            ViewBag.SEV2Count = model.TtaList.Count(a => a.Severity == 2) + model.TteList.Count(a => a.Severity == 2);
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
        public ActionResult ITARIncidents(IcmDataModel model, string Days = "90")
        {
            String kustoErr = string.Empty;
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
                FillModel(ref model, Days, "ITAR", ref kustoErr);
            }
            //FillModel(ref model, Days, "ITAR", ref kustoErr);
            ViewBag.TotalCount = model.TtaList.Count() + model.TteList.Count();
            ViewBag.TriageCount = model.TtaList.Count();
            ViewBag.AsignedCount = model.TteList.Count();
            ViewBag.SEV1Count = model.TtaList.Count(a => a.Severity == 1) + model.TteList.Count(a => a.Severity == 1);
            ViewBag.SEV2Count = model.TtaList.Count(a => a.Severity == 2) + model.TteList.Count(a => a.Severity == 2);
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
                IQueryable<TTADataModel> tta = SortIQueryable<TTADataModel>(model.TtaList.AsQueryable(), model.ttaSort, model.ttaSortDir);
                IQueryable<TTEDataModel> tte = SortIQueryable<TTEDataModel>(model.TteList.AsQueryable(), model.tteSort, model.tteSortDir);
                model.TtaList = tta.ToList();
                model.TteList = tte.ToList();
                ViewBag.KustoReminder = kustoErr;
            }
            Session["DataToSort"] = model;
            return View(model);
        }
        public static IQueryable<T> SortIQueryable<T>(IQueryable<T> data, string fieldName, string sortOrder)
        {
            if (string.IsNullOrWhiteSpace(fieldName)) return data;
            if (string.IsNullOrWhiteSpace(sortOrder)) return data;

            var param = System.Linq.Expressions.Expression.Parameter(typeof(T), "i");
            System.Linq.Expressions.Expression conversion = System.Linq.Expressions.Expression.Convert(System.Linq.Expressions.Expression.Property(param, fieldName), typeof(object));
            var mySortExpression = System.Linq.Expressions.Expression.Lambda<Func<T, object>>(conversion, param);

            return (sortOrder == "desc") ? data.OrderByDescending(mySortExpression)
                : data.OrderBy(mySortExpression);
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
        private List<ICM_Incidents.IcmIncident> SetIncidentData(string noOfDays, string icmTp, ref string kustoError)
        {
            List<ICM_Incidents.IcmIncident> incidentList = new List<ICM_Incidents.IcmIncident>();
            try
            {
                var reqDate = noOfDays == null ? DateTime.UtcNow.AddDays(-1).ToString("o") : DateTime.UtcNow.AddDays(-Convert.ToInt32(noOfDays)).ToString("o");
                List<string> filter = new List<string>();
                GetConfig(ref filter, icmTp, reqDate);
                foreach (var item in filter)
                {
                    string data = iCMDataHandler.GetIncident(item, thumbprint);
                    var obj = JsonConvert.DeserializeObject(data) as dynamic;
                    var pageOfIncidents = JsonConvert.DeserializeObject<List<ICM_Incidents.IcmIncident>>(obj.value.ToString());
                    incidentList.AddRange(pageOfIncidents);
                }
                if (!GetIncidentDates(ref incidentList, noOfDays, icmTp))
                {
                    kustoError = "Currently Kusto is down. So TTE details will not be available";
                }
            }
            catch (Exception)
            {

            }
            return incidentList;
        }
        private void GetConfig(ref List<string> filter, string icmTp, string reqDate)
        {
            if (icmTp.Equals("SRE"))
            {
                filter.Add(ConfigurationManager.AppSettings["SRESearch"].Replace("@reqDate", reqDate));
                filter.Add(ConfigurationManager.AppSettings["SRESearch2"].Replace("@reqDate", reqDate));
                //filter.Add(ConfigurationManager.AppSettings["incidentSearch3"].Replace("@reqDate", reqDate));
                filter.Add(ConfigurationManager.AppSettings["SRESearch4"].Replace("@reqDate", reqDate));
                filter.Add(ConfigurationManager.AppSettings["SRESearch5"].Replace("@reqDate", reqDate));
                filter.Add(ConfigurationManager.AppSettings["SRESearch6"].Replace("@reqDate", reqDate));
                filter.Add(ConfigurationManager.AppSettings["SRESearch7"].Replace("@reqDate", reqDate));
                filter.Add(ConfigurationManager.AppSettings["SRESearch8"].Replace("@reqDate", reqDate));
            }
            else if (icmTp.Equals("ITAR"))
            {
                filter.Add(ConfigurationManager.AppSettings["SDSearch6"].Replace("@reqDate", reqDate));

            }
            else if (icmTp.Equals("SD"))
            {
                filter.Add(ConfigurationManager.AppSettings["SDSearch1"].Replace("@reqDate", reqDate));
                filter.Add(ConfigurationManager.AppSettings["SDSearch2"].Replace("@reqDate", reqDate));
                filter.Add(ConfigurationManager.AppSettings["SDSearch3"].Replace("@reqDate", reqDate));
                filter.Add(ConfigurationManager.AppSettings["SDSearch4"].Replace("@reqDate", reqDate));
                filter.Add(ConfigurationManager.AppSettings["SDSearch5"].Replace("@reqDate", reqDate));
                // filter.Add(ConfigurationManager.AppSettings["SDSearch6"].Replace("@reqDate", reqDate));
            }
        }
        private bool GetIncidentDates(ref List<ICM_Incidents.IcmIncident> incidents, string requDate, string icmType)
        {
            try
            {
                var dataTable = new DataTable();
                string kustoQuery = GetQuery(requDate, icmType);
                dataTable = iCMDataHandler.ExecuteKustoQuery(kustoQuery);
                if (dataTable.Rows.Count > 0)
                {
                    foreach (var item in incidents)
                    {
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            if (dataTable.Rows[i].ItemArray[1].ToString().Trim().Equals(item.Id))
                            {
                                if (!item.OwningTeamId.ToUpper().Contains("BUSINESSVOICEBVD"))
                                {
                                    item.CreateDate = dataTable.Rows[i].ItemArray[2].ToString();
                                    item.AcknowledgeDate = dataTable.Rows[i].ItemArray[3].ToString();
                                }
                                item.Customer = dataTable.Rows[i].ItemArray[5].ToString().ToUpper().Equals("CUSTOMERTYPE") ? "YES" : "NO";
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
        private string GetQuery(string reqDays, string icmType)
        {
            try
            {
                string file;
                string reader = icmType == "SRE" ? $"~/Content/ICM_SRE_Query.txt" : $"~/Content/ICM_ST_Query.txt";
                using (StreamReader readFl = new StreamReader(Server.MapPath(reader)))
                {
                    file = readFl.ReadToEnd();
                }
                return file.Replace("@reqDays", reqDays);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Duplicate Code and it can be optimized
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
    }
}