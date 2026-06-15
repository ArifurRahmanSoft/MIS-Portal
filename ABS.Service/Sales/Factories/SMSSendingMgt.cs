using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel.ViewModel.Sales;
using CTGroup.Service.Sales.Interfaces;
using CTGroup.Utility.Common;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace CTGroup.Service.Sales.Factories
{
    public class SMSSendingMgt : iSMSSendingMgt
    {
        public string SendSingleSMS(vmSMSSending model, string messageResponse)
        {
            string MasterOID = string.Empty;

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
            OracleCommand objCmd = new OracleCommand();
            try
            {
                using (objCmd.Connection = con)
                {
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.CommandText = "PKG_SMS_SENDING.SET_SUCCESS_MESSAGE_LOG";
                    objCmd.Parameters.Add("P_OID", OracleDbType.Varchar2, 1).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("P_MESSAGETEXT", OracleDbType.NVarchar2).Value = model.MESSAGETEXT;
                    objCmd.Parameters.Add("P_MOBILENUMBER", OracleDbType.Varchar2).Value = model.MOBILENUMBER;
                    objCmd.Parameters.Add("P_MESSAGE_RESPONSE", OracleDbType.Varchar2).Value = messageResponse;

                    objCmd.Parameters.Add("P_ENTRYUSER", OracleDbType.Varchar2).Value = model.ENTRYUSER;
                    objCmd.Parameters.Add("P_ENTRYDATE", OracleDbType.Date).Value = DateTime.Now;

                    objCmd.Connection.Open();
                    objCmd.ExecuteNonQuery();

                    MasterOID = objCmd.Parameters["P_OID"].Value.ToString();
                }
            }
            catch (OracleException exception)
            {
                var frame = new StackTrace(true).GetFrame(0);
                var filename = frame.GetFileName();
                var line = frame.GetFileLineNumber();

                //Utils u = new Utils();
                //u.LowWrite(exception, filename, line);
            }
            finally
            {
                objCmd.Connection.Close();
            }
            return MasterOID;
        }

        #region Read         
        public IEnumerable<vmSMSSending> GetSentMessageDetail(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmSMSSending> objUser = null;
            IEnumerable<vmSMSSending> objUserWithOutPaging = null;
            recordsTotal = 0;

            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_SMS_SENDING.GET_MESSAGE_LOG";
            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("P_LOGGEDUSER", OracleDbType.Varchar2).Value = objcmnParam.loggeduser;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetData(objCmd);
            objUserWithOutPaging = ConvertDataTableToGenericList.BindList<vmSMSSending>(dt);
            objUser = objUserWithOutPaging.OrderByDescending(x => x.OID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();

            recordsTotal = objUserWithOutPaging.Count();
            return objUser;
        }
        #endregion
    }
}
