using CTGroup.Models.ViewModel.Sales;
using CTGroup.Service.Sales.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using CTGroup.Models.ViewModel.SystemCommon;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Configuration;
using CTGroup.Utility.Common;
using CTGroup.OracleModel.ViewModel.SystemCommon;
using CTGroup.OracleModel.ViewModel.Sales;
using ABS.Service;
using CTGroup.Utility;

namespace CTGroup.Service.Sales.Factories
{
    public class ProductsRateMgt : iProductsRateMgt
    {
        public IEnumerable<vmIncentiveFormulaSetupMaster> GetIncentiveFormulaSetupMaster(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmIncentiveFormulaSetupMaster> objvmPIMaster = null;
            IEnumerable<vmIncentiveFormulaSetupMaster> objvmPIMasterWithOutPaging = null;
            recordsTotal = 0;

            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "INCENTIVE.Get_DistTargetMaster";
            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetData(objCmd);
            objvmPIMasterWithOutPaging = ConvertDataTableToGenericList.BindList<vmIncentiveFormulaSetupMaster>(dt);
            objvmPIMaster = objvmPIMasterWithOutPaging.OrderByDescending(x => x.INCEN_ID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();

            recordsTotal = objvmPIMasterWithOutPaging.Count();
            return objvmPIMaster;
        }

        public string SaveUpdateProductRate(vmProductRate itemMaster, List<vmProductRate> itemDetails, vmCmnParameters objcmnParam)
        {
            itemMaster.VERIFIED_BY = "1";
            itemMaster.APPROVED_BY = "1";
            itemMaster.APPROVED_DATE = DateTime.Now;
            itemMaster.ENTRY_BY = objcmnParam.loggeduser;
            itemMaster.ENTRY_DATE = DateTime.Now;
            itemMaster.UPDATE_BY = "x";
            itemMaster.UPDATE_DATE = DateTime.Now;
            itemMaster.VERSION = 1;
            itemMaster.DT = DateTime.Now;
            itemMaster.COMP_ID = "1";
            itemMaster.LOC_ID = "1";
            itemMaster.DEVICE_ID = HostService.GetLocalIPAddress();
            string ProductRateDetails = "";
            string RATE_ID = string.Empty;

            ProductRateDetails = GetDetails(itemDetails);

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
            OracleCommand objCmd = new OracleCommand();

            try
            {
                using (objCmd.Connection = con)
                {
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.CommandText = (itemMaster.DIST_OID == "" || itemMaster.DIST_OID == null) ? "CITYN.PKG_SALES_RATE_SETUP.INS_SALES_RATE" : "SALESSMS.PKG_DIST_RATE_SETUP.INS_DIST_SALES_RATE";

                    objCmd.Parameters.Add("p_OID", OracleDbType.Varchar2, 35).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("p_REF_NUMBER", OracleDbType.Varchar2, 20).Value = itemMaster.REF_NUMBER;

                    if (itemMaster.DIST_OID != "" && itemMaster.DIST_OID != null)
                    {
                        objCmd.Parameters.Add("p_DIST_ID", OracleDbType.Varchar2, 20).Value = itemMaster.DIST_OID;
                    }

                    objCmd.Parameters.Add("p_ACTIVE_DATE", OracleDbType.Varchar2).Value = itemMaster.ACTIVE_DATE.Date.ToString("dd/MM/yyyy");
                    objCmd.Parameters.Add("p_CLOSE_DATE", OracleDbType.Varchar2).Value = itemMaster.CLOSE_DATE.Date.ToString("dd/MM/yyyy");
                    objCmd.Parameters.Add("p_rate_details", OracleDbType.Varchar2, 10000).Value = ProductRateDetails;
                    objCmd.Parameters.Add("p_VERIFIED_BY", OracleDbType.Varchar2, 20).Value = itemMaster.VERIFIED_BY;
                    objCmd.Parameters.Add("p_APPROVED_BY", OracleDbType.Varchar2, 20).Value = itemMaster.APPROVED_BY;
                    objCmd.Parameters.Add("p_APPROVED_DATE", OracleDbType.Varchar2).Value = itemMaster.APPROVED_DATE.Date.ToString("dd/MM/yyyy");
                    objCmd.Parameters.Add("p_ENTRY_BY", OracleDbType.Varchar2, 20).Value = itemMaster.ENTRY_BY;
                    objCmd.Parameters.Add("p_ENTRY_DATE", OracleDbType.Varchar2).Value = itemMaster.ENTRY_DATE.Date.ToString("dd/MM/yyyy");
                    objCmd.Parameters.Add("p_UPDATE_BY", OracleDbType.Varchar2, 20).Value = itemMaster.UPDATE_BY;
                    objCmd.Parameters.Add("p_UPDATE_DATE", OracleDbType.Varchar2).Value = itemMaster.UPDATE_DATE.Date.ToString("dd/MM/yyyy");
                    objCmd.Parameters.Add("p_COMP_ID", OracleDbType.Varchar2, 20).Value = itemMaster.COMP_ID;
                    objCmd.Parameters.Add("p_LOC_ID", OracleDbType.Varchar2, 20).Value = itemMaster.LOC_ID;
                    objCmd.Parameters.Add("p_DEVICE_ID", OracleDbType.Varchar2, 20).Value = itemMaster.DEVICE_ID;

                    objCmd.Connection.Open();
                    objCmd.ExecuteNonQuery();

                    RATE_ID = objCmd.Parameters["p_OID"].Value.ToString();
                }
            }
            catch (OracleException exception)
            {
                Console.WriteLine(exception.Message);
                // may be you shouldn't return 0 here possibly throw;
            }
            finally
            {
                objCmd.Connection.Close();
            }
            return RATE_ID;
        }
        public string GetDetails(List<vmProductRate> itemDetails)
        {
            string details = "";
            foreach (var item in itemDetails)
            {
                string detail = "";
                string PRODUCT_BRAND_ID = "";
                string PRODUCT_SKU_ID = "";
                decimal? DP_CTN = 0;
                decimal? DP_PCS = 0;
                decimal? TP_CTN = 0;
                decimal? TP_PCS = 0;
                decimal? MRP_CTN = 0;
                decimal? MRP_PCS = 0;
                decimal? SWP_CTN = 0;
                decimal? SWP_PCS = 0;
                //string REMARKS = "";


                PRODUCT_BRAND_ID = item.BRANDOID;
                PRODUCT_SKU_ID = item.PRODUCTOID;
                DP_CTN = item.DP_CTN;
                DP_PCS = item.DP_PCS;
                TP_CTN = item.TP_CTN;
                TP_PCS = item.TP_PCS;
                MRP_CTN = item.MRP_CTN;
                MRP_PCS = item.MRP_PCS;
                SWP_CTN = item.SWP_CTN;
                SWP_PCS = item.SWP_PCS;
                //REMARKS = "N/A";

                detail = "x" + ':' + PRODUCT_BRAND_ID + ':' + PRODUCT_SKU_ID + ':' + DP_PCS + ':'
                    + TP_PCS + ':' + MRP_PCS + ':' + SWP_PCS + ';';
                details += detail;
            }
            return details;
        }

        public IEnumerable<vmBrandSKU> GetBrand(vmCmnParameters objcmnParam)
        {
            IEnumerable<vmBrandSKU> objBrand = null;

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);

            con.Open();
            string query = "SELECT DISTINCT BRAND.OID BRANDID, BRAND.SBRND_NAME FROM CITYN.T_SPROD PRODUCT " +
                "INNER JOIN CITYN.T_SBRND BRAND ON PRODUCT.SPROD_SBRND = BRAND.OID WHERE " +
                "CASE WHEN '" + objcmnParam.parameter + "'='SSTYPxxxxxxxxxxxx01' THEN 1 " +
                "WHEN SPROD_SSTYP = '" + objcmnParam.parameter + "' THEN 1 ELSE 0 END=1 " +
                "ORDER BY BRAND.SBRND_NAME";

            OracleCommand cmd = new OracleCommand(query, con);

            DataTable t1 = new DataTable();
            using (OracleDataAdapter a = new OracleDataAdapter(cmd))
            {
                a.Fill(t1);
            }
            con.Close();

            objBrand = t1.AsEnumerable().Select(dataRow => new vmBrandSKU
            {
                BRANDID = dataRow.Field<string>("BRANDID"),
                SBRND_NAME = dataRow.Field<string>("SBRND_NAME")
            }).ToList();

            return objBrand;
        }

        public IEnumerable<vmDistributor> GetSingleDistributor(vmCmnParameters objcmnParam)
        {
            IEnumerable<vmDistributor> objDistributor = null;

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);

            con.Open();
            string query = "   SELECT DISTINCT  dist.OID DIST_ID, dist.SCUST_INFO_TEXT DIST_CODE,  " +
                "concat(concat( dist.SCUST_INFO_TEXT, ' - '), dist.SCUST_INFO_NAME) DIST_NAME " +
                "FROM CITYN.T_SCUST_INFO dist where SCUST_INFO_TEXT = '018966' ";

            OracleCommand cmd = new OracleCommand(query, con);

            DataTable t1 = new DataTable();
            using (OracleDataAdapter a = new OracleDataAdapter(cmd))
            {
                a.Fill(t1);
            }
            con.Close();

            objDistributor = t1.AsEnumerable().Select(dataRow => new vmDistributor
            {
                OID = dataRow.Field<string>("DIST_ID"),
                SCUST_INFO_NAME = dataRow.Field<string>("DIST_NAME")
            }).ToList();

            return objDistributor;
        }


        public object GetProductType()
        {
            string result = string.Empty;

            OracleCommand objCmd = new OracleCommand();
            string Query = "MIS_PORTAL.Get_SSTYP_Dropdown";
            objCmd.Parameters.Add("gresult", OracleDbType.Clob).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            result = classDt.ExecuteNonQueryOutClob(Query, objCmd, "gresult", conString);
            return result;
        }




        string conString = ConfigurationManager.ConnectionStrings["productInsert"].ConnectionString;
        public object GetSconType()
        {
            string result = string.Empty;

            OracleCommand objCmd = new OracleCommand();
            string Query = "MIS_PORTAL.Get_SCON_TYP_Dropdown";
            objCmd.Parameters.Add("gresult", OracleDbType.Clob).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            result = classDt.ExecuteNonQueryOutClob(Query, objCmd, "gresult", conString);
            return result;
        }


        public object GetSpcatType()
        {
            string result = string.Empty;

            OracleCommand objCmd = new OracleCommand();
            string Query = "MIS_PORTAL.Get_SPCAT_Dropdown";
            objCmd.Parameters.Add("gresult", OracleDbType.Clob).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            result = classDt.ExecuteNonQueryOutClob(Query, objCmd, "gresult", conString);
            return result;
        }










    }
}
