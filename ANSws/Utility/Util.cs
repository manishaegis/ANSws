using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ANSws.Utility
{
    public class Util
    {
        #region Utility

        public static string GetJsonFromDataTable(DataTable dataTable)
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = null;
            foreach (DataRow dr in dataTable.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dataTable.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return JsonConvert.SerializeObject(rows);
        }

        public static DateTime FYStartDate(DateTime dateTime)
        {
            return new DateTime((dateTime.Month >= 4 ? dateTime.Year : dateTime.Year - 1), 4, 1);
        }

        public static DataTable GetDataTableFromCommand(SqlCommand sqlCommand)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ansReportConnection"].ConnectionString);
                con.Open();

                SqlCommand arithabortCommand = new SqlCommand("SET ARITHABORT ON", con);
                arithabortCommand.ExecuteNonQuery();

                sqlCommand.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                da.Fill(dt);
                con.Close();
            }
            catch (Exception x)
            {
                ErrorHandling.LogException(x);
                return new DataTable();
            }
            return dt;
        }

        #endregion
    }
}