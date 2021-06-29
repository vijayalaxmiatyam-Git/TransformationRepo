using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.OleDb;
using Dashboards_BusinessLayer.Interfaces;
using Dashboards_Web.Models;
using Dashboards_BusinessLayer.Models;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace Dashboards_Web.Controllers
{
    public class TaskController : Controller
    {
        // GET: Task
        readonly ITaskHandler iTaskHandler;
        public TaskController(ITaskHandler iTaskHandler)
        {
            this.iTaskHandler = iTaskHandler;
        }
        public ActionResult Imaging()
        {
            return View();
        }

        [ActionName("Imaging")]
        [HttpPost]
        public ActionResult ImagingData()
        {
            List<TaskModel> taskModel = new List<TaskModel>();
            try
            {
                if (Request.Files["FileUpload1"].ContentLength > 0)
                {
                    string extension = Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
                    string connString = "";
                    string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                    string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), Request.Files["FileUpload1"].FileName);

                    if (!Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                    }
                    if (validFileTypes.Contains(extension))
                    {
                        if (System.IO.File.Exists(path1))
                        {
                            ViewBag.Msg = "Step delete";
                            System.IO.File.Delete(path1); 
                        }
                        Request.Files["FileUpload1"].SaveAs(path1);
                        DataTable data = null;
                        if (extension == ".csv")
                        {
                            data = iTaskHandler.ConvertCSVtoDataTable(path1);
                        }
                        //Connection String to Excel Workbook  
                        else if (extension.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                            data = iTaskHandler.ConvertXSLXtoDataTable(path1, connString);
                        }
                        else if (extension.Trim() == ".xlsx")
                        {
                            connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                            data = iTaskHandler.ConvertXSLXtoDataTable(path1, connString);
                        }

                        if (data != null && data.Rows.Count > 0)
                        {
                            
                            foreach (DataRow item in data.Rows)
                            {
                                if (!string.IsNullOrEmpty(item[0].ToString()))
                                {
                                    TaskModel tasks = new TaskModel();
                                    tasks.Id = Convert.ToInt32(item[0]);
                                    tasks.Title = item[1].ToString();
                                    tasks.Description = item[2].ToString().ToString();
                                    tasks.State = item[3].ToString();
                                    tasks.CreatedDate = item[4].ToString();
                                    tasks.Machines = iTaskHandler.GetMachines(item[2].ToString());
                                    taskModel.Add(tasks);
                                }
                            }
                        }
                    }
                    else
                    {
                        ViewBag.Error = "Please Upload Files in .xls, .xlsx or .csv format";
                    }
                }
                else
                {
                    ViewBag.Error = "Error while uploading the file";
                }
                Session["DumpData"] = taskModel;
                return View(taskModel);
            }
            catch (Exception ex) 
            {
                ViewBag.Error = ex.Message;
                taskModel = null;
                return View(taskModel);
            }
        }

        //[ActionName("Imaging")]
        public JsonResult DumpData()
        {
            try
            {
                List<TaskData> tasks = new List<TaskData>();
                var dataList = (List<TaskModel>)Session["DumpData"];
                foreach (var item in dataList)
                {
                    tasks.Add(new TaskData { Id = item.Id, CreatedDate = item.CreatedDate, Description = item.Description, Machines = item.Machines, State = item.State, Title = item.Title });
                }
                iTaskHandler.AddVsoData(tasks);
                var data = new { Status = dataList.Count()+" Records dumped into database successfully." };
                return Json(data,JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
