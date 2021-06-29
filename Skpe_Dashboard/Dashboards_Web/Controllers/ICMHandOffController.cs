using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dashboards_BusinessLayer;
using Dashboards_Web.Models;
using Newtonsoft.Json;

namespace Dashboards_Web.Controllers
{
    public class ICMHandOffController : Controller
    {
        // GET: ICMHandOff
        readonly IICMDataHandler iCMDataHandler;
        string thumbprint = "";
        public ICMHandOffController(IICMDataHandler iCMDataHandler)
        {
            this.iCMDataHandler = iCMDataHandler;
            thumbprint = ConfigurationManager.AppSettings["Thumbprint"];
        }
        public ActionResult HandOffReport(IcmDataModel model, string Severity, string Status, string TeamName, string IsCarry = "NO")
        {
            List<string> filter = new List<string>();
            bool flag = false;
            model.IcmType = "CVM";
            IcmDataModel newModel = new IcmDataModel();
            if (!string.IsNullOrEmpty(model.ttaSort) && !string.IsNullOrEmpty(model.ttaSortDir))
            {
                var temp = (IcmDataModel)Session["ModelData"];
                newModel.Shift = temp.Shift;
                newModel.IcmType = temp.IcmType;
                newModel.AllTrackCvm = temp.AllTrackCvm;
                newModel.BVLis = temp.BVLis;
                newModel.BVOrRouting = temp.BVOrRouting;
                newModel.SBVVa = temp.SBVVa;
                newModel.TtaList = temp.TtaList;
                newModel.TeamList = temp.TeamList;
                newModel.ShiftInfo = temp.ShiftInfo;
                newModel.TeamList = temp.TeamList;
                newModel.TtaList = temp.TtaList;
                IQueryable<TTADataModel> tta = SortIQueryable<TTADataModel>(newModel.TtaList.AsQueryable(), model.ttaSort, model.ttaSortDir);
                newModel.TtaList = tta.ToList();
                ModelState.Remove("AllTrackCvm");
                ModelState.Remove("SBVVa");
                ModelState.Remove("BVOrRouting");
                ModelState.Remove("BVLis");
            }

            if (string.IsNullOrEmpty(Severity) && string.IsNullOrEmpty(Status) && string.IsNullOrEmpty(TeamName) && string.IsNullOrEmpty(model.ttaSortDir))
            {
                Session["ModelData"] = null;
            }
            else if (!string.IsNullOrEmpty(model.ttaSortDir) || (!string.IsNullOrEmpty(Severity) && !string.IsNullOrEmpty(Status) && !string.IsNullOrEmpty(TeamName)))
            {
                flag = true;
                var temp = (IcmDataModel)Session["ModelData"];
                model.Shift = temp.Shift;
                model.IcmType = temp.IcmType;
                model.AllTrackCvm = temp.AllTrackCvm;
                model.BVLis = temp.BVLis;
                model.BVOrRouting = temp.BVOrRouting;
                model.SBVVa = temp.SBVVa;
                model.TtaList = temp.TtaList;
                model.TeamList = temp.TeamList;
                model.ShiftInfo = temp.ShiftInfo;
            }
            if (!string.IsNullOrEmpty(model.Shift) && !string.IsNullOrEmpty(model.IcmType) && flag == false)
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
                    string data = iCMDataHandler.GetIncident(i, thumbprint);
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
                ViewBag.IcmType = model.IcmType;
                Session["IcmType"] = model.IcmType;
                ViewBag.GrandTotal = model.TeamList.Sum(a => a.TeamTotal);
                //if (model.IcmType == "CVM")
                model.HandOffEmail = ConfigurationManager.AppSettings["CvmHandoffEmail"];
                //else if (model.IcmType == "SRE")
                //    model.HandOffEmail = ConfigurationManager.AppSettings["SREHandoffEmail"];
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
            //}
            if (string.IsNullOrEmpty(model.ttaSort) && string.IsNullOrEmpty(model.ttaSortDir) && newModel.TtaList != null && newModel.TtaList.Count() > 1)
            {
                newModel.TtaList = newModel.TtaList.OrderByDescending(a => a.ModifiedDate).ToList();
            }
            return View(newModel);
        }

        public ActionResult SDHandOffReport(IcmDataModel model, string Severity, string Status, string TeamName, string IsCarry = "NO")
        {
            List<string> filter = new List<string>();
            bool flag = false;
            model.IcmType = "SD";
            IcmDataModel newModel = new IcmDataModel();
            if (!string.IsNullOrEmpty(model.ttaSort) && !string.IsNullOrEmpty(model.ttaSortDir))
            {
                var temp = (IcmDataModel)Session["ModelData"];
                newModel.Shift = temp.Shift;
                newModel.IcmType = temp.IcmType;
                newModel.SFBMindtreeDeliveryTeam = temp.SFBMindtreeDeliveryTeam;
                newModel.SFBMindtreeSD = temp.SFBMindtreeSD;
                newModel.SFBUserMoves = temp.SFBUserMoves;
                newModel.SkypeMindtreeDelivery = temp.SkypeMindtreeDelivery;
                newModel.AllTrackSd = temp.AllTrackSd;
                newModel.ServiceDelivery = temp.ServiceDelivery;
                newModel.TtaList = temp.TtaList;
                newModel.TeamList = temp.TeamList;
                newModel.ShiftInfo = temp.ShiftInfo;
                newModel.TeamList = temp.TeamList;
                newModel.TtaList = temp.TtaList;
                IQueryable<TTADataModel> tta = SortIQueryable<TTADataModel>(newModel.TtaList.AsQueryable(), model.ttaSort, model.ttaSortDir);
                newModel.TtaList = tta.ToList();

                ModelState.Remove("AllTrackSd");
                ModelState.Remove("ServiceDelivery");
                ModelState.Remove("SFBUserMoves");
                ModelState.Remove("SFBMindtreeDeliveryTeam");
                ModelState.Remove("SFBMindtreeSD");
                ModelState.Remove("SkypeMindtreeDelivery");
            }

            if (string.IsNullOrEmpty(Severity) && string.IsNullOrEmpty(Status) && string.IsNullOrEmpty(TeamName) && string.IsNullOrEmpty(model.ttaSortDir))
            {
                Session["ModelData"] = null;
            }
            else if (!string.IsNullOrEmpty(model.ttaSortDir) || (!string.IsNullOrEmpty(Severity) && !string.IsNullOrEmpty(Status) && !string.IsNullOrEmpty(TeamName)))
            {
                flag = true;
                var temp = (IcmDataModel)Session["ModelData"];
                model.Shift = temp.Shift;
                model.IcmType = temp.IcmType;
                model.SFBMindtreeDeliveryTeam = temp.SFBMindtreeDeliveryTeam;
                model.SFBMindtreeSD = temp.SFBMindtreeSD;
                model.SFBUserMoves = temp.SFBUserMoves;
                model.SkypeMindtreeDelivery = temp.SkypeMindtreeDelivery;
                model.AllTrackSd = temp.AllTrackSd;
                model.ServiceDelivery = temp.ServiceDelivery;
                model.TtaList = temp.TtaList;
                model.TeamList = temp.TeamList;
                model.ShiftInfo = temp.ShiftInfo;
            }
            if (!string.IsNullOrEmpty(model.Shift) && !string.IsNullOrEmpty(model.IcmType) && flag == false)
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
                    string data = iCMDataHandler.GetIncident(i, thumbprint);
                    var obj = JsonConvert.DeserializeObject(data) as dynamic;
                    var pageOfIncidents = JsonConvert.DeserializeObject<List<ICM_Incidents.IcmIncident>>(obj.value.ToString());
                    incidentList.AddRange(pageOfIncidents);
                    FillModel(isCarry, ref model, incidentList);
                }
                string kustoError = "";

                GetEffortData(model.IcmType, ref model, ref kustoError);
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
                ViewBag.IcmType = model.IcmType;
                Session["IcmType"] = model.IcmType;
                ViewBag.GrandTotal = model.TeamList.Sum(a => a.TeamTotal);

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
            return View(newModel);
        }

        public ActionResult SREHandOffReport(IcmDataModel model, string Severity, string Status, string TeamName, string IsCarry = "NO")
        {
            List<string> filter = new List<string>();
            bool flag = false;
            model.IcmType = "SRE";
            IcmDataModel newModel = new IcmDataModel();
            if (!string.IsNullOrEmpty(model.ttaSort) && !string.IsNullOrEmpty(model.ttaSortDir))
            {
                var temp = (IcmDataModel)Session["ModelData"];
                newModel.Shift = temp.Shift;
                newModel.IcmType = temp.IcmType;
                newModel.IsIc3Prov = temp.IsIc3Prov;
                newModel.IsRtcAppSre = temp.IsRtcAppSre;
                newModel.IsIc3RunTime = temp.IsIc3RunTime;
                newModel.IsGpa = temp.IsGpa;
                newModel.IsIC3AdminControls = temp.IsIC3AdminControls;
                newModel.IsIC3TenantPolicies = temp.IsIC3TenantPolicies;
                newModel.IsTeamsLROS = temp.IsTeamsLROS;
                newModel.AllTrackSre = temp.AllTrackSre;
                newModel.TtaList = temp.TtaList;
                newModel.TeamList = temp.TeamList;
                newModel.ShiftInfo = temp.ShiftInfo;
                newModel.TeamList = temp.TeamList;
                newModel.TtaList = temp.TtaList;                
                IQueryable<TTADataModel> tta = SortIQueryable<TTADataModel>(newModel.TtaList.AsQueryable(), model.ttaSort, model.ttaSortDir);
                newModel.TtaList = tta.ToList();
                ModelState.Remove("AllTrackSre");
                ModelState.Remove("IsIc3Prov");
                ModelState.Remove("IsRtcAppSre");
                ModelState.Remove("IsIc3RunTime");
                ModelState.Remove("IsGpa");
                ModelState.Remove("IsIC3AdminControls");
                ModelState.Remove("IsIC3TenantPolicies");
                ModelState.Remove("IsTeamsLROS");
                //return View(newModel);
            }

            if (string.IsNullOrEmpty(Severity) && string.IsNullOrEmpty(Status) && string.IsNullOrEmpty(TeamName) && string.IsNullOrEmpty(model.ttaSortDir))
            {
                Session["ModelData"] = null;
            }
            else if (!string.IsNullOrEmpty(model.ttaSortDir) || (!string.IsNullOrEmpty(Severity) && !string.IsNullOrEmpty(Status) && !string.IsNullOrEmpty(TeamName)))
            {
                flag = true;
                var temp = (IcmDataModel)Session["ModelData"];
                model.Shift = temp.Shift;
                model.IcmType = temp.IcmType;
                model.IsIc3Prov = temp.IsIc3Prov;
                model.IsRtcAppSre = temp.IsRtcAppSre;
                model.IsIc3RunTime = temp.IsIc3RunTime;
                model.AllTrackSre = temp.AllTrackSre;
                model.TtaList = temp.TtaList;
                model.TeamList = temp.TeamList;
                model.ShiftInfo = temp.ShiftInfo;
            }
            if (!string.IsNullOrEmpty(model.Shift) && !string.IsNullOrEmpty(model.IcmType) && flag == false)
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
                    string data = iCMDataHandler.GetIncident(i, thumbprint);
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
                ViewBag.IcmType = model.IcmType;
                Session["IcmType"] = model.IcmType;
                ViewBag.GrandTotal = model.TeamList.Sum(a => a.TeamTotal);
                model.HandOffEmail = ConfigurationManager.AppSettings["SREHandoffEmail"];
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
            Session["ModelData"] = newModel;
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

        private void GetEffortData(string icmType, ref IcmDataModel model, ref string kustoError)
        {
            try
            {
                string file;
                string reader = $"~/Content/Efforts.txt";

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
                    dataTable = iCMDataHandler.ExecuteKustoQuery(file);
                    ExtractData(dataTable, ref model);
                }
            }
            catch (Exception)
            {
                kustoError = "Currently Kusto is unable to bring the Efforts and ICM comments data";
            }
        }

        private void ExtractData(DataTable data, ref IcmDataModel model)
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
        private void FillModel(bool isCarryForward, ref IcmDataModel model, List<ICM_Incidents.IcmIncident> incidentList)
        {
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
                        TicketAge = iCMDataHandler.GetDuration(DateTime.UtcNow.Subtract(Convert.ToDateTime(item.CreateDate)))
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
                        TicketAge = iCMDataHandler.GetDuration(DateTime.UtcNow.Subtract(Convert.ToDateTime(item.CreateDate)))
                    });
                }
            }
        }
        private void GetConfig(ref List<string> filter, ref IcmDataModel model)
        {
            string shift = string.Empty;
            string dateTm = iCMDataHandler.GetShiftData(model.Shift, ref shift);
            model.ShiftInfo = shift;
            string Config;

            if (model.IcmType == "SRE")
            {
                model.TeamList = new List<Team>();
                //if (model.IsBusinessBVD)
                //{
                //    Config = ConfigurationManager.AppSettings["HandOffReport"];
                //    Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\BusinessVoiceBVD");
                //    model.TeamList.Add(new Team { TeamName = "Business Voice BVD", TeamVal = "BusinessVoiceBVD" });
                //    filter.Add(Config);
                //}
                if (model.IsRtcAppSre)
                {
                    Config = ConfigurationManager.AppSettings["HandOffReport"];
                    Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\RTCAPPSRE");
                    model.TeamList.Add(new Team { TeamName = "RTCAPPSRE", TeamVal = "RTCAPPSRE", FriendlyName = "RTCAPPSRE" });
                    filter.Add(Config);
                    Config = ConfigurationManager.AppSettings["CarryForward"];
                    Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\RTCAPPSRE");
                    filter.Add(Config);
                }
                if (model.IsIc3Prov)
                {
                    Config = ConfigurationManager.AppSettings["HandOffReport"];
                    Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\IC3ProvisioningTriage");
                    model.TeamList.Add(new Team { TeamName = "IC3 Provisioning Triage", TeamVal = "IC3ProvisioningTriage", FriendlyName = "IC3 Prov" });
                    filter.Add(Config);
                    Config = ConfigurationManager.AppSettings["CarryForward"];
                    Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\IC3ProvisioningTriage");
                    filter.Add(Config);
                }
                if (model.IsIc3RunTime)
                {
                    Config = ConfigurationManager.AppSettings["HandOffReport"];
                    Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\IC3RuntimeStoreTriage");
                    model.TeamList.Add(new Team { TeamName = "IC3 Runtime Store Triage", TeamVal = "IC3RuntimeStoreTriage", FriendlyName = "IC3 Runtime ST" });
                    filter.Add(Config);
                    Config = ConfigurationManager.AppSettings["CarryForward"];
                    Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\IC3RuntimeStoreTriage");
                    filter.Add(Config);
                }
                if (model.IsGpa)
                {
                    Config = ConfigurationManager.AppSettings["HandOffReport"];
                    Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\GPA");
                    model.TeamList.Add(new Team { TeamName = "GPA", TeamVal = "GPA", FriendlyName = "GPA" });
                    filter.Add(Config);
                    Config = ConfigurationManager.AppSettings["CarryForward"];
                    Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\GPA");
                    filter.Add(Config);
                }
                if (model.IsIC3TenantPolicies)
                {
                    Config = ConfigurationManager.AppSettings["HandOffReport"];
                    Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\IC3TenantPolicies");
                    model.TeamList.Add(new Team { TeamName = "IC3 Tenant Policies", TeamVal = "IC3TenantPolicies", FriendlyName = "IC3 Tenant Policies" });
                    filter.Add(Config);
                    Config = ConfigurationManager.AppSettings["CarryForward"];
                    Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\IC3TenantPolicies");
                    filter.Add(Config);
                }
                if (model.IsTeamsLROS)
                {
                    Config = ConfigurationManager.AppSettings["HandOffReport"];
                    Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\TeamsLROS");
                    model.TeamList.Add(new Team { TeamName = "Teams LROS", TeamVal = "TeamsLROS", FriendlyName = "Teams LROS" });
                    filter.Add(Config);
                    Config = ConfigurationManager.AppSettings["CarryForward"];
                    Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\TeamsLROS");
                    filter.Add(Config);
                }
                if (model.IsIC3AdminControls)
                {
                    Config = ConfigurationManager.AppSettings["HandOffReport"];
                    Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\IC3AdminControls");
                    model.TeamList.Add(new Team { TeamName = "IC3 Admin Controls", TeamVal = "IC3AdminControls", FriendlyName = "IC3 Admin Controls" });
                    filter.Add(Config);
                    Config = ConfigurationManager.AppSettings["CarryForward"];
                    Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\IC3AdminControls");
                    filter.Add(Config);
                }
            }
            else if (model.IcmType == "SD")
            {
                model.TeamList = new List<Team>();
                if (model.ServiceDelivery)
                {
                    Config = ConfigurationManager.AppSettings["HandOffReport"];
                    Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\ServiceDelivery");
                    model.TeamList.Add(new Team { TeamName = "Service Delivery", TeamVal = "ServiceDelivery", FriendlyName = "SD" });
                    filter.Add(Config);
                    Config = ConfigurationManager.AppSettings["CarryForward"];
                    Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\ServiceDelivery");
                    filter.Add(Config);
                }
                if (model.SFBUserMoves)
                {
                    Config = ConfigurationManager.AppSettings["HandOffReport"];
                    Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\SFBUserMoves");
                    model.TeamList.Add(new Team { TeamName = "SFB User Moves", TeamVal = "SFBUserMoves", FriendlyName = "User Moves" });
                    filter.Add(Config);
                    Config = ConfigurationManager.AppSettings["CarryForward"];
                    Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\SFBUserMoves");
                    filter.Add(Config);
                }
                if (model.SFBMindtreeDeliveryTeam)
                {
                    Config = ConfigurationManager.AppSettings["HandOffReport"];
                    Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\SFBMindtreeDeliveryTeam");
                    model.TeamList.Add(new Team { TeamName = "SFB Mindtree Delivery Team", TeamVal = "SFBMindtreeDeliveryTeam", FriendlyName = "Mindtree DT" });
                    filter.Add(Config);
                    Config = ConfigurationManager.AppSettings["CarryForward"];
                    Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\SFBMindtreeDeliveryTeam");
                    filter.Add(Config);
                }
                if (model.SFBMindtreeSD)
                {
                    Config = ConfigurationManager.AppSettings["HandOffReport"];
                    Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\SFBMindtreeSD");
                    model.TeamList.Add(new Team { TeamName = "SFB Mindtree SD", TeamVal = "SFBMindtreeSD", FriendlyName = "Mindtree SD" });
                    filter.Add(Config);
                    Config = ConfigurationManager.AppSettings["CarryForward"];
                    Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\SFBMindtreeSD");
                    filter.Add(Config);
                }
                if (model.SkypeMindtreeDelivery)
                {
                    Config = ConfigurationManager.AppSettings["HandOffReport"];
                    Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\SkypeMindtreeDelivery");
                    model.TeamList.Add(new Team { TeamName = "Skype Mindtree Delivery", TeamVal = "SkypeMindtreeDelivery", FriendlyName = "Skype Mindtree Del" });
                    filter.Add(Config);
                    Config = ConfigurationManager.AppSettings["CarryForward"];
                    Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\SkypeMindtreeDelivery");
                    filter.Add(Config);
                }
            }
            else if (model.IcmType == "CVM")
            {
                model.TeamList = new List<Team>();
                if (model.SBVVa)
                {
                    Config = ConfigurationManager.AppSettings["CvmHandOffReport"];
                    Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\SBVVoiceApplications");
                    model.TeamList.Add(new Team { TeamName = "SBV Voice Applications", TeamVal = "SBVVoiceApplications", FriendlyName = "SBV VA" });
                    filter.Add(Config);
                    Config = ConfigurationManager.AppSettings["CvmCarryForward"];
                    Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\SBVVoiceApplications");
                    filter.Add(Config);
                }
                if (model.BVOrRouting)
                {
                    Config = ConfigurationManager.AppSettings["CvmHandOffReport"];
                    Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\BusinessVoicePSTNCalling");//'SKYPEFORBUSINESS\\BusinessVoice0RRouting' or OwningTeamId eq 
                    model.TeamList.Add(new Team { TeamName = "Business Voice 0R Routing", TeamVal = "BusinessVoicePSTNCalling", FriendlyName = "BV 0R Routing" });
                    filter.Add(Config);
                    Config = ConfigurationManager.AppSettings["CvmCarryForward"];
                    Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\BusinessVoicePSTNCalling");
                    filter.Add(Config);
                }
                if (model.BVLis)
                {
                    Config = ConfigurationManager.AppSettings["CvmHandOffReport"];
                    Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\BusinessVoiceLIS");
                    model.TeamList.Add(new Team { TeamName = "Business Voice LIS", TeamVal = "BusinessVoiceLIS", FriendlyName = "BV LIS" });
                    filter.Add(Config);
                    Config = ConfigurationManager.AppSettings["CvmCarryForward"];
                    Config = Config.Replace("@Team", "SKYPEFORBUSINESS\\BusinessVoiceLIS");
                    filter.Add(Config);
                }
            }
            filter = filter.Select(a => a.Replace("@FromDate", dateTm.Split('|')[0])).ToList();
            filter = filter.Select(a => a.Replace("@ToDate", dateTm.Split('|')[1])).ToList();

        }
        //private string GetShiftData(string ddlShift, ref IcmDataModel model)
        //{
        //    string dateTime = string.Empty;
        //    string FromDate = "", ToDate = "", FromTm = "", ToTm = "";
        //    TimeZoneInfo US_ZONE = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
        //    if (ddlShift.Contains("SHIFT A"))
        //    {
        //        // India Morning: 5 PM - 1 AM PST sys_created_on>=2018-06-18 17:00:00 And sys_created_on<2018-06-19 01:00:00
        //        FromDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow.AddDays(-1), US_ZONE).ToString("yyyy-MM-dd");
        //        ToDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow, US_ZONE).ToString("yyyy-MM-dd");// + " 09:00:00";
        //        FromTm = "17:00:00";
        //        ToTm = "01:00:00";

        //    }
        //    else if (ddlShift.Contains("SHIFT B"))
        //    {
        //        //Redmond early morning - 1 AM - 9 AM PST - India evening
        //        FromDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow, US_ZONE).ToString("yyyy-MM-dd");// + " 09:00:00";
        //        ToDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow, US_ZONE).ToString("yyyy-MM-dd");// + " 17:00:00";
        //        FromTm = "01:00:00";
        //        ToTm = "09:00:00";
        //    }
        //    else if (ddlShift.Contains("SHIFT C"))
        //    {
        //        FromDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow, US_ZONE).ToString("yyyy-MM-dd");// + " 01:00:00";
        //        ToDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow, US_ZONE).ToString("yyyy-MM-dd");// + " 01:00:00";
        //        FromTm = "09:00:00";
        //        ToTm = "17:00:00";
        //        //dateTime = From + "T17:00:00Z|" + To + "T01:00:00Z";
        //    }
        //    model.ShiftInfo = FromDate + ", " + FromTm + " To " + ToTm;
        //    dateTime = FromDate + "T" + FromTm + "Z|" + ToDate + "T" + ToTm + "Z";
        //    return dateTime;
        //}
    }
}