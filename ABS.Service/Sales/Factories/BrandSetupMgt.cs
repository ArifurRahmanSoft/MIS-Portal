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
using Oracle.ManagedDataAccess.Types;

namespace CTGroup.Service.Sales.Factories
{
    public class BrandSetupMgt : iBrandSetupMgt
    {

        //------------------------------------------------------Start-------------------------------------

     

        

        string GetConString = ConfigurationManager.ConnectionStrings["lcmsget"].ConnectionString;
        string setConString = ConfigurationManager.ConnectionStrings["lcmsset"].ConnectionString;


        public string SaveUpdateProductDetails(string jsonDataMstr, string userId)
        {
            string result = string.Empty;
            using (OracleConnection con = new OracleConnection(setConString))
            {
                try
                {
                    con.Open();
                    using (OracleCommand objCmd = new OracleCommand("MIS_PORTAL.Set_ProductDetail", con))
                    {
                        objCmd.CommandType = CommandType.StoredProcedure;
                        objCmd.Parameters.Add("mresult", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.Output;
                        objCmd.Parameters.Add("JsonData_Mstr", OracleDbType.Clob).Value = jsonDataMstr;
                        objCmd.Parameters.Add("mCreateBy", OracleDbType.Varchar2, 100).Value = userId;
                        objCmd.ExecuteNonQuery();
                        result = objCmd.Parameters["mresult"].Value.ToString();
                    }
                }
                catch (Exception ex)
                {
                    result = "Error: " + ex.Message;
                }
            }

            return result;
        }

        public string DeleteProduct(string ProductId, string userId)
        {
            string result = string.Empty;
            using (OracleConnection con = new OracleConnection(setConString))
            {
                try
                {
                    con.Open();
                    using (OracleCommand objCmd = new OracleCommand("MIS_PORTAL.Delete_Product", con))
                    {
                        objCmd.CommandType = CommandType.StoredProcedure;
                        objCmd.Parameters.Add("mresult", OracleDbType.Varchar2, 1000).Direction = ParameterDirection.Output;
                        objCmd.Parameters.Add("ProdId", OracleDbType.Varchar2,50).Value = ProductId;
                        objCmd.Parameters.Add("mCreateBy", OracleDbType.Varchar2, 100).Value = userId;
                        objCmd.ExecuteNonQuery();
                        result = objCmd.Parameters["mresult"].Value.ToString();
                    }
                }
                catch (Exception ex)
                {
                    result = "Error: " + ex.Message;
                }
            }

            return result;
        }
        



        public object GetProductList(vmCmnParameters objcmnParam)
        {
            string result = string.Empty;

            OracleCommand objCmd = new OracleCommand();
            string Query = "MIS_PORTAL.Get_PRODUCTDETAILS_BY_PRODNAME_PRODCODE";
            objCmd.Parameters.Add("gresult", OracleDbType.Clob).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("gProductName", OracleDbType.Varchar2).Value = objcmnParam.parameter;
            objCmd.Parameters.Add("gProductCode", OracleDbType.Varchar2).Value = objcmnParam.parameter1;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            result = classDt.ExecuteNonQueryOutClob(Query, objCmd, "gresult", GetConString);
            return result;
        }

        
        public object GetProductDetail(string parameter)
        {
            string result = string.Empty;

            OracleCommand objCmd = new OracleCommand();
            string Query = "MIS_PORTAL.Get_PRODUCTDETAILS_BY_PRODCODE";
            objCmd.Parameters.Add("gresult", OracleDbType.Clob).Direction = ParameterDirection.Output;
            //objCmd.Parameters.Add("gProductName", OracleDbType.Varchar2).Value = objcmnParam.parameter;
            objCmd.Parameters.Add("gProductCode", OracleDbType.Varchar2).Value =parameter;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            result = classDt.ExecuteNonQueryOutClob(Query, objCmd, "gresult", GetConString);
            return result;
        }
        

        public object GetProductDetailById(string parameter)
        {
            string result = string.Empty;

            OracleCommand objCmd = new OracleCommand();
            string Query = "MIS_PORTAL.Get_PRODUCTDETAILS_BY_ID";
            objCmd.Parameters.Add("gresult", OracleDbType.Clob).Direction = ParameterDirection.Output;
            //objCmd.Parameters.Add("gProductName", OracleDbType.Varchar2).Value = objcmnParam.parameter;
            objCmd.Parameters.Add("gProductId", OracleDbType.Varchar2).Value = parameter;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            result = classDt.ExecuteNonQueryOutClob(Query, objCmd, "gresult", GetConString);
            return result;
        }


        public object GetProductListByPage()
        {
            string result = string.Empty;
            OracleCommand objCmd = new OracleCommand();
            string Query = "MIS_PORTAL.Get_SPRODUCT_LIST";
            objCmd.Parameters.Add("gresult", OracleDbType.Clob).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            result = classDt.ExecuteNonQueryOutClob(Query, objCmd, "gresult", GetConString);
            return result;
        }



        public object GetProductType()
        {
            string result = string.Empty;

            OracleCommand objCmd = new OracleCommand();
            string Query = "MIS_PORTAL.Get_SSTYP_Dropdown";
            objCmd.Parameters.Add("gresult", OracleDbType.Clob).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            result = classDt.ExecuteNonQueryOutClob(Query, objCmd, "gresult", GetConString);
            return result;
        }


        public object GetSconType()
        {
            string result = string.Empty;

            OracleCommand objCmd = new OracleCommand();
            string Query = "MIS_PORTAL.Get_SCON_TYP_Dropdown";
            objCmd.Parameters.Add("gresult", OracleDbType.Clob).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            result = classDt.ExecuteNonQueryOutClob(Query, objCmd, "gresult", GetConString);
            return result;
        }


        public object GetSpcat()
        {
            string result = string.Empty;

            OracleCommand objCmd = new OracleCommand();
            string Query = "MIS_PORTAL.Get_SPCAT_Dropdown";
            objCmd.Parameters.Add("gresult", OracleDbType.Clob).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            result = classDt.ExecuteNonQueryOutClob(Query, objCmd, "gresult", GetConString);
            return result;
        }

        
        public object GetSmunt()
        {
            string result = string.Empty;

            OracleCommand objCmd = new OracleCommand();
            string Query = "MIS_PORTAL.Get_SMUNT_Dropdown";
            objCmd.Parameters.Add("gresult", OracleDbType.Clob).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            result = classDt.ExecuteNonQueryOutClob(Query, objCmd, "gresult", GetConString);
            return result;
        }
        
        public object GetSBrand()
        {
            string result = string.Empty;

            OracleCommand objCmd = new OracleCommand();
            string Query = "MIS_PORTAL.Get_SBRND_Dropdown";
            objCmd.Parameters.Add("gresult", OracleDbType.Clob).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            result = classDt.ExecuteNonQueryOutClob(Query, objCmd, "gresult", GetConString);
            return result;
        }
        
        public object GetDoGroup()
        {
            string result = string.Empty;

            OracleCommand objCmd = new OracleCommand();
            string Query = "MIS_PORTAL.Get_SDOGP_Dropdown";
            objCmd.Parameters.Add("gresult", OracleDbType.Clob).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            result = classDt.ExecuteNonQueryOutClob(Query, objCmd, "gresult", GetConString);
            return result;
        }
        
        public object GetProductGroup()
        {
            string result = string.Empty;

            OracleCommand objCmd = new OracleCommand();
            string Query = "MIS_PORTAL.Get_SPROG_Dropdown";
            objCmd.Parameters.Add("gresult", OracleDbType.Clob).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            result = classDt.ExecuteNonQueryOutClob(Query, objCmd, "gresult", GetConString);
            return result;
        }

        
        public object GetProductSize()
        {
            string result = string.Empty;

            OracleCommand objCmd = new OracleCommand();
            string Query = "MIS_PORTAL.Get_PSIZE_Dropdown";
            objCmd.Parameters.Add("gresult", OracleDbType.Clob).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            result = classDt.ExecuteNonQueryOutClob(Query, objCmd, "gresult", GetConString);
            return result;
        }

        public object GetProductSerial()
        {
            string result = string.Empty;

            OracleCommand objCmd = new OracleCommand();
            string Query = "MIS_PORTAL.Get_SPROD_SERIL_Dropdown";
            objCmd.Parameters.Add("gresult", OracleDbType.Clob).Direction = ParameterDirection.Output;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            result = classDt.ExecuteNonQueryOutClob(Query, objCmd, "gresult", GetConString);
            return result;
        }



        public string SaveUpdateLitem(string jsonDataMstr, string userId)
        {
            string result = string.Empty;
            using (OracleConnection con = new OracleConnection(setConString))
            {
                try
                {
                    con.Open();
                    using (OracleCommand objCmd = new OracleCommand("MIS_PORTAL.Set_Litem", con))
                    {
                        objCmd.CommandType = CommandType.StoredProcedure;
                        objCmd.Parameters.Add("mresult", OracleDbType.Varchar2, 400).Direction = ParameterDirection.Output;
                        objCmd.Parameters.Add("JsonData_Mstr", OracleDbType.Clob).Value = jsonDataMstr;
                        objCmd.Parameters.Add("mCreateBy", OracleDbType.Varchar2, 100).Value = userId;
                        objCmd.ExecuteNonQuery();
                        result = objCmd.Parameters["mresult"].Value.ToString();
                    }
                }
                catch (Exception ex)
                {
                    result = "Error: " + ex.Message;
                }
            }

            return result;
        }

        
        public object GetLitemDetail(string parameter)
        {
            string result = string.Empty;

            OracleCommand objCmd = new OracleCommand();
            string Query = "MIS_PORTAL.Get_Litem_BY_ID";
            objCmd.Parameters.Add("gresult", OracleDbType.Clob).Direction = ParameterDirection.Output;
            //objCmd.Parameters.Add("gProductName", OracleDbType.Varchar2).Value = objcmnParam.parameter;
            objCmd.Parameters.Add("gLitemID", OracleDbType.Varchar2).Value = parameter;
            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            result = classDt.ExecuteNonQueryOutClob(Query, objCmd, "gresult", GetConString);
            return result;
        }





        //----------------------------------------------------------End-----------------------------------




    }
}
