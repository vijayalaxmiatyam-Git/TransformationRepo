using Dashboards_BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Dashboards_BusinessLayer.Interfaces
{
    public interface ISfbHandler
    {
        bool InsertSfbLibrarysData(string name, string description, string link, string imagePath, Int64 track,Int64 categoryId, int type, int freqUsd);
        List<NameValue> GetCategory();
        List<NameValue> GetLinkType();
        List<NameValue> GetFrequencyUsedTypes();
        List<NameValue> GetTrackDataByCategory(Int64 categoryId);
        DataTable GetFreqUsedData(Int64 freqId);
        DataTable GetSfbLibraryData(Int64 cId, Int64 trackId);
        bool IsLinkExist(string link, int freqUsd);
        DataTable GetSFBLibraryDataById(long Id);
        bool UpdateSFBLibraryDataById(Int64 Id, string description, string link);
        DataTable GetOnboardTasks();
        DataTable GetOnboardVSOTasks(string Track);

        DataTable GetOnboardServices();
        DataTable GetOnboardVSOServices(string Track);
        Int64 InsertUpdateTrackTypeName(Int64 trackType, string name);
        DataTable GetTrackNamesByType(int type);
        int InsertOnboardServiceData(string track, string taskName, string subTsk);
        int InsertOnboardTaskData(string track, string taskName, string subTsk);
    }
}
