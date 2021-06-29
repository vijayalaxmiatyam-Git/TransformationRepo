using Dashboards_BusinessLayer.Handlers.Interfaces;
using Dashboards_BusinessLayer.Models;
using Dashboards_Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dashboards_Web.Controllers
{
    public class Ic3EdgeController : Controller
    {
        readonly IIc3EdgeHandler iIc3Handler;
        public Ic3EdgeController(IIc3EdgeHandler iIc3Handler)
        {
            this.iIc3Handler = iIc3Handler;
        }

        //public ActionResult Ic3TestUpdate()
        //{
        //    return View();
        //}

        //[HttpPost]
        public ActionResult Ic3TestUpdate(Ic3EdgeModel ic3EdgeModel)
        {
            try
            {
                if (ic3EdgeModel.TestPlanId > 0 && ic3EdgeModel.TestSuiteId > 0)
                {
                    ic3EdgeModel.TestCaseData = new List<TestCaseModel>();
                    ic3EdgeModel.TestDbData = new List<TestCaseModel>();
                    GetVSOTestData(ref ic3EdgeModel);
                    GetDBTestData(ref ic3EdgeModel);
                }
                return View(ic3EdgeModel);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error occurred while fetching data." + ex.Message;
                return View(ic3EdgeModel);
            }
        }
        private void GetVSOTestData(ref Ic3EdgeModel ic3EdgeModel)
        {
            try
            {
                var testPortalData = iIc3Handler.GetTestCaseDetais(ic3EdgeModel.TestPlanId, ic3EdgeModel.TestSuiteId);
                if (testPortalData.Count > 0)
                {
                    Session["VSOData"] = testPortalData;
                    ic3EdgeModel.TestCaseData = new List<TestCaseModel>();
                    foreach (var item in testPortalData)
                    {
                        TestCaseModel model = new TestCaseModel();
                        //model.IsActive = item.isActive;
                        model.LastResetToActive = item.lastResetToActive;
                        model.LastUpdatedBy = item.lastUpdatedBy.displayName;
                        model.TesterName = item.tester.displayName;
                        model.TestId = item.testCaseReference.id;
                        //model.TestPointId = item.testCaseReference.id;
                        model.Result = item.results.outcome;
                        model.LastUpdatedDate = item.lastUpdatedDate;
                        model.Title = item.testCaseReference.name;
                        ic3EdgeModel.TestCaseData.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetDBTestData(ref Ic3EdgeModel ic3EdgeModel)
        {
            try
            {
                var dbData = iIc3Handler.GetTestcasesFromDB(ic3EdgeModel.TestSuiteId);
                if (dbData.Rows != null && dbData.Rows.Count > 0)
                {
                    Session["DbData"] = dbData;
                    ic3EdgeModel.TestDbData = new List<TestCaseModel>();
                    foreach (DataRow item in dbData.Rows)
                    {
                        TestCaseModel model = new TestCaseModel();
                        model.TestId = Convert.ToInt32(item["TestCaseID"]);
                        model.Title = item["Title"].ToString();
                        model.Result = item["Result"].ToString();
                        model.Path = item["Path"].ToString();
                        model.LastUpdatedDate = Convert.ToDateTime(item["updated"]);
                        model.IsProcessed = Convert.ToBoolean(item["IsProcessed"]);
                        ic3EdgeModel.TestDbData.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult UpdateTests(Ic3EdgeModel ic3EdgeModel)
        {
            try
            {
                var dbData = (DataTable)Session["DbData"];
                var vsoData = (List<TestPoints.Value>)Session["VSOData"];
                bool success = iIc3Handler.UpdateTestCaseDetais(dbData, vsoData);
                ic3EdgeModel.TestPlanId = vsoData.First().testPlan.id;
                ic3EdgeModel.TestSuiteId = vsoData.First().testSuite.id;
                //GetVSOTestData(ref ic3EdgeModel);
                //GetDBTestData(ref ic3EdgeModel);
                return RedirectToAction("Ic3TestUpdate",ic3EdgeModel);
            }
            catch (Exception)
            {
                return RedirectToAction("Ic3TestUpdate", ic3EdgeModel);
            }
        }

        public ActionResult Ic3BuildReleases(Ic3EdgeModel model, int id = 0,string name="")
        {
            List<PipelineModel> pipelineModel = new List<PipelineModel>();
            List<ReleaseModel> rlsModel = new List<ReleaseModel>();
            
            if (id > 0)
            {
                var releaseData = iIc3Handler.GetReleasesByBuild(id);
                if (releaseData != null && releaseData.Count > 0)
                {
                    
                    var releaseDefData = iIc3Handler.GetReleaseData(releaseData);
                    model.Ic3ReleaseDefData = new List<ReleaseModel>();
                    ViewBag.Name = releaseDefData.ToArray()[0].releaseDefinition.name;
                    foreach (var rlItem in releaseDefData)
                    {
                        ReleaseModel data = new ReleaseModel();
                        data.Id = rlItem.id;
                        data.CompletedOn = rlItem.completedOn;
                        data.LastModifiedOn = rlItem.lastModifiedOn;
                        data.LastModifiedBy = rlItem.lastModifiedBy.displayName;
                        data.RequestedBy = rlItem.requestedBy.displayName;
                        data.DeploymentStatus = rlItem.deploymentStatus;
                        data.Path = rlItem.releaseDefinition.path;
                        data.Name = rlItem.releaseDefinition.name;
                        data.ReleaseId = rlItem.release.id;
                        data.ReleaseDefId = rlItem.releaseDefinition.id;
                        data.BuilDefdId = rlItem.release.artifacts[0].definitionReference.definition.id;
                        data.BuildefName = rlItem.release.artifacts[0].definitionReference.definition.name;
                        data.CreatedOn = rlItem.queuedOn;
                        model.Ic3ReleaseDefData.Add(data);
                    }
                }

                model.Ic3ReleaseData = new List<ReleaseModel>();
                model.Ic3ReleaseData = rlsModel;
                model.Ic3BuildData = new List<PipelineModel>();
                model.Ic3BuildData = (List<PipelineModel>)Session["pipeline"];
            }
            else
            {
                var pipelineData = iIc3Handler.GetIc3EdgeBuildData();
                if (pipelineData != null && pipelineData.Count > 0)
                {
                    foreach (var item in pipelineData)
                    {
                        PipelineModel data = new PipelineModel();
                        data.BuildId = item.id;
                        data.finishTime = item.finishTime;
                        data.LastChangedDate = item.lastChangedDate;
                        data.LastChangedBy = item.lastChangedBy.displayName;
                        data.requestedBy = item.requestedBy.displayName;
                        data.priority = item.priority;
                        data.Description = item.project.description;
                        data.PipelineName = item.definition.name;
                        data.result = item.result;
                        data.BuildNumber = item.buildNumber;
                        data.Path = item.definition.path;
                        data.DefinitionId = item.definition.id;
                        data.DefinitionName = item.definition.name;
                        data.ServiceName = GetServiceName(item.definition.id);
                        data.CrntDu = GetDu(data.ServiceName);
                        data.Environment = GetEnvironment(data.ServiceName);
                        pipelineModel.Add(data);
                    }
                }
                model.Ic3BuildData = new List<PipelineModel>();
                model.Ic3BuildData = pipelineModel;
                model.Ic3ReleaseData = new List<ReleaseModel>();
                Session["pipeline"] = model.Ic3BuildData;
            }
            return View(model);
        }

        [NonAction]
        private string GetServiceName(int id)
        {
            string serviceName = "";
            if (id == 3594)
            {
                serviceName = "Userpool BE";
            }
            else if (id == 3565)
            {
                serviceName = "CGW";
            }
            else if (id == 3919)
            {
                serviceName = "LGW";
            }
            else if (id == 4126)
            {
                serviceName = "Device Pin Auth";
            }
            return serviceName;
        }
        private string GetDu(string servName)
        {
            string du = "";
            if (servName == "Userpool BE" || servName == "CGW")
            {
                du = "DU-A";
            }
            else if (servName == "LGW")
            {
                // Dont know yet
            }
            else if (servName == "Device Pin Auth")
            {
                du = "N/A";
            }
            return du;
        }
        private string GetEnvironment(string servName)
        {
            string environment = "";
            if (servName == "Userpool BE" || servName == "CGW" || servName == "Device Pin Auth")
            {
                environment = "Public Cloud";
            }
            else if (servName == "LGW")
            {
                // Dont know yet
            }
            
            return environment;
        }
    }
}