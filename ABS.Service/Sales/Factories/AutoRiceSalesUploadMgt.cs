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
using System.IO;

namespace CTGroup.Service.Sales.Factories
{
    public class AutoRiceSalesUploadMgt : iAutoRiceSalesUpload
    {
        public List<vmDistributorTarget> DuplicateDataUploadChecking(DateTime startDate, DateTime endDate)
        {
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
            con.Open();
            string query = "select START_DATE EXISTINGDATE from T_CMNDOCUMENT where START_DATE >= '" + startDate.ToString("dd-MMMM-yy")
                         + "' and END_DATE <= '" + endDate.ToString("dd-MMMM-yy") + "' AND ISDELETED = 0 and TRANSACTIONTYPE = 'DistributorPrimaryTarget'";

            OracleCommand cmd = new OracleCommand(query, con);
            DataTable t1 = new DataTable();
            using (OracleDataAdapter a = new OracleDataAdapter(cmd))
            {
                a.Fill(t1);
            }
            con.Close();

            List<vmDistributorTarget> objDistributor = null;
            objDistributor = t1.AsEnumerable().Select(dataRow => new vmDistributorTarget
            {
                EXISTINGDATE = dataRow.Field<DateTime>("EXISTINGDATE")
            }).ToList();

            return objDistributor;
        }
        public object UploadBulkDatas(List<vmAutoRiceSalesUpload> bulkData, DateTime startDate, DateTime endDate, string LoggedUser)
        {
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
            string multivalue = string.Empty;
            string returnValue = string.Empty;
            string returnItem = string.Empty;
            int cno = 0; string[] salesDateList = new string[bulkData.Count], salesItemList = new string[bulkData.Count];
            foreach (vmAutoRiceSalesUpload singleRecord in bulkData)
            {
                string singlevlaue = string.Empty;

                singlevlaue = "x"
                    + ':' + singleRecord.SALES_ORG
                    + ':' + singleRecord.CUSTOMER_CODE
                    + ':' + singleRecord.SALES_DATE.ToString("dd-MMM-yy").ToUpper()
                    + ':' + singleRecord.SALES_ORDER
                    + ':' + singleRecord.MATERIAL_NAME
                    + ':' + singleRecord.MATERIAL_DESCRIPTION
                    + ':' + singleRecord.QNT_IN_BAG_CTN
                    + ':' + singleRecord.QNT_IN_KG_PCS
                    + ':' + singleRecord.QNT_IN_TON
                    + ':' + singleRecord.NET_WEIGHT
                    + ':' + singleRecord.SALES_VALUE
                    + ':' + singleRecord.SALES_TYPE + ':' + ';';

                multivalue += singlevlaue;

                var ie = salesDateList.Where(x => x == singleRecord.SALES_DATE.ToString("dd-MMM-yy").ToUpper()).FirstOrDefault();
                if (ie == null)
                {
                    salesDateList[cno] = singleRecord.SALES_DATE.ToString("dd-MMM-yy").ToUpper();
                }

                var itm = salesItemList.Where(x => x == singleRecord.MATERIAL_NAME).FirstOrDefault();
                if (itm == null)
                {
                    salesItemList[cno] = singleRecord.MATERIAL_NAME;
                }

                cno++;
            }

            try
            {
                string[] itemList = salesItemList.Where(x => x != null).ToArray();
                string multiItem = string.Join(",", itemList);
                string salesDates = string.Join(",", salesDateList.Where(x => x != null));

                using (OracleCommand objCmd = new OracleCommand())
                {
                    using (objCmd.Connection = con)
                    {
                        // checking if the item exists in legacy system product table

                        objCmd.CommandType = CommandType.StoredProcedure;
                        objCmd.CommandText = "AUTORICESALESUPLOAD.Check_Get_AutoRiceSalesItem";
                        objCmd.Parameters.Add("P_RESULT", OracleDbType.Varchar2, 30000).Direction = ParameterDirection.Output;
                        objCmd.Parameters.Add("P_ITEMLIST", OracleDbType.Varchar2, 30000).Value = multiItem;
                        objCmd.Connection.Open();
                        objCmd.ExecuteNonQuery();
                        objCmd.Connection.Close();
                        string returnStr = objCmd.Parameters["P_RESULT"].Value.ToString();
                        if (!string.IsNullOrEmpty(returnStr))
                        {
                            string[] rStrArray = returnStr.Split(',');
                            if (rStrArray.Count() == itemList.Count())
                            {
                                objCmd.Parameters.Clear();
                                objCmd.CommandType = CommandType.StoredProcedure;
                                objCmd.CommandText = "AUTORICESALESUPLOAD.SET_AutoRiceSalesUpload";
                                objCmd.Parameters.Add("P_OUTPUT_NUMBER", OracleDbType.Long, 35).Direction = ParameterDirection.Output;
                                objCmd.Parameters.Add("P_UPLOAD_BY", OracleDbType.Varchar2).Value = LoggedUser;
                                objCmd.Parameters.Add("P_UPLOAD_ON", OracleDbType.Date).Value = DateTime.Now;
                                objCmd.Parameters.Add("P_UPLOAD_IP", OracleDbType.Varchar2).Value = HostService.GetLocalIPAddress();
                                objCmd.Parameters.Add("p_DELDATE", OracleDbType.Varchar2).Value = salesDates;
                                //objCmd.Parameters.Add("P_REQDATALIST", OracleDbType.Varchar2, 30000).Value = multivalue;

                                objCmd.Parameters.Add("P_REQDATALIST", OracleDbType.Clob).Value = multivalue;


                                objCmd.Connection.Open();
                                objCmd.ExecuteNonQuery();
                                objCmd.Connection.Close();
                                returnValue = objCmd.Parameters["P_OUTPUT_NUMBER"].Value.ToString();
                            }
                            else
                            {
                                var lookup1 = itemList.ToLookup(item => item);
                                var lookup2 = rStrArray.ToLookup(item => item);
                                var output = lookup1.SelectMany(i => i.Take(i.Count() - lookup2[i.Key].Count())).ToArray();
                                returnItem = string.Join(", ", output);
                            }
                        }
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
            return new { returnValue, returnItem };
        }
        public IEnumerable<vmDistributorTargetMaster> GetDistTargetDocUploadMaster(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmDistributorTargetMaster> objvmPIMaster = null;
            IEnumerable<vmDistributorTargetMaster> objvmPIMasterWithOutPaging = null;
            recordsTotal = 0;

            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "Primary_SalesTarget_VS_Achievement.Get_DistPrimaryTargetUploadMaster";
            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetData(objCmd);
            objvmPIMasterWithOutPaging = ConvertDataTableToGenericList.BindList<vmDistributorTargetMaster>(dt);
            objvmPIMaster = objvmPIMasterWithOutPaging.OrderByDescending(x => x.DIST_TARGET_ID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();

            recordsTotal = objvmPIMasterWithOutPaging.Count();
            return objvmPIMaster;
        }

        public IEnumerable<vmCmnDocument> GetCmnDocument(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmCmnDocument> objvmPIMaster = null;
            IEnumerable<vmCmnDocument> objvmPIMasterWithOutPaging = null;
            recordsTotal = 0;

            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "AUTORICESALESUPLOAD.Get_CmnDocument";
            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("P_DOCPATHID", OracleDbType.Varchar2).Value = objcmnParam.TransactionTypeID;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetData(objCmd);
            objvmPIMasterWithOutPaging = ConvertDataTableToGenericList.BindList<vmCmnDocument>(dt);
            objvmPIMaster = objvmPIMasterWithOutPaging.OrderByDescending(x => x.DOCUMENTID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();

            recordsTotal = objvmPIMasterWithOutPaging.Count();
            return objvmPIMaster;
        }

        public int DeleteDistributorTargetRecord(vmCmnParameters objcmnParam, Int64 DocumentId)
        {
            int result = 0;

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
            try
            {
                using (OracleCommand objCmd = new OracleCommand())
                {
                    using (objCmd.Connection = con)
                    {
                        objCmd.CommandType = CommandType.StoredProcedure;
                        objCmd.CommandText = "Primary_SalesTarget_VS_Achievement.DELETE_DISTTARGETRECORDS";
                        objCmd.Parameters.Add("DOCUMENTIDIN", OracleDbType.Decimal).Value = DocumentId;
                        objCmd.Parameters.Add("P_STARTDATE", OracleDbType.Date).Value = objcmnParam.startDate;
                        objCmd.Parameters.Add("P_ENDDATE", OracleDbType.Date).Value = objcmnParam.endDate;
                        objCmd.Parameters.Add("IS_DELETED", OracleDbType.Varchar2).Value = "1";
                        objCmd.Parameters.Add("DELETE_BY", OracleDbType.Varchar2).Value = objcmnParam.loggeduser;
                        objCmd.Parameters.Add("DELETE_ON", OracleDbType.Date).Value = DateTime.Now;
                        objCmd.Parameters.Add("DELETE_IP", OracleDbType.Varchar2).Value = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(1).ToString();
                        objCmd.Connection.Open();
                        objCmd.ExecuteNonQuery();
                        objCmd.Connection.Close();
                        result = 1;
                    }
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
                con.Close();
            }
            return result;
        }
    }
}

