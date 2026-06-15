using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using CTGroup.Utility;
using System.Globalization;

namespace ABS.Web.Utility
{
    public class ReportActivityMonitoring
    {
        // constructor, property

        public string RequestStart(string reportName, string dataFilter, string userId, string userName)
        {
            string clientIpAddress = HostService.GetLocalIPAddress();

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ToString());
            OracleCommand objCommand = new OracleCommand();
            string MasterOID = string.Empty;
            try
            {
                using (objCommand.Connection = con)
                {
                    objCommand.CommandType = CommandType.StoredProcedure;
                    objCommand.CommandText = "PKG_REPORT_ACTIVITY.SET_REPORT_ACTIVITY";
                    objCommand.Parameters.Add("P_OID", OracleDbType.Varchar2, 50).Direction = ParameterDirection.Output;
                    objCommand.Parameters.Add("P_Report_Name", OracleDbType.Varchar2).Value = reportName;
                    objCommand.Parameters.Add("P_User_Pc_Ip", OracleDbType.Varchar2).Value = clientIpAddress;
                    objCommand.Parameters.Add("P_User_Id", OracleDbType.Varchar2).Value = userId;
                    objCommand.Parameters.Add("P_User_Name", OracleDbType.Varchar2).Value = userName;
                    objCommand.Parameters.Add("P_Data_Filter", OracleDbType.Varchar2).Value = dataFilter;

                    con.Open();
                    objCommand.ExecuteNonQuery();
                    MasterOID = objCommand.Parameters["P_OID"].Value.ToString();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return MasterOID;
        }

        public void RequestEnd(string autoId)
        {
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ToString());
            OracleCommand objCommand = new OracleCommand();
            try
            {
                using (objCommand.Connection = con)
                {
                    objCommand.CommandType = CommandType.StoredProcedure;
                    objCommand.CommandText = "PKG_REPORT_ACTIVITY.UPDATE_REPORT_ACTIVITY";
                    objCommand.Parameters.Add("P_OID", OracleDbType.Varchar2).Value = autoId;
                    con.Open();
                    objCommand.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }


        public string GetReportDateValidation(string fromDate)
        {
            DateTime dt = DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            int fromDateNew = dt.Year;

            if (fromDateNew < 2018)
                fromDate = "01/01/2018";
            return fromDate;
        }
    }
}