using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using CTGroup.Service.Sales.Interfaces;
using CTGroup.Models.ViewModel.Sales;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using CTGroup.OracleModel.ViewModel.Sales;
using CTGroup.Utility.Common;
using System.Diagnostics;

namespace CTGroup.Service.Sales.Factories
{
    public class SOWiseDistTargetDocumentUploadMgt : iSOWiseDistTargetDocumentUploadMgt
    {
        public List<vmDistributorTargetSOWise> DuplicateDataUploadChecking(DateTime startDate, DateTime endDate)
        {
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);

            con.Open();

            string query = "select START_DATE EXISTINGDATE from T_CMNDOCUMENT where START_DATE >= '" + startDate.ToString("dd-MMMM-yy")
                + "' and END_DATE <= '" + endDate.ToString("dd-MMMM-yy") + "' AND ISDELETED = 0 and TRANSACTIONTYPE = 'DistributorTargetSOWise'";

            OracleCommand cmd = new OracleCommand(query, con);

            DataTable t1 = new DataTable();
            using (OracleDataAdapter a = new OracleDataAdapter(cmd))
            {
                a.Fill(t1);
            }
            con.Close();

            List<vmDistributorTargetSOWise> objDistributor = null;
            objDistributor = t1.AsEnumerable().Select(dataRow => new vmDistributorTargetSOWise
            {
                EXISTINGDATE = dataRow.Field<DateTime>("EXISTINGDATE")
            }).ToList();

            return objDistributor;
        }

        public string UploadDocumentsSaveSOWiseDistTarget(List<vmDistributorTargetSOWise> itemDetails, DateTime startDate, DateTime endDate, string LoggedUser)
        {
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);

            String UPLOAD_RESULT = string.Empty;
            bool tempInsert = false;
            try
            {
                string query = @"insert into T_SECON_SALES_DIST_TAR_SO_TEMP
                ( CUST_ID, CUST_NAME, SO_ID, SO_NAME, TSO_C, TASO_C, SSO_C, TCR_C, TMO_C, TSG_C, TMR, JMW_C, 
                  TLD_C, NVO_C, BCT,  TA_C, MD_C, SZ_C, TWA_C, TCN_C, TPIS_C, VALUE) 

         values (:CUST_ID,:CUST_NAME, :SO_ID, :SO_NAME,:TSO_C,:TASO_C,:SSO_C,:TCR_C,:TMO_C,:TSG_C,:TMR,:JMW_C,
                  :TLD_C,:NVO_C,:BCT,:TA_C,:MD_C, :SZ_C, :TWA_C, :TCN_C, :TPIS_C, :VALUE)";

                con.Open();
                using (var command = con.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;
                    command.BindByName = true;
                    // In order to use ArrayBinding, the ArrayBindCount property
                    // of OracleCommand object must be set to the number of records to be inserted
                    command.ArrayBindCount = itemDetails.Count;
                    command.Parameters.Add(":CUST_ID", OracleDbType.Varchar2, itemDetails.Select(c => c.CUST_ID).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":CUST_NAME", OracleDbType.Varchar2, itemDetails.Select(c => c.CUST_NAME).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":SO_ID", OracleDbType.Varchar2, itemDetails.Select(c => c.SO_ID).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":SO_NAME", OracleDbType.Varchar2, itemDetails.Select(c => c.SO_NAME).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":TSO_C", OracleDbType.Double, itemDetails.Select(c => c.TSO_C).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":TASO_C", OracleDbType.Double, itemDetails.Select(c => c.TASO_C).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":SSO_C", OracleDbType.Double, itemDetails.Select(c => c.SSO_C).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":TCR_C", OracleDbType.Double, itemDetails.Select(c => c.TCR_C).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":TMO_C", OracleDbType.Double, itemDetails.Select(c => c.TMO_C).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":TSG_C", OracleDbType.Double, itemDetails.Select(c => c.TSG_C).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":TMR", OracleDbType.Double, itemDetails.Select(c => c.TMR).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":JMW_C", OracleDbType.Double, itemDetails.Select(c => c.JMW_C).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":TLD_C", OracleDbType.Double, itemDetails.Select(c => c.TLD_C).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":NVO_C", OracleDbType.Double, itemDetails.Select(c => c.NVO_C).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":BCT", OracleDbType.Double, itemDetails.Select(c => c.BCT).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":TA_C", OracleDbType.Double, itemDetails.Select(c => c.TA_C).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":MD_C", OracleDbType.Double, itemDetails.Select(c => c.MD_C).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":SZ_C", OracleDbType.Double, itemDetails.Select(c => c.SZ_C).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":TWA_C", OracleDbType.Double, itemDetails.Select(c => c.TWA_C).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":TCN_C", OracleDbType.Double, itemDetails.Select(c => c.TCN_C).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":TPIS_C", OracleDbType.Double, itemDetails.Select(c => c.TPIS_C).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":VALUE", OracleDbType.Double, itemDetails.Select(c => c.VALUE).ToArray(), ParameterDirection.Input);

                    int result = command.ExecuteNonQuery();
                    if (result == itemDetails.Count)
                        tempInsert = true;
                    else
                    {
                        UPLOAD_RESULT = "There are some invalid data. Please check data then upload again !!!";
                    }
                }
            }
            catch (OracleException exception)
            {
                var frame = new StackTrace(true).GetFrame(0);
                var filename = frame.GetFileName();
                var line = frame.GetFileLineNumber();

                Utils u = new Utils();
                u.LogWrite(exception, filename, line);
            }
            finally
            {
                con.Close();
            }
            if (tempInsert)
            {
                UPLOAD_RESULT = ProcessData(startDate, endDate, LoggedUser);
            }
            return UPLOAD_RESULT;
        }
        public string ProcessData(DateTime startDate, DateTime endDate, string LoggedUser)
        {
            string result = string.Empty;
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
            OracleCommand objCmd = new OracleCommand();

            string clientIpAddress = HostService.GetLocalIPAddress();

            try
            {
                using (OracleCommand objCmds = new OracleCommand())
                {
                    objCmds.Connection = con;
                    objCmds.CommandType = CommandType.StoredProcedure;
                    objCmds.Parameters.Add("P_START_DATE", OracleDbType.Date).Value = startDate;
                    objCmds.Parameters.Add("P_END_DATE", OracleDbType.Date).Value = endDate;
                    objCmds.Parameters.Add("P_CREATED_BY", OracleDbType.Varchar2).Value = LoggedUser;
                    objCmds.Parameters.Add("P_CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                    objCmds.Parameters.Add("P_CREATED_IP", OracleDbType.Varchar2).Value = clientIpAddress;

                    objCmds.CommandText = "DATAUPLOAD.SET_SECONDARY_SALES_TARGET_SO_WISE_UPLOAD_VALUE";

                    objCmds.Connection.Open();
                    objCmds.ExecuteNonQuery();

                    objCmds.Connection.Close();
                }

                using (objCmd.Connection = con)
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.Add("P_START_DATE", OracleDbType.Date).Value = startDate;
                    objCmd.Parameters.Add("P_END_DATE", OracleDbType.Date).Value = endDate;
                    objCmd.Parameters.Add("P_CREATED_BY", OracleDbType.Varchar2).Value = LoggedUser;
                    objCmd.Parameters.Add("P_CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                    objCmd.Parameters.Add("P_CREATED_IP", OracleDbType.Varchar2).Value = clientIpAddress;

                    objCmd.CommandText = "DATAUPLOAD.SET_SECONDARY_SALES_TARGET_SO_WISE_UPLOAD";

                    objCmd.Connection.Open();
                    objCmd.ExecuteNonQuery();

                    result = "1";
                }
            }
            catch (OracleException exception)
            {
                var frame = new StackTrace(true).GetFrame(0);
                var filename = frame.GetFileName();
                var line = frame.GetFileLineNumber();

                Utils u = new Utils();
                u.LogWrite(exception, filename, line);
                result = "0";
            }
            finally
            {
                objCmd.Connection.Close();
            }
            return result;
        }
        public IEnumerable<vmDistributorTargetSOWiseMaster> GetDistTargetDocUploadMaster(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmDistributorTargetSOWiseMaster> objvmPIMaster = null;
            IEnumerable<vmDistributorTargetSOWiseMaster> objvmPIMasterWithOutPaging = null;
            recordsTotal = 0;

            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "DATAUPLOAD.Get_DistTargetUploadMaster_SO";
            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetData(objCmd);
            objvmPIMasterWithOutPaging = ConvertDataTableToGenericList.BindList<vmDistributorTargetSOWiseMaster>(dt);
            objvmPIMaster = objvmPIMasterWithOutPaging.OrderByDescending(x => x.DIST_TARGET_ID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();

            recordsTotal = objvmPIMasterWithOutPaging.Count();
            return objvmPIMaster;
        }
        public string DeleteDistributorTargetRecord(vmCmnParameters objcmnParam, Int64 DocumentId)
        {
            Int32 currenttime = Convert.ToInt32(DateTime.Now.ToString("MMyyyy"));
            Int32 filetime = Convert.ToInt32(objcmnParam.startDate.ToString("MMyyyy"));

            string result = string.Empty;
            if (currenttime <= filetime)
            {
                OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
                OracleCommand objCmd = new OracleCommand();

                string clientIpAddress = HostService.GetLocalIPAddress();

                try
                {
                    using (objCmd.Connection = con)
                    {
                        objCmd.CommandType = CommandType.StoredProcedure;
                        objCmd.CommandText = "DATAUPLOAD.DELETE_SECONDARY_SALES_TARGET_SO_WISE_RECORDS";
                        objCmd.Parameters.Add("P_RESULT", OracleDbType.Varchar2).Direction = ParameterDirection.Output;
                        objCmd.Parameters.Add("DOCUMENTIDIN", OracleDbType.Decimal).Value = DocumentId;
                        objCmd.Parameters.Add("P_STARTDATE", OracleDbType.Date).Value = objcmnParam.startDate;
                        objCmd.Parameters.Add("P_ENDDATE", OracleDbType.Date).Value = objcmnParam.endDate;
                        objCmd.Parameters.Add("IS_DELETED", OracleDbType.Varchar2).Value = "1";
                        objCmd.Parameters.Add("DELETE_BY", OracleDbType.Varchar2).Value = objcmnParam.loggeduser;
                        objCmd.Parameters.Add("DELETE_ON", OracleDbType.Date).Value = DateTime.Now;
                        objCmd.Parameters.Add("DELETE_IP", OracleDbType.Varchar2).Value = clientIpAddress;
                        objCmd.Connection.Open();
                        objCmd.ExecuteNonQuery();
                        result = "1";
                    }
                }
                catch (OracleException exception)
                {
                    result = "0";

                    var frame = new StackTrace(true).GetFrame(0);
                    var filename = frame.GetFileName();
                    var line = frame.GetFileLineNumber();

                    Utils u = new Utils();
                    u.LogWrite(exception, filename, line);
                }
                finally
                {
                    objCmd.Connection.Close();
                }
            }
            else
            {
                result = "0";
            }
            return result;
        }
    }
}

