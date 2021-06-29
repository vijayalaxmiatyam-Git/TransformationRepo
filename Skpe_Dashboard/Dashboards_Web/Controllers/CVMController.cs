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
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Dashboards_Web.Controllers
{
    public class CVMController : Controller
    {
        readonly IICMDataHandler iCMDataHandler;
        string thumbprint = "";
        public CVMController(IICMDataHandler iCMDataHandler)
        {
            this.iCMDataHandler = iCMDataHandler;
            thumbprint = ConfigurationManager.AppSettings["Thumbprint"];
        }
        public ActionResult CvmHome(IcmDataModel model, string Days = "90")
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
                FillModel(ref model, Days, ref kustoErr);
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
        private void FillModel(ref IcmDataModel model, string days, ref string kustoError)
        {
            model.TtaList = new List<TTADataModel>();
            model.TteList = new List<TTEDataModel>();
            var incidents = SetIncidentData(days, ref kustoError);
            var ttaData = iCMDataHandler.GetTTADataForCvm(incidents, "CVM");
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
            var tteData = iCMDataHandler.GetTTEForCvm(incidents, "CVM");
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
                        TTE = tta, /*item.TTA,*/
                        Status = item.Status,
                        Severity = item.Severity,
                        TTECategory = item.TTACategory
                    });
                }

            }
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
        private List<ICM_Incidents.IcmIncident> SetIncidentData(string noOfDays, ref string kustoError)
        {
            List<ICM_Incidents.IcmIncident> incidentList = new List<ICM_Incidents.IcmIncident>();
            try
            {
                var reqDate = noOfDays == null ? DateTime.UtcNow.AddDays(-1).ToString("o") : DateTime.UtcNow.AddDays(-Convert.ToInt32(noOfDays)).ToString("o");
                List<string> filter = new List<string>();
                GetConfig(ref filter, reqDate);
                foreach (var item in filter)
                {
                    string data = iCMDataHandler.GetIncident(item, thumbprint);
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

        private void GetConfig(ref List<string> filter, string reqDate)
        {
            filter.Add(ConfigurationManager.AppSettings["CvmSearch1"].Replace("@reqDate", reqDate));
            filter.Add(ConfigurationManager.AppSettings["CvmSearch2"].Replace("@reqDate", reqDate));
            filter.Add(ConfigurationManager.AppSettings["CvmSearch3"].Replace("@reqDate", reqDate));
        }

        private void GetConfigForMitigated(ref List<string> filter, string fromDate, string toDate)
        {
            filter.Add(ConfigurationManager.AppSettings["CvmMitigated1"].Replace("@FromDate", fromDate).Replace("@ToDate", toDate));
            filter.Add(ConfigurationManager.AppSettings["CvmMitigated2"].Replace("@FromDate", fromDate).Replace("@ToDate", toDate));
            filter.Add(ConfigurationManager.AppSettings["CvmMitigated3"].Replace("@FromDate", fromDate).Replace("@ToDate", toDate));
        }
        private bool GetIncidentDates(ref List<ICM_Incidents.IcmIncident> incidents, string requDate)
        {
            try
            {
                var dataTable = new DataTable();
                string kustoQuery = GetQuery(requDate, $"~/Content/CVMDashboard.txt");
                dataTable = iCMDataHandler.ExecuteKustoQuery(kustoQuery);
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
        private string GetQuery(string reqDays, string path)
        {
            try
            {
                string file;
                //string reader = string.Empty;
                //if (type == "CVMDashboard")
                //    reader = $"~/Content/CVMDashboard.txt";
                //else
                //    reader = $"~/Content/CVMTransferred.txt";
                using (StreamReader readFl = new StreamReader(Server.MapPath(path)))
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

        public ActionResult CvmMitigated(List<CvmMitigatedModel> model, string fDate = "", string tDate = "", string sort = "", string sortDir = "")
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
                    GetConfigForMitigated(ref filter, fDate, tDate);
                    foreach (var item in filter)
                    {
                        string data = iCMDataHandler.GetIncident(item, thumbprint);
                        var obj = JsonConvert.DeserializeObject(data) as dynamic;
                        var pageOfIncidents = JsonConvert.DeserializeObject<List<ICM_Incidents.IcmIncident>>(obj.value.ToString());
                        incidentList.AddRange(pageOfIncidents);
                    }
                    model = new List<CvmMitigatedModel>();
                    if (incidentList.Count() > 0)
                    {
                        iCMDataHandler.GetTTAForCVM(ref incidentList);
                        foreach (var item in incidentList)
                        {
                            CvmMitigatedModel data = new CvmMitigatedModel();
                            data.IncidentId = item.Id;
                            data.MitigatedDate = Convert.ToDateTime(item.MitigationData.Date);
                            data.OwningTeam = item.OwningTeamId.Replace("\\", "*").Split('*')[1];// item.OwningTeamId;
                            data.Severity = item.Severity;
                            data.Status = item.Status;
                            data.Title = item.Title;
                            data.Owner = item.OwningContactAlias;
                            data.MitigatedBy = item.MitigationData.ChangedBy;
                            data.Mitigation = item.MitigationData.Mitigation;
                            data.TTR = item.TTA;
                            data.TtrCategory = item.TTACategory;
                            model.Add(data);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(sortDir))
                {
                    model = (List<CvmMitigatedModel>)Session["ExportData"];
                    if (sort == "TTR")
                    {
                        for (int i = 0; i < model.Count; i++)
                        {
                            model[i].TTR = TimeDuration.ConvertTTETTADurationToMinutes(model[i].TTR.ToString());
                        }
                        IQueryable<CvmMitigatedModel> ttr = SortIQueryable<CvmMitigatedModel>(model.AsQueryable(), sort);
                        model = ttr.ToList();
                        for (int i = 0; i < model.Count; i++)
                        {
                            model[i].TTR = TimeDuration.CovertTTETTAMinsToDuration((double)model[i].TTR).ToString();
                        }
                        var modelTTEList = model;
                        model = null;
                        model = GetTTRSortedData(modelTTEList);
                    }
                    else
                    {
                        var sortedData = SortIQueryable<CvmMitigatedModel>(model.AsQueryable(), sort, sortDir);
                        model = sortedData.ToList();
                    }
                }
                else
                {
                    model = model.OrderByDescending(a => a.MitigatedDate).ToList();
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
        private List<CvmMitigatedModel> GetTTRSortedData(List<CvmMitigatedModel> modelData)
        {
            var overDueList = modelData.Where(x => x.TTR.ToString().ToLower().Contains("overdue")).ToList();
            var leftList = modelData.Where(x => x.TTR.ToString().ToLower().Contains("left")).ToList();
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
        public void ExportExcel_MitigatedData()
        {
            var list = (List<CvmMitigatedModel>)Session["ExportData"];
            var grid = new System.Web.UI.WebControls.GridView();
            foreach (var item in list)
            {
                if (item.TtrCategory == "RED")
                {
                    item.TtrCategory = "Not Met";
                }
                else
                {
                    item.TtrCategory = "With in the SLA";
                }
            }
            grid.DataSource = list;
            grid.DataBind();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=CVM_Mitigated.xls");
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }

        public ActionResult CvmTransferred(List<CvmTransferredModel> model, string fDate = "", string tDate = "", string sort = "", string sortDir = "")
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
                    //List<string> filter = new List<string>();
                    //GetConfigForMitigated(ref filter, fDate, tDate);
                    //foreach (var item in filter)
                    //{
                    //    string data = iCMDataHandler.GetIncident(item, thumbprint);
                    //    var obj = JsonConvert.DeserializeObject(data) as dynamic;
                    //    var pageOfIncidents = JsonConvert.DeserializeObject<List<ICM_Incidents.IcmIncident>>(obj.value.ToString());
                    //    incidentList.AddRange(pageOfIncidents);
                    //}
                    string kustoQuery = GetQuery("", $"~/Content/CVMTransferred.txt");
                    kustoQuery = kustoQuery.Replace("@StartDate", fDate);
                    kustoQuery = kustoQuery.Replace("@EndDate", tDate);
                    DataTable dataTable = iCMDataHandler.ExecuteKustoQuery(kustoQuery);
                    model = new List<CvmTransferredModel>();
                    if (dataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            CvmTransferredModel m = new CvmTransferredModel();
                            m.IncidentId = dataTable.Rows[i].ItemArray[0].ToString();
                            m.TransferComments = dataTable.Rows[i].ItemArray[1].ToString();
                            m.OwningTeam = dataTable.Rows[i].ItemArray[2].ToString();
                            m.TransferredDate = Convert.ToDateTime(dataTable.Rows[i].ItemArray[3]);
                            model.Add(m);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(sortDir))
                {
                    model = (List<CvmTransferredModel>)Session["ExportTrData"];
                    var sortedData = SortIQueryable<CvmTransferredModel>(model.AsQueryable(), sort, sortDir);
                    model = sortedData.ToList();
                }
                else
                {
                    model = model.OrderByDescending(a => a.TransferredDate).ToList();
                }
                Session["ExportTrData"] = model;
                return View(model);
            }
            catch (Exception)
            {
                model = null;
                return View(model);
            }
        }
        public void ExportExcel_TransferredData()
        {
            var list = (List<CvmTransferredModel>)Session["ExportTrData"];
            var grid = new System.Web.UI.WebControls.GridView();
            grid.DataSource = list;
            grid.DataBind();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=CVM_Transferred.xls");
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }
    }
}