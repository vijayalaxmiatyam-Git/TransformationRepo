using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace Dashboards_DataLayer
{
    public class SfbDAL
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SFBConnection"].ConnectionString);
        public int InsertSfbLibrarysData(string name, string description, string link, string imagePath, Int64 cateTrack, int type, int freqUsd)
        {
            try
            {
                int result;
                SqlCommand command = new SqlCommand("Insert_Update_SFBLibraryData", connection);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@DisplayName", SqlDbType.NVarChar));
                command.Parameters["@DisplayName"].Value = name;
                command.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar));
                command.Parameters["@Description"].Value = description;
                command.Parameters.Add(new SqlParameter("@Link", SqlDbType.NVarChar));
                command.Parameters["@Link"].Value = link;
                command.Parameters.Add(new SqlParameter("@ImagePath", SqlDbType.NVarChar));
                command.Parameters["@ImagePath"].Value = imagePath;
                command.Parameters.Add(new SqlParameter("@CateTrack", SqlDbType.BigInt));
                command.Parameters["@CateTrack"].Value = cateTrack;
                command.Parameters.Add(new SqlParameter("@Type", SqlDbType.Int));
                command.Parameters["@Type"].Value = type;
                command.Parameters.Add(new SqlParameter("@FrequentlyUsed", SqlDbType.Int));
                command.Parameters["@FrequentlyUsed"].Value = freqUsd;

                connection.Open();
                result = command.ExecuteNonQuery();
                connection.Close();
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetCategory()
        {
            try
            {
                DataTable dt = new DataTable();
                GetData(ref dt, "Get_Category");
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataTable GetLinkType()
        {
            try
            {
                DataTable dt = new DataTable();
                GetData(ref dt, "Get_LinkType");
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IsLinkExist(string link, int freqUsd)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand command = new SqlCommand("Get_IsSFBLibraryDataExist", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(command);
                command.Parameters.Add(new SqlParameter("@Link", SqlDbType.VarChar));
                command.Parameters["@Link"].Value = link;
                command.Parameters.Add(new SqlParameter("@FreqUsedId", SqlDbType.Int));
                command.Parameters["@FreqUsedId"].Value = freqUsd;
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public DataTable GetFrequencyUsedTypes()
        {
            try
            {
                DataTable dt = new DataTable();
                GetData(ref dt, "Get_FrequencyUsedTypes");
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataTable GetTrackDataByCategory(Int64 categoryId)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand command = new SqlCommand("Get_CategoryWiseTrackData", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(command);
                command.Parameters.Add(new SqlParameter("@CategoryId", SqlDbType.BigInt));
                command.Parameters["@CategoryId"].Value = categoryId;
                da.Fill(dt);
                return dt;
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
                DataTable dt = new DataTable();
                GetData(ref dt, "Get_OnBoardTasks");
                return dt;
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
                DataTable dt = new DataTable();
                GetData(ref dt, "Get_OnboardServiceData");
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void GetData(ref DataTable table, string procName)
        {
            try
            {
                SqlCommand command = new SqlCommand(procName, connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(table);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public DataTable GetOnboardServiceTasks(string track)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand command = new SqlCommand("Get_OnboardServiceVSOTasks", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(command);
                command.Parameters.Add(new SqlParameter("@Track", SqlDbType.VarChar));
                command.Parameters["@Track"].Value = track;
                da.Fill(dt);
                return dt;
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
                DataTable dt = new DataTable();
                SqlCommand command = new SqlCommand("Get_OnBoardVSOTasks", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(command);
                command.Parameters.Add(new SqlParameter("@Track", SqlDbType.VarChar));
                command.Parameters["@Track"].Value = Track;
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public DataTable GetFrequentlyUsedData(Int64 fId)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand command = new SqlCommand("Get_SFBLibraryFrequentlyUsedData", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(command);
                command.Parameters.Add(new SqlParameter("@FId", SqlDbType.BigInt));
                command.Parameters["@FId"].Value = fId;
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Int64 GetCategoryTrackId(Int64 cId, Int64 trackId)
        {
            try
            {
                SqlCommand command = new SqlCommand("Get_CategoryTrackID", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter parm = new SqlParameter("@categoryTrack", SqlDbType.BigInt);
                parm.Direction = ParameterDirection.ReturnValue;

                SqlDataAdapter da = new SqlDataAdapter(command);
                command.Parameters.Add(parm);
                command.Parameters.Add(new SqlParameter("@CategoryId", SqlDbType.BigInt));
                command.Parameters["@CategoryId"].Value = cId;
                command.Parameters.Add(new SqlParameter("@TrackId", SqlDbType.BigInt));
                command.Parameters["@TrackId"].Value = trackId;
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

                Int64 catTrack = Convert.ToInt64(parm.Value);
                return catTrack;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public DataTable GetSfbLibraryData(Int64 cId, Int64 trackId)
        {
            try
            {
                Int64 cateTrack = GetCategoryTrackId(cId, trackId);
                DataTable dt = new DataTable();

                SqlCommand command = new SqlCommand("Get_SFBLibraryData", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(command);
                command.Parameters.Add(new SqlParameter("@CategoryTrackId", SqlDbType.BigInt));
                command.Parameters["@CategoryTrackId"].Value = cateTrack;

                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable GetSFBLibraryDataById(Int64 Id)
        {
            try
            {
                DataTable dt = new DataTable();

                SqlCommand command = new SqlCommand("Get_SFBLibraryDataById", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(command);
                command.Parameters.Add(new SqlParameter("@SfbLibId", SqlDbType.BigInt));
                command.Parameters["@SfbLibId"].Value = Id;

                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int UpdateSFBLibraryDataById(Int64 Id, string description, string link)
        {
            try
            {
                SqlCommand command = new SqlCommand("Update_SFBLibraryDataById", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(command);
                command.Parameters.Add(new SqlParameter("@SfbLibId", SqlDbType.BigInt));
                command.Parameters["@SfbLibId"].Value = Id;
                command.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar));
                command.Parameters["@Description"].Value = description;
                command.Parameters.Add(new SqlParameter("@Link", SqlDbType.NVarChar));
                command.Parameters["@Link"].Value = link;

                connection.Open();
                int result = command.ExecuteNonQuery();
                connection.Close();
                return result;
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
                SqlCommand command = new SqlCommand("Insert_Update_TrackTypeName", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(command);
                command.Parameters.Add(new SqlParameter("@TrackType", SqlDbType.Int));
                command.Parameters["@TrackType"].Value = trackType;
                command.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar));
                command.Parameters["@Name"].Value = name;
                connection.Open();
                int val = command.ExecuteNonQuery();
                connection.Close();
                return val;
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
                DataTable dt = new DataTable();

                SqlCommand command = new SqlCommand("Get_TrackNamesByType", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(command);
                command.Parameters.Add(new SqlParameter("@TrackType", SqlDbType.Int));
                command.Parameters["@TrackType"].Value = type;

                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int InsertOnboardTasksData (string track, string taskName, string subTsk)
        {
            try
            {
                SqlCommand command = new SqlCommand("Insert_OnboardTasksData", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(command);
                command.Parameters.Add(new SqlParameter("@Track", SqlDbType.VarChar));
                command.Parameters["@Track"].Value = track;
                command.Parameters.Add(new SqlParameter("@TaskName", SqlDbType.VarChar));
                command.Parameters["@TaskName"].Value = taskName;
                command.Parameters.Add(new SqlParameter("@SubTasks", SqlDbType.VarChar));
                command.Parameters["@SubTasks"].Value = subTsk;

                connection.Open();
                int result = command.ExecuteNonQuery();
                connection.Close();
                return result;
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
                SqlCommand command = new SqlCommand("Insert_OnboardServiceData", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(command);
                command.Parameters.Add(new SqlParameter("@Track", SqlDbType.VarChar));
                command.Parameters["@Track"].Value = track;
                command.Parameters.Add(new SqlParameter("@TaskName", SqlDbType.VarChar));
                command.Parameters["@TaskName"].Value = taskName;
                command.Parameters.Add(new SqlParameter("@SubTasks", SqlDbType.VarChar));
                command.Parameters["@SubTasks"].Value = subTsk;

                connection.Open();
                int result = command.ExecuteNonQuery();
                connection.Close();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
