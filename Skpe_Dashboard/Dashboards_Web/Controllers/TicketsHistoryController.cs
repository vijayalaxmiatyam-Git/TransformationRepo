using Dashboards_BusinessLayer;
using Dashboards_Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Dashboards_Web.Controllers
{
    public class TicketsHistoryController : Controller
    {
        // GET: DetainedTickets
        readonly IICMDataHandler iCMDataHandler;
        public TicketsHistoryController(IICMDataHandler iCMDataHandler)
        {
            this.iCMDataHandler = iCMDataHandler;
        }
        public ActionResult TicketsHistory(List<TicketHistoryModel> model, string fDate = "", string tDate = "", string format = "", string tktHstrysort = "", string tktHstrysortDir = "", string team = "")
        {
            try
            {
                ViewData["fromDate"] = fDate;
                ViewData["toDate"] = tDate;
                fDate = string.IsNullOrEmpty(fDate) ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd") : fDate;
                tDate = string.IsNullOrEmpty(tDate) ? DateTime.Now.Date.ToString("yyyy-MM-dd") : tDate;
                if (format.Equals("OwningHistory"))
                {
                    string reader = team == "BVD" ? $"~/Content/BVDOwningHistory.txt" : $"~/Content/TicketOwningHistory.txt";
                    var data = iCMDataHandler.GetRefinedHistoryData(GetDetainedIncident(fDate, tDate, reader), team);
                    model = new List<TicketHistoryModel>();
                    foreach (var item in data)
                    {
                        model.Add(
                        new TicketHistoryModel
                        {
                            IncidentId = Convert.ToInt32(item.Id),
                            ChangeDate = Convert.ToDateTime(item.ModifiedDate),
                            OwningTeam = item.OwningTeamId
                        });
                    }
                    ViewBag.DataItems = team == "BVD" ? "BusinessVoiceBVD and IC3RuntimeStoreTriage teams incidents" : "IC3ProvisioningTriage and RTCAPPSRE teams incidents";
                    ViewBag.OwningHistory = "OwningHistory";
                }
                else if (format.Equals("Severity2"))
                {
                    string reader = $"~/Content/Sev2Incidents.txt";
                    var data = GetDetainedIncident(fDate, tDate, reader);
                    model = new List<TicketHistoryModel>();
                    model = (from DataRow dt in data.Rows
                             select new TicketHistoryModel
                             {
                                 IncidentId = Convert.ToInt32(dt["IncidentId"]),
                                 CreateDate = Convert.ToDateTime(dt["CreateDate"]),
                                 ChangeDate = Convert.ToDateTime(dt["ModifiedDate"]),
                                 OwningTeam = dt["OwningTeamName"].ToString().Replace("\\", "*").Split('*')[1],
                                 Severity = Convert.ToInt32(dt["Severity"]),
                                 Status = dt["Status"].ToString(),
                                 Title = dt["Title"].ToString()
                             }).ToList();
                    ViewBag.Sev2 = "Sev2";
                }

                else
                {
                    string reader = $"~/Content/DetainedTickets.txt";
                    var data = GetDetainedIncident(fDate, tDate, reader);
                    model = (from DataRow dt in data.Rows
                             select new TicketHistoryModel
                             {
                                 IncidentId = Convert.ToInt32(dt["IncidentId"]),
                                 CreateDate = Convert.ToDateTime(dt["CreateDate"]),
                                 ChangeDate = Convert.ToDateTime(dt["ChangeDate"]),
                                 Format = dt["Format"].ToString(),
                                 OwningTeam = dt["OwningTeamName"].ToString().Replace("\\", "*").Split('*')[1],
                                 Severity = Convert.ToInt32(dt["Severity"])
                             }).ToList();
                    model = string.IsNullOrEmpty(format) || format.Equals("All") ? model : model.Where(a => a.Format.Contains(format)).ToList();
                }

                if (!(string.IsNullOrEmpty(tktHstrysort) && string.IsNullOrEmpty(tktHstrysort)))
                {
                    IQueryable<TicketHistoryModel> sorted = SortIQueryable<TicketHistoryModel>(model.AsQueryable(), tktHstrysort, tktHstrysortDir);
                    model = sorted.ToList();
                }
                Session["ExportData"] = model;
                return View(model);
            }
            catch (Exception ex)
            {
                model = null;
                return View(model);
            }
        }

        private DataTable GetDetainedIncident(string fromDate, string toDate, string readerPath)
        {
            try
            {
                var dataTable = new DataTable();
                string file;

                using (StreamReader readFl = new StreamReader(Server.MapPath(readerPath)))
                {
                    file = readFl.ReadToEnd();
                }
                file = file.Replace("@fromDate", fromDate);
                file = file.Replace("@toDate", toDate);
                return iCMDataHandler.ExecuteKustoQuery(file);
            }
            catch (Exception)
            {

                throw;
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

        public void ExportCSV_HistoryData()
        {
            var sb = new StringBuilder();

            var list = (List<TicketHistoryModel>)Session["ExportData"];
            sb.AppendFormat("{0},{1},{2},{3},{4},{5}", "Incident No.", "Severity", "Owner ", "Created Date", "Change Date", "Format", Environment.NewLine);
            foreach (var item in list)
            {
                sb.AppendFormat("{0},{1},{2},{3},{4},{5}", item.IncidentId, item.Severity, item.OwningTeam, item.CreateDate, item.ChangeDate, item.Format, Environment.NewLine);
            }
            //Get Current Response  
            var response = System.Web.HttpContext.Current.Response;
            response.BufferOutput = true;
            response.Clear();
            response.ClearHeaders();
            response.ContentEncoding = Encoding.Unicode;
            response.AddHeader("content-disposition", "attachment;filename=TicketHistory.CSV ");
            response.ContentType = "text/plain";
            response.Write(sb.ToString());
            response.End();
        }
        public void ExportExcel_HistoryData()
        {
            var sb = new StringBuilder();
            var list = (List<TicketHistoryModel>)Session["ExportData"];
            var grid = new System.Web.UI.WebControls.GridView();
            //var histList = (from h in list select new {h.IncidentId,h.OwningTeam,h.ChangeDate }.ToString()).ToList();
            grid.DataSource = list;
            grid.DataBind();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=TicketHistory.xls");
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }
    }
}