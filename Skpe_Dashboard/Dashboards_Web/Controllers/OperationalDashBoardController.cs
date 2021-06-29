using Dashboards_BusinessLayer.Interfaces;
using Dashboards_Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dashboards_Web.Controllers
{
    public class OperationalDashBoardController : Controller
    {
        // GET: OperationalDashBoard
        readonly ISfbHandler SfbDataHandler;
        public OperationalDashBoardController(ISfbHandler SfbDataHandler)
        {
            this.SfbDataHandler = SfbDataHandler;
        }
        public ActionResult OperationalDashBoard(SFBDataModel model)
        {
            model = new SFBDataModel();
            var catData = SfbDataHandler.GetCategory();
            List<SelectListItem> dataList = new List<SelectListItem>();
            foreach (var item in catData)
            {
                SelectListItem cate = new SelectListItem();
                cate.Text = item.Name;
                cate.Value = item.Id.ToString();
                dataList.Add(cate);
            }
            ViewBag.CategoryList = dataList;

            dataList = new List<SelectListItem>();
            var typeData = SfbDataHandler.GetLinkType();
            foreach (var item in typeData)
            {
                SelectListItem type = new SelectListItem();
                type.Text = item.Name;
                type.Value = item.Id.ToString();
                dataList.Add(type);
            }
            ViewBag.LinkType = dataList;

            dataList = new List<SelectListItem>();
            var freqData = SfbDataHandler.GetFrequencyUsedTypes();
            foreach (var item in freqData)
            {
                SelectListItem type = new SelectListItem();
                type.Text = item.Name;
                type.Value = item.Id.ToString();
                dataList.Add(type);
            }
            ViewBag.FreqUsedType = dataList;
            return View(model);
        }
        public JsonResult GetTrackByCategory(string categoryId)
        {
            try
            {
                var data = SfbDataHandler.GetTrackDataByCategory(Convert.ToInt64(categoryId));
                List<DDLOptions> dataList = new List<DDLOptions>();
                foreach (var item in data)
                {
                    DDLOptions options = new DDLOptions { Id = item.Id, Name = item.Name };
                    dataList.Add(options);
                }
                return Json(dataList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult GetFrequentlyUsedData(double freqId)
        {
            List<SFBDataModel> modelList = new List<SFBDataModel>();
            var data = SfbDataHandler.GetFreqUsedData(Convert.ToInt64(freqId));
            foreach (DataRow item in data.Rows)
            {
                SFBDataModel model = new SFBDataModel();
                model.FriendlyName = item["DisplayName"].ToString();
                model.Description = item["Description"].ToString();
                model.Link = item["Link"].ToString();
                model.ImagePath = item["ImagePath"].ToString();
                model.SfbLibId = Convert.ToInt64(item["ID"]);
                model.LinkType = item["LinkType"].ToString();
                modelList.Add(model);
            }
            Session["sfbList"] = modelList;
            return Json(modelList.ToArray());
        }

        [HttpPost]
        public ActionResult GetDetailsByCategoryTrack(long catId, long trackId)
        {
            List<SFBDataModel> modelList = new List<SFBDataModel>();
            var data = SfbDataHandler.GetSfbLibraryData(Convert.ToInt64(catId), Convert.ToInt64(trackId));
            foreach (DataRow item in data.Rows)
            {
                SFBDataModel model = new SFBDataModel();
                model.FriendlyName = item["DisplayName"].ToString();
                model.Description = item["Description"].ToString();
                model.Link = item["Link"].ToString();
                model.ImagePath = item["ImagePath"].ToString();
                model.LinkType = item["LinkType"].ToString();
                model.SfbLibId = Convert.ToInt64(item["ID"]);
                modelList.Add(model);
            }
            Session["sfbList"] = modelList;
            return Json(modelList.ToArray());
        }

        [HttpPost]
        public ActionResult SFBLibraryPost(SFBDataModel modelData)
        {
            try
            {
                string imagePath;

                if (modelData.LinkType == "3")
                {
                    //wiki
                    imagePath = "wiki_8.jpg";
                }
                else if (modelData.ModalTrack == "12" || modelData.ModalTrack == "13")
                {
                    //SOC
                    imagePath = "SOC.png";
                }
                else if (modelData.ModalTrack == "3")
                {
                    //CVM
                    imagePath = "CVM.png";
                }
                else if(modelData.Link.ToLower().Contains("powerbi"))
                {
                    imagePath = "PowerBI_82.png";
                }
                else
                {
                    //SD
                    imagePath = "SD.jpg";
                }
                if (SfbDataHandler.IsLinkExist(modelData.Link, Convert.ToInt16(modelData.FrequentlyUsed)))
                {
                    return Json(new { success = false, responseText = "Link you entered is exist.Kindly verify it in SFB library data." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    bool result = SfbDataHandler.InsertSfbLibrarysData(modelData.FriendlyName, modelData.Description, modelData.Link, imagePath, Convert.ToInt64(modelData.ModalTrack), Convert.ToInt64(modelData.Category), Convert.ToInt32(modelData.LinkType), Convert.ToInt32(modelData.FrequentlyUsed));
                    if (result == true)
                        return Json(new { success = false, responseText = "Unable to save data.Please try again" }, JsonRequestBehavior.AllowGet);
                    else
                        return Json(new { success = true, responseText = "SFB data saved successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            //return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SfbDataSort(string sortDir)
        {
            var dataList = (List<SFBDataModel>)Session["sfbList"];
            if (sortDir.ToUpper().Equals("ASC"))
            {
                dataList = dataList.OrderBy(a => a.FriendlyName).ToList();
            }
            else
            {
                dataList = dataList.OrderByDescending(a => a.FriendlyName).ToList();
            }
            return Json(dataList.ToArray());
        }

        [HttpPost]
        public ActionResult SfbDataSearch(string searchText)
        {
            var dataList = (List<SFBDataModel>)Session["sfbList"];
            dataList = dataList.Where(a => a.FriendlyName.ToUpper().Contains(searchText.ToUpper())).ToList();
            return Json(dataList.ToArray());
        }

        [HttpPost]
        public ActionResult GetDataForEdit(Int64 libId)
        {
            SFBDataModel model = new SFBDataModel();
            var data = SfbDataHandler.GetSFBLibraryDataById(libId);
            foreach (DataRow item in data.Rows)
            {
                model.FriendlyName = item["DisplayName"].ToString();
                model.Description = item["Description"].ToString();
                model.Link = item["Link"].ToString();
                model.SfbLibId = Convert.ToInt64(item["ID"]);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateSFBLibraryData(SFBDataModel modelData)
        {
            try
            {
                if (SfbDataHandler.UpdateSFBLibraryDataById(modelData.SfbLibId, modelData.Description, modelData.Link))
                {
                    return Json(new { success = true, responseText = "SFB data got updated successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, responseText = "Unable to update data. Please try again." }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}