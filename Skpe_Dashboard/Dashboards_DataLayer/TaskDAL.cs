using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace Dashboards_DataLayer
{
    public class TaskDAL
    {
        public DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }

                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    if (rows.Length > 1)
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i].Trim();
                        }
                        dt.Rows.Add(dr);
                    }
                }

            }
            return dt;
        }

        public DataTable ConvertXSLXtoDataTable(string strFilePath, string connString)
        {
            OleDbConnection oledbConn = new OleDbConnection(connString);
            DataTable dt = new DataTable();
            try
            {

                oledbConn.Open();
                using (OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Imaging Failure Data$]", oledbConn))
                {
                    OleDbDataAdapter oleda = new OleDbDataAdapter();
                    oleda.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    oleda.Fill(ds);
                    dt = ds.Tables[0];

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                oledbConn.Close();
            }

            return dt;

        }

        public int InsertVSOsData(Int64 Id, string title, DateTime createDate, string description, string state, string machines, int count)
        {
            try
            {
                int result;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AutomationConnectionString"].ConnectionString);
                SqlCommand command = new SqlCommand("Insert_ImagingData", connection);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.BigInt));
                command.Parameters["@Id"].Value = Id;
                command.Parameters.Add(new SqlParameter("@Title", SqlDbType.VarChar));
                command.Parameters["@Title"].Value = title;
                command.Parameters.Add(new SqlParameter("@CreatedDate", SqlDbType.DateTime));
                command.Parameters["@CreatedDate"].Value = createDate;
                command.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar));
                command.Parameters["@Description"].Value = description;
                command.Parameters.Add(new SqlParameter("@State", SqlDbType.VarChar));
                command.Parameters["@State"].Value = state;
                command.Parameters.Add(new SqlParameter("@Machines", SqlDbType.VarChar));
                command.Parameters["@Machines"].Value = machines;
                command.Parameters.Add(new SqlParameter("@MachineCount", SqlDbType.Int));
                command.Parameters["@MachineCount"].Value = count;

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

        public bool ExecuteImagingProcedure(DataTable tableData)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AutomationConnectionString"].ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "dbo.Insert_ImagingDataDump";
                        command.CommandType = CommandType.StoredProcedure;
                        SqlParameter parameter;
                        parameter = command.Parameters.AddWithValue("@Display", tableData);
                        parameter.SqlDbType = SqlDbType.Structured;
                        parameter.TypeName = "dbo.ImagingTableType";
                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetVsoData()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AutomationConnectionString"].ConnectionString);
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "Get_VSOImageData";
                SqlDataAdapter adapterObj = new SqlDataAdapter();
                adapterObj.SelectCommand = command;
                adapterObj.SelectCommand.CommandType = CommandType.StoredProcedure;
                connection.Open();
                adapterObj.Fill(ds, "VsoData");
                connection.Close();

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw e;
            }
            return ds.Tables[0];
        }
    }
}

