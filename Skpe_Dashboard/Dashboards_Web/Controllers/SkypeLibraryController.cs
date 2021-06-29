using Dashboards_BusinessLayer.Interfaces;
using Dashboards_Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dashboards_Web.Controllers
{
    public class SkypeLibraryController : Controller
    {
        readonly ISfbHandler SfbDataHandler;
        public SkypeLibraryController(ISfbHandler SfbDataHandler)
        {
            this.SfbDataHandler = SfbDataHandler;
        }

        // GET: SkypeLibrary
        public ActionResult SkypeLibrary(SFBDataModel model)
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

            ViewBag.Images = GetImageDropDown();
            return View(model);
        }

        public ActionResult SFBLibrary(SFBDataModel model)
        {
            model = new SFBDataModel();
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

        public List<SelectListItem> GetImageDropDown()
        {
            try
            {
                List<SelectListItem> dataList = new List<SelectListItem>();
                for (int i = 0; i < 15; i++)
                {
                    SelectListItem options = new SelectListItem { Value = "~/Content/img/wiki_" + i + ".jpg", Text = "<div><img src= ~/Content/img/wiki_" + i + ".jpg /><div>", };
                    dataList.Add(options);
                }
                return dataList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //[HttpPost]
        //public ActionResult SFBLibraryPost(SFBDataModel modelData)
        //{
        //    string imagePath = "Content/img/Template_95";

        //    bool result = SfbDataHandler.InsertSfbLibrarysData(modelData.FriendlyName,modelData.Description,modelData.Link, imagePath, Convert.ToInt64(modelData.Track),Convert.ToInt32(modelData.LinkType),Convert.ToInt32(modelData.FrequentlyUsed));
        //    string message = result == true ? "Link data stored successfully" : "Unable to save data. Please try again";
        //    return Json(message, JsonRequestBehavior.AllowGet);
        //}
    }
}