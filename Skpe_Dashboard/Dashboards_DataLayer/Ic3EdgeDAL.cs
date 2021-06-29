using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Dashboards_DataLayer
{
    public class Ic3EdgeDAL
    {
        public DataTable GetUnprocessedTestData(int suitId)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SFBConnection"].ConnectionString))
                {
                    SqlCommand command = new SqlCommand("Get_Ic3TestData", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    SqlParameter parameter;
                    parameter = command.Parameters.AddWithValue("@SuiteId", suitId);
                    parameter.SqlDbType = SqlDbType.Int;

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool UpdateProcessedTestData(DataTable tableData, int suitId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SFBConnection"].ConnectionString))
                {

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        DataTable dt = new DataTable();
                        dt = tableData.Copy();
                        dt.Columns.RemoveAt(1);
                        dt.Columns.RemoveAt(1);
                        dt.Columns.RemoveAt(1);
                        dt.Columns.RemoveAt(1);
                        command.CommandText = "dbo.UpdateProcessedTestData";
                        command.CommandType = CommandType.StoredProcedure;
                        SqlParameter parameter;
                        parameter = command.Parameters.AddWithValue("@PrcsedTstDt", dt);
                        parameter.SqlDbType = SqlDbType.Structured;
                        parameter.TypeName = "dbo.updateIc3ProcessedTests";
                        parameter = command.Parameters.AddWithValue("@SuitID", suitId);
                        parameter.SqlDbType = SqlDbType.Int;
                        connection.Open();
                        command.ExecuteNonQuery();
                        //connection.Close();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
