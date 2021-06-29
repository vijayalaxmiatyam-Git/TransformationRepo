using Dashboards_BusinessLayer.Interfaces;
using Dashboards_BusinessLayer.Models;
using Dashboards_DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Dashboards_BusinessLayer.Handlers
{
    public class SfbHandler : ISfbHandler
    {
        SfbDAL dalObj = new SfbDAL();
        public SfbHandler()
        {

        }
        public bool InsertSfbLibrarysData(string name, string description, string link, string imagePath, Int64 track,Int64 categoryId, int type, int freqUsd)
        {
            try
            {
                long cateTrack = dalObj.GetCategoryTrackId(categoryId, track);
                if (cateTrack > 0)
                {
                    if (dalObj.InsertSfbLibrarysData(name, description, link, imagePath, cateTrack, type, freqUsd) > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IsLinkExist(string link,int freqUsd)
        {
            try
            {
                return dalObj.IsLinkExist(link,freqUsd);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<NameValue> GetCategory()
        {
            try
            {
                List<NameValue> category = new List<NameValue>();
                var data = dalObj.GetCategory();
                foreach (DataRow item in data.Rows)
                {
                    NameValue cat = new NameValue();
                    cat.Id = Convert.ToInt64(item["CId"]);
                    cat.Name = item["CategoryName"].ToString();
                    category.Add(cat);
                }
                return category;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<NameValue> GetLinkType()
        {
            try
            {
                List<NameValue> linkType = new List<NameValue>();
                var data = dalObj.GetLinkType();
                foreach (DataRow item in data.Rows)
                {
                    NameValue type = new NameValue();
                    type.Id = Convert.ToInt64(item["Id"]);
                    type.Name = item["TypeName"].ToString();
                    linkType.Add(type);
                }
                return linkType;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<NameValue> GetFrequencyUsedTypes()
        {
            try
            {
                List<NameValue> freqUsd = new List<NameValue>();
                var data = dalObj.GetFrequencyUsedTypes();
                foreach (DataRow item in data.Rows)
                {
                    NameValue frqUs = new NameValue();
                    frqUs.Id = Convert.ToInt64(item["Id"]);
                    frqUs.Name = item["Name"].ToString();
                    freqUsd.Add(frqUs);
                }
                return freqUsd;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<NameValue> GetTrackDataByCategory(Int64 categoryId)
        {
            try
            {
                List<NameValue> tracks = new List<NameValue>();
                var data = dalObj.GetTrackDataByCategory(categoryId);
                foreach (DataRow item in data.Rows)
                {
                    NameValue track = new NameValue();
                    track.Id = Convert.ToInt64(item["TID"]);
                    track.Name = item["TrackName"].ToString();
                    tracks.Add(track);
                }
                return tracks;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable GetFreqUsedData(Int64 freqId)
        {
            try
            {
                return dalObj.GetFrequentlyUsedData(freqId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable GetSfbLibraryData(long cId, long trackId)
        {
            try
            {
                return dalObj.GetSfbLibraryData(cId, trackId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable GetSFBLibraryDataById(long Id)
        {
            try
            {
                return dalObj.GetSFBLibraryDataById(Id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool UpdateSFBLibraryDataById(Int64 Id, string description, string link)
        {
            try
            {
                int res = dalObj.UpdateSFBLibraryDataById(Id,description,link);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
                
        public DataTable GetOnboardTasks()
        {
            try
            {
                return dalObj.GetOnboardTasks();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public DataTable GetOnboardServices()
        {
            try
            {
                return dalObj.GetOnboardServices();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public DataTable GetOnboardVSOServices(string Track)
        {
            try
            {
                return dalObj.GetOnboardServiceTasks(Track);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public DataTable GetOnboardVSOTasks(string Track)
        {
            try
            {
                return dalObj.GetOnboardVSOTasks(Track);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Int64 InsertUpdateTrackTypeName(Int64 trackType, string name)
        {
            try
            {
                return dalObj.InsertUpdateTrackTypeName(trackType, name);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public DataTable GetTrackNamesByType(int type)
        {
            try
            {
                return dalObj.GetTrackNamesByType(type);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int InsertOnboardServiceData(string track, string taskName, string subTsk)
        {
            try
            {
                return dalObj.InsertOnboardServiceData(track, taskName, subTsk);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int InsertOnboardTaskData(string track, string taskName, string subTsk)
        {
            try
            {
                return dalObj.InsertOnboardTasksData(track, taskName, subTsk);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
