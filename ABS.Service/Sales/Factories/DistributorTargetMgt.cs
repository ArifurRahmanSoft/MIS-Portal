using CTGroup.Data.BaseInterfaces;
using CTGroup.Models.ViewModel.Sales;
using CTGroup.Service.Sales.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using CTGroup.Models.ViewModel.SystemCommon;
//using CTGroup.Utility;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Configuration;
using ABS.Service;
using CTGroup.Utility.Common;
using CTGroup.OracleModel.ViewModel.SystemCommon;
using CTGroup.Utility;
using System.Diagnostics;
using CTGroup.OracleModel.ViewModel.Sales;

namespace CTGroup.Service.Sales.Factories
{
    public class DistributorTargetMgt : iDistributorTargetMgt
    {
       
       // private iGenericFactory<T_DIST_TAR_MASTER> DistributorTargetMaster_GF = null;
        private iGenericFactory<vmDistributorTargetDetail> GenericFactory_vmDistributorTargetDetail_GF = null;

        public List<vmDistributor> GetDivision(int? pageNumber, int? pageSize, int? IsPaging)
        {
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);

            con.Open();
            string query = "SELECT OID, SCUST_INFO_TEXT, SCUST_INFO_NAME FROM CITYN.T_SCUST_INFO";

            OracleCommand cmd = new OracleCommand(query, con);

            DataTable t1 = new DataTable();
            using (OracleDataAdapter a = new OracleDataAdapter(cmd))
            {
                a.Fill(t1);
            }
            con.Close();

            List<vmDistributor> objDistributor = null;
            objDistributor = t1.AsEnumerable().Select(dataRow => new vmDistributor
            {
                OID = dataRow.Field<string>("OID"),
                SCUST_INFO_TEXT = dataRow.Field<string>("SCUST_INFO_TEXT"),
                SCUST_INFO_NAME = dataRow.Field<string>("SCUST_INFO_NAME"),
            }).ToList();

            return objDistributor;
        }

        public List<vmDistributor> GetDistributor(int? pageNumber, int? pageSize, int? IsPaging)
        {
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);

            con.Open();
            string query = "SELECT OID, SCUST_INFO_TEXT, SCUST_INFO_NAME FROM CITYN.T_SCUST_INFO";

            OracleCommand cmd = new OracleCommand(query, con);

            DataTable t1 = new DataTable();
            using (OracleDataAdapter a = new OracleDataAdapter(cmd))
            {
                a.Fill(t1);
            }
            con.Close();

            List<vmDistributor> objDistributor = null;
            objDistributor = t1.AsEnumerable().Select(dataRow => new vmDistributor
            {
                OID = dataRow.Field<string>("OID"),
                SCUST_INFO_TEXT = dataRow.Field<string>("SCUST_INFO_TEXT"),
                SCUST_INFO_NAME = dataRow.Field<string>("SCUST_INFO_NAME"),
            }).ToList();

            return objDistributor;
        }
        
     
        public IEnumerable<vmDistributorTargetMaster> GetDistributorTargetMaster(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmDistributorTargetMaster> objvmPIMaster = null;
            IEnumerable<vmDistributorTargetMaster> objvmPIMasterWithOutPaging = null;
            recordsTotal = 0;
            
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "INCENTIVE.Get_DistTargetMaster";
            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetData(objCmd);
            objvmPIMasterWithOutPaging = ConvertDataTableToGenericList.BindList<vmDistributorTargetMaster>(dt);
            objvmPIMaster = objvmPIMasterWithOutPaging.OrderByDescending(x => x.DIST_TARGET_ID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();

            recordsTotal = objvmPIMasterWithOutPaging.Count();
            return objvmPIMaster;
        }
        //public string DeleteMasterDetail(vmCmnParameters objcmnParam)
        //{
        //    DistributorTargetMaster_GF = new DISTRIBUTORTARGETMASTER_GF();
        //    string result = string.Empty;
        //    string spQuery = string.Empty;
        //    try
        //    {
        //        Hashtable ht = new Hashtable();
        //        ht.Add("CompanyID", objcmnParam.loggedCompany);
        //        ht.Add("LoggedUser", objcmnParam.loggeduser);
        //        ht.Add("DistributorTargetID", objcmnParam.id);
        //        spQuery = "[Delete_BookingMasterDetail]";
        //        result = DistributorTargetMaster_GF.ExecuteCommandString(spQuery, ht);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return result;
        //}
        public string SaveUpdateDistributorTarget(vmDistributorTargetMaster itemMaster, List<vmDistributorTargetDetail> itemDetails, vmCmnParameters objcmnParam)
        {
            string DIST_TARGET_DETAIL_LIST = "";

            string DIST_TARGET_ID = string.Empty;

            DIST_TARGET_DETAIL_LIST = GetDetails(itemDetails);
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
            OracleCommand objCmd = new OracleCommand();
            try
            {
                using (objCmd.Connection = con)
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "INCENTIVE.SET_DISTTARGETMASTER";


                    objCmd.Parameters.Add("DIST_TARGET_ID", OracleDbType.Long, 35).Direction = ParameterDirection.Output;


                    objCmd.Parameters.Add("DISTRIBUTOR_ID", OracleDbType.Varchar2).Value = itemMaster.DISTRIBUTOR_ID;
                    objCmd.Parameters.Add("START_DATE", OracleDbType.Date).Value = itemMaster.START_DATE;
                    objCmd.Parameters.Add("END_DATE", OracleDbType.Date).Value = itemMaster.END_DATE;
                    objCmd.Parameters.Add("SALES_TYPE", OracleDbType.Varchar2).Value = itemMaster.SALES_TYPE.Trim();
                    objCmd.Parameters.Add("REMARKS", OracleDbType.Varchar2).Value = itemMaster.Remarks;
                    objCmd.Parameters.Add("IS_ACTIVE", OracleDbType.Varchar2).Value = "Y";
                    objCmd.Parameters.Add("CREATED_BY", OracleDbType.Int32).Value = objcmnParam.loggeduser;
                    objCmd.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                    objCmd.Parameters.Add("CREATED_IP", OracleDbType.Varchar2).Value = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(1).ToString();

                    objCmd.Parameters.Add("DIST_TARGET_DETAIL_LIST", OracleDbType.Varchar2, 10000).Value = DIST_TARGET_DETAIL_LIST;

                    objCmd.Connection.Open();
                    objCmd.ExecuteNonQuery();

                    DIST_TARGET_ID = objCmd.Parameters["DIST_TARGET_ID"].Value.ToString();
                }
            }
            catch (OracleException exception)
            {
                //string thisFile = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();

                //var st = new StackTrace(exception, true);

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
            return DIST_TARGET_ID;
        }       

        public string GetDetails(List<vmDistributorTargetDetail> itemDetails)
        {
            string details = "";

            foreach (var item in itemDetails)
            {
                string detail = "";
                int UOMID = 0;
                string BRANDID = "";
                string CREATEDIP = "1.1.1.1";
                int CREATEDBY = 1;
                decimal? QUANTITY = 0;

                BRANDID = item.BRANDID;
                QUANTITY = item.QUANTITY;
                UOMID = 8;
                CREATEDIP = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(1).ToString();
                DateTime CREATEDDATE = DateTime.Now;


                CREATEDBY = 1;

                detail = "x" + ':' + BRANDID + ':' + QUANTITY 
                             + ':' + UOMID + ':' + CREATEDIP + ':' + CREATEDDATE + ':' + CREATEDBY + ';';

                details += detail;
            }
            return details;
        }
        public IEnumerable<vmBrandSKU> GetBrand(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmBrandSKU> objBrand = null;
            IEnumerable<vmBrandSKU> objBrandWithOutPaging = null;
            recordsTotal = 0;

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);

            con.Open();
            string query = "SELECT OID AS BRANDID, SBRND_TEXT, SBRND_NAME, IDAT, EDAT FROM CITYN.T_SBRND";

            OracleCommand cmd = new OracleCommand(query, con);

            DataTable t1 = new DataTable();
            using (OracleDataAdapter a = new OracleDataAdapter(cmd))
            {
                a.Fill(t1);
            }
            con.Close();

            objBrandWithOutPaging = t1.AsEnumerable().Select(dataRow => new vmBrandSKU
            {
                BRANDID = dataRow.Field<string>("BRANDID"),
                SBRND_TEXT = dataRow.Field<string>("SBRND_TEXT"),
                SBRND_NAME = dataRow.Field<string>("SBRND_NAME"),
                IDAT = dataRow.Field<DateTime>("IDAT"),
                EDAT = dataRow.Field<DateTime>("EDAT")
            }).ToList();

           

            objBrand = objBrandWithOutPaging.OrderBy(x => x.BRANDID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();

            recordsTotal = objBrandWithOutPaging.Count();
            return objBrand;
        }

        public string GenOrdDlvCountCustom(vmCmnParameters cparam)
        {   
            string lastGenerate=string.Empty;
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["SecondarySales"].ConnectionString);
            OracleCommand objCmd = new OracleCommand();
            try
            {
                using (objCmd.Connection = con)
                {
                    objCmd.CommandText = "PKG_ORD_DLV_RET_COUNT_ONE.GEN_ORD_DLV_COUNT_CUSTOM";
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.Add("p_result", OracleDbType.Long, 35).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("p_s_date", OracleDbType.Varchar2).Value = cparam.fromDate;
                    objCmd.Parameters.Add("p_e_date", OracleDbType.Varchar2).Value = cparam.toDate;

                    objCmd.Connection.Open();
                    objCmd.ExecuteNonQuery();

                    lastGenerate = objCmd.Parameters["p_result"].Value.ToString();
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
            return lastGenerate;
        }

        public string GetLastGenDate()
        {
            string lastGenDate = string.Empty;

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["SecondarySales"].ConnectionString);

            con.Open();
            string query = "SELECT to_char(DATE_GEN,'dd/mm/yyyy') AS GEN_DATE FROM TEMP_ALL_BRND_RET_COUNT_30DAYS WHERE ROWNUM=1";

            OracleCommand cmd = new OracleCommand(query, con);

            DataTable dt = new DataTable();
            using (OracleDataAdapter a = new OracleDataAdapter(cmd))
            {
                a.Fill(dt);
            }
            con.Close();

            lastGenDate = dt.Rows[0].Field<string>("GEN_DATE");

            return lastGenDate;
        }
    }
}
