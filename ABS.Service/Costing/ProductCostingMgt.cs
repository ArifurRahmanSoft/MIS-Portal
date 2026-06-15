using CTGroup.Models.ViewModel.SystemCommon;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using CTGroup.Utility.Common;
using System.Configuration;
using System.Diagnostics;
using System;

namespace CTGroup.Service.Costing.Factories
{
    public class ProductCostingMgt
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
        public string SaveProductCostingRate(vmCmnParameters objcmnParam, vmSprodCostRate costing)
        {
            string result = string.Empty;
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
            OracleCommand objCmd = new OracleCommand();
            try
            {
                    using (objCmd.Connection = con)
                    {
                        objCmd.CommandType = CommandType.StoredProcedure;

                        objCmd.CommandText = "cityn.PRODUCT_COSTING.INSERT_SPROD_COST_RATE";

                        objCmd.Parameters.Add("P_COST_TRDT", OracleDbType.Date).Value = DateTime.Now;
                        objCmd.Parameters.Add("P_COST_SCOMP", OracleDbType.Varchar2).Value = costing.COST_SCOMP;
                        objCmd.Parameters.Add("P_COST_SPROD", OracleDbType.Varchar2).Value = costing.COST_SPROD;
                        objCmd.Parameters.Add("P_COST_RATE", OracleDbType.Decimal).Value = costing.COST_RATE;
                        objCmd.Parameters.Add("P_IUSER", OracleDbType.Varchar2).Value = objcmnParam.loggeduser;

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

                //Utils u = new Utils();
                //u.LowWrite(exception, filename, line);
            }
            finally
            {
                objCmd.Connection.Close();
            }
            return result;
        }
    }
}

