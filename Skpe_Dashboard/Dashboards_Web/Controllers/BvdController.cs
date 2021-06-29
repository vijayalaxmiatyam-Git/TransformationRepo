using Dashboards_BusinessLayer.Handlers.Interfaces;
using Dashboards_Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Dashboards_Web.Controllers
{
    public class BvdController : Controller
    {
        readonly IBvdHandler iBvdHandler;
        public BvdController(IBvdHandler iBvdHandler)
        {
            this.iBvdHandler = iBvdHandler;
        }
        // GET: Bvd
        public ActionResult BVDReleases(BvdDataModel model, int id=0)
        {
            List<PipelineModel> pipelineModel = new List<PipelineModel>();
            List<ReleaseModel> rlsModel = new List<ReleaseModel>();

            if (id > 0)
            {
                var releaseData = iBvdHandler.GetReleasesByBuild(id);
                if (releaseData != null && releaseData.Count > 0)
                {
                    var releaseDefData = iBvdHandler.GetReleaseData(releaseData);
                    model.ReleaseModelData = new List<ReleaseModel>();
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
                        data.ReleaseDefId = rlItem.releaseDefinition.id;
                        data.ReleaseId = rlItem.release.id;
                        data.BuilDefdId = rlItem.release.artifacts[0].definitionReference.definition.id;
                        data.BuildefName = rlItem.release.artifacts[0].definitionReference.definition.name;
                        data.CreatedOn = rlItem.queuedOn;
                        model.ReleaseModelData.Add(data);
                    }
                }
                model.PipelineModelData = new List<PipelineModel>();
                model.PipelineModelData = (List<PipelineModel>)Session["pipeline"];
            }
            else
            {
                var pipelineData = iBvdHandler.GetBVDPipelines();
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
                        pipelineModel.Add(data);
                    }
                }

                model.PipelineModelData = new List<PipelineModel>();
                model.PipelineModelData = pipelineModel;
                model.ReleaseModelData = new List<ReleaseModel>();
                Session["pipeline"] = model.PipelineModelData;
            }
            return View(model);
        }
        [NonAction]
        private string GetServiceName(int id)
        {
            string serviceName = "";
            if (id == 7739)
            {
                serviceName = "EERS Release ADORM";
            }
            else if (id == 7614)
            {
                serviceName = "BVD Release ADORM";
            }
            else if (id == 1922)
            {
                serviceName = "BVD Experimental";
            }
            else if (id == 8677)
            {
                serviceName = "EERS TST Experimental";                
            }
            else if (id == 7803)
            {
                serviceName = "BVD E2E Rolling Build";
            }
            else if (id == 8955)
            {
                serviceName = "EERS Rolling Build";
            }
            else if (id == 10449)
            {
                serviceName = "Testing BVD";
            }
            return serviceName;
        }
    }
}