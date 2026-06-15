using CTGroup.Models.ViewModel.SystemCommon;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using CTGroup.Utility.Common;
using System.Configuration;
using System.Diagnostics;
using System;
using CTGroup.OracleModel.ViewModel.SystemCommon;

namespace CTGroup.Service.Costing.Factories
{
    public class ProductDetailsPerLCBasedMgt
    {
        public IEnumerable<vmProductCostingCmn> getAllCompanyList()
        {
            IEnumerable<vmProductCostingCmn> mCompanyList = null;
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_BASIC_INFO.Get_All_Company";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetData(objCmd);
            mCompanyList = ConvertDataTableToGenericList.BindList<vmProductCostingCmn>(dt);
            return mCompanyList;
        }
        public List<vmProductCostingCmn> LoadAllProduct(vmCmnParameters objcmnParam)
        {          
            List<vmProductCostingCmn> listProduct = new List<vmProductCostingCmn>();
            OracleCommand objCmd = new OracleCommand();
            // objCmd.CommandText = "cityn.p_sprod.select_sprod";
            objCmd.CommandText = "cityn.PRODUCT_COSTING.select_costing_rate";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("cur_01", OracleDbType.RefCursor).Direction = ParameterDirection.Output;


            if (objcmnParam.parameter == "SCOMPxxxxxxxxxxxxx13")
            {
                objCmd.Parameters.Add("call_name", OracleDbType.Varchar2).Value = "FG_DELV_PROD_FEED";
                objCmd.Parameters.Add("sel1", OracleDbType.Varchar2).Value = "";
            }
            else if (objcmnParam.parameter == "SCOMPxxxxxxxxxxxxx32")
            {
                objCmd.Parameters.Add("call_name", OracleDbType.Varchar2).Value = "FG_DELV_PROD_PACK";
                objCmd.Parameters.Add("sel1", OracleDbType.Varchar2).Value = "";
            }
            else
            {
                objCmd.Parameters.Add("call_name", OracleDbType.Varchar2).Value = "FG_DELV_PROD_BULK";
                objCmd.Parameters.Add("sel1", OracleDbType.Varchar2).Value = objcmnParam.parameter1;
            }
           
            objCmd.Parameters.Add("sel2", OracleDbType.Varchar2).Value = "";
            objCmd.Parameters.Add("sel3", OracleDbType.Varchar2).Value = "";
            objCmd.Parameters.Add("sel4", OracleDbType.Varchar2).Value = "";
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetData(objCmd);
            listProduct = ConvertDataTableToGenericList.BindList<vmProductCostingCmn>(dt);
            return listProduct;
        }
        public string SaveOpeningStock(vmCmnParameters objcmnParam, vmOpeningStock costing)
        {
            string result = string.Empty;
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
            try
            {
                using (OracleCommand objCmd = new OracleCommand())
                {
                    using (objCmd.Connection = con)
                    {
                        objCmd.CommandType = CommandType.StoredProcedure;

                        objCmd.CommandText = "PKG_PRODUCT_DETAILS_PER_LC.INSERT_OPENING_STOCK";

                        objCmd.Parameters.Add("P_COMPANYOID", OracleDbType.Varchar2).Value = costing.COMPANYOID;
                        objCmd.Parameters.Add("P_PRODUCTOID", OracleDbType.Varchar2).Value = costing.PRODUCTOID;
                        objCmd.Parameters.Add("P_STOCKDATE", OracleDbType.Date).Value = costing.STOCKDATE;
                        objCmd.Parameters.Add("P_OPENINGQUANTITY", OracleDbType.Decimal).Value = costing.OPENINGQUANTITY;
                        objCmd.Parameters.Add("P_OPENINGAMOUNT", OracleDbType.Decimal).Value = costing.OPENINGAMOUNT;
                        objCmd.Parameters.Add("P_CREATEBY", OracleDbType.Varchar2).Value = objcmnParam.loggeduser;
                        objCmd.Parameters.Add("P_CREATEPC", OracleDbType.Varchar2).Value = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(1).ToString();

                        objCmd.Connection.Open();
                        objCmd.ExecuteNonQuery();
                        objCmd.Connection.Close();
                        result = "1";
                    }
                }
            }
            catch (OracleException exception)
            {
                result = "0";
                var frame = new StackTrace(true).GetFrame(0);
                var filename = frame.GetFileName();
                var line = frame.GetFileLineNumber();

                //Utils u = new Utils();
                //u.LowWrite(exception, filename, line);
            }
            return result;
        }
     
        public List<vmOpeningStock> getOpenStockList(vmCmnParameters objcmnParam, out long recordsTotal)
        {
            List<vmOpeningStock> listOpenStock = null;
            recordsTotal = 0;

            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_PRODUCT_DETAILS_PER_LC.GET_OPENING_STOCK_LIST";
            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetData(objCmd);
            listOpenStock = ConvertDataTableToGenericList.BindList<vmOpeningStock>(dt);

            recordsTotal = listOpenStock.Count;
            return listOpenStock;
        }

        public string SaveLCReceive(vmCmnParameters objcmnParam, vmSrLcReceive costing)
        {
            string result = string.Empty;
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
            try
            {
                using (OracleCommand objCmd = new OracleCommand())
                {
                    using (objCmd.Connection = con)
                    {
                        objCmd.CommandType = CommandType.StoredProcedure;

                        objCmd.CommandText = "PKG_PRODUCT_DETAILS_PER_LC.INSERT_SR_LC_RECEIVE";

                        objCmd.Parameters.Add("P_COMPANYOID", OracleDbType.Varchar2).Value = costing.COMPANYOID;
                        objCmd.Parameters.Add("P_PRODUCTOID", OracleDbType.Varchar2).Value = costing.PRODUCTOID;
                        objCmd.Parameters.Add("P_RECEIVEDDATE", OracleDbType.Date).Value = costing.RECEIVEDDATE;
                        objCmd.Parameters.Add("P_RECEIVEDQUANTITY", OracleDbType.Decimal).Value = costing.RECEIVEDQUANTITY;
                        objCmd.Parameters.Add("P_CREATEBY", OracleDbType.Varchar2).Value = objcmnParam.loggeduser;
                        objCmd.Parameters.Add("P_CREATEPC", OracleDbType.Varchar2).Value = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(1).ToString();

                        objCmd.Connection.Open();
                        objCmd.ExecuteNonQuery();
                        objCmd.Connection.Close();
                        result = "1";
                    }
                }
            }
            catch (OracleException exception)
            {
                result = "0";
                var frame = new StackTrace(true).GetFrame(0);
                var filename = frame.GetFileName();
                var line = frame.GetFileLineNumber();

                //Utils u = new Utils();
                //u.LowWrite(exception, filename, line);
            }
            return result;
        }
    }
}

