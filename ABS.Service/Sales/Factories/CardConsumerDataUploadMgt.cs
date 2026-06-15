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
    public class CardConsumerDataUploadMgt : iCardConsumerDataUploadMgt
    {
        public IEnumerable<vmCardConsumerDataUpload> CardConsumerReportData(vmCmnParameters objcmnParam)
        {
            int reportType = 0;

            if(objcmnParam.challanNo != "" && objcmnParam.challanNo != null)
            {
                reportType = 1;
                objcmnParam.distributorId = "x";
                objcmnParam.fromDate = "x";
                objcmnParam.toDate = "x";
            }
            else if((objcmnParam.challanNo == "" || objcmnParam.challanNo == null) && (objcmnParam.distributorId == "" || objcmnParam.distributorId == null))
            {
                objcmnParam.challanNo = "x";
                objcmnParam.distributorId = "x";
                reportType = 3;
            }
            else
            {
                objcmnParam.challanNo = "x";
                reportType = 2;
            }

            List<vmCardConsumerDataUpload> reportData = new List<vmCardConsumerDataUpload>();
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "CITYN.p_scham_card.get_card_challan";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("p_reportType", OracleDbType.Varchar2).Value = reportType;
            objCmd.Parameters.Add("p_challanNo", OracleDbType.Varchar2).Value = objcmnParam.challanNo;
            objCmd.Parameters.Add("p_distributorId", OracleDbType.Varchar2).Value = objcmnParam.distributorId;           
            objCmd.Parameters.Add("p_startDate", OracleDbType.Varchar2).Value = objcmnParam.fromDate;
            objCmd.Parameters.Add("p_endDate", OracleDbType.Varchar2).Value = objcmnParam.toDate;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();

            DataTable dt = classDt.GetData(objCmd);
            reportData = ConvertDataTableToGenericList.BindList<vmCardConsumerDataUpload>(dt);
            return reportData;
        }
        public string UploadDocumentsSave(List<vmCardConsumerDataUpload> itemDetails)
        {
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);

            String UPLOAD_RESULT = string.Empty;
            bool tempInsert = false;
            try
            {
                string query = @"insert into cityn.t_temp_sap_data ( CHALLAN_NUMBER, CHALLAN_DATE, DISTRIBUTOR_ID, PRODUCT_ID, CHALLAN_QTY_CTN, CHALLAN_QTY_PCS, DP_CTN, DP_PCS, ENTRY_BY, UPLOAD_BY) 
                                                           values (:CHALLAN_NUMBER, :CHALLAN_DATE, :DISTRIBUTOR_ID, :PRODUCT_ID, :CHALLAN_QTY_CTN, :CHALLAN_QTY_PCS, :DP_CTN, :DP_PCS, :ENTRY_BY, :UPLOAD_BY)";
                con.Open();
                using (var command = con.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;
                    command.BindByName = true;
                    // In order to use ArrayBinding, the ArrayBindCount property
                    // of OracleCommand object must be set to the number of records to be inserted
                    command.ArrayBindCount = itemDetails.Count;
                    command.Parameters.Add(":CHALLAN_NUMBER", OracleDbType.Varchar2, itemDetails.Select(c => c.CHALLAN_NUMBER).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":CHALLAN_DATE", OracleDbType.Date, itemDetails.Select(c => c.CHALLAN_DATE).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":DISTRIBUTOR_ID", OracleDbType.Varchar2, itemDetails.Select(c => c.DISTRIBUTOR_ID).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":PRODUCT_ID", OracleDbType.Varchar2, itemDetails.Select(c => c.PRODUCT_ID).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":CHALLAN_QTY_CTN", OracleDbType.Decimal, itemDetails.Select(c => c.CHALLAN_QTY_CTN).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":CHALLAN_QTY_PCS", OracleDbType.Decimal, itemDetails.Select(c => c.CHALLAN_QTY_PCS).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":DP_CTN", OracleDbType.Decimal, itemDetails.Select(c => c.DP_CTN).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":DP_PCS", OracleDbType.Decimal, itemDetails.Select(c => c.DP_PCS).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":ENTRY_BY", OracleDbType.Varchar2, itemDetails.Select(c => c.ENTRY_BY).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":UPLOAD_BY", OracleDbType.Varchar2, itemDetails.Select(c => c.UPLOAD_BY).ToArray(), ParameterDirection.Input);
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
                UPLOAD_RESULT = ProcessData();
            }
            return UPLOAD_RESULT;
        }
        public string ProcessData()
        {
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
            OracleCommand objCmd = new OracleCommand();
            string UPLOAD_RESULT = string.Empty;
            try
            {
                    using (objCmd.Connection = con)
                    {
                        objCmd.CommandType = CommandType.StoredProcedure;

                        objCmd.Parameters.Add("p_oid", OracleDbType.Varchar2).Direction = ParameterDirection.Output;
                        objCmd.CommandText = "CITYN.p_scham_card.insert_challan_pack_card";                       
                      
                        objCmd.Connection.Open();
                        objCmd.ExecuteNonQuery();

                        //UPLOAD_RESULT = objCmd.Parameters["p_oid"].Value.ToString();
                        UPLOAD_RESULT = objCmd.Parameters[0].Value.ToString();
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
                objCmd.Connection.Close();

            }
            return UPLOAD_RESULT;
        }
        //public IEnumerable<vmCardConsumerMaster> GetDistTargetDocUploadMasterBackup(vmCmnParameters objcmnParam, out int recordsTotal)
        //{
        //    IEnumerable<vmCardConsumerMaster> objReportData = null;
        //    IEnumerable<vmCardConsumerMaster> objReportDataWithOutPaging = null;
        //    recordsTotal = 0;

        //    OracleCommand objCmd = new OracleCommand();
        //    objCmd.CommandText = "INCENTIVE.Get_DistTargetUploadMaster";
        //    objCmd.CommandType = CommandType.StoredProcedure;

        //    objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

        //    ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
        //    DataTable dt = classDt.GetData(objCmd);
        //    objReportDataWithOutPaging = ConvertDataTableToGenericList.BindList<vmCardConsumerMaster>(dt);
        //    objReportData = objReportDataWithOutPaging.OrderByDescending(x => x.DISTRIBUTOR_ID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();

        //    recordsTotal = objReportDataWithOutPaging.Count();
        //    return objReportData;
        //}

        public DataTable GetDistTargetDocUploadMaster(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            //IEnumerable<vmCardConsumerMaster> objReportData = null;
            //IEnumerable<vmCardConsumerMaster> objReportDataWithOutPaging = null;
            recordsTotal = 0;

            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "INCENTIVE.Get_DistTargetUploadMaster";
            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetData(objCmd);
            //objReportDataWithOutPaging = ConvertDataTableToGenericList.BindList<vmCardConsumerMaster>(dt);
            //objReportData = objReportDataWithOutPaging.OrderByDescending(x => x.DISTRIBUTOR_ID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();

            recordsTotal = dt.Rows.Count;
            return dt;
        }

        public int DeleteDistributorTargetRecord(vmCmnParameters objcmnParam, Int64 DocumentId)
        {
            int result = 0;

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
            OracleCommand objCmd = new OracleCommand();
            try
            {
                    using (objCmd.Connection = con)
                    {
                        objCmd.CommandType = CommandType.StoredProcedure;
                        objCmd.CommandText = "Incentive.DELETE_DISTTARGETRECORDS";
                        objCmd.Parameters.Add("DOCUMENTIDIN", OracleDbType.Decimal).Value = DocumentId;
                        objCmd.Parameters.Add("IS_DELETED", OracleDbType.Varchar2).Value = "1";
                        objCmd.Parameters.Add("DELETE_BY", OracleDbType.Decimal).Value = objcmnParam.loggeduser;
                        objCmd.Parameters.Add("DELETE_ON", OracleDbType.Date).Value = DateTime.Now;
                        objCmd.Parameters.Add("DELETE_IP", OracleDbType.Varchar2).Value = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(1).ToString();
                        objCmd.Connection.Open();
                        objCmd.ExecuteNonQuery();
                        result = 1;
                    }
            }
            catch (OracleException exception)
            {
                result = -1;

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
            return result;
        }        
    }
}

