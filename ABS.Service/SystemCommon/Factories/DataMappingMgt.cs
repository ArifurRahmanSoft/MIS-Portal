using ABS.Utility;
using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel.ViewModel.SystemCommon;
using CTGroup.Service.SystemCommon.Interfaces;
using CTGroup.Utility;
using CTGroup.Utility.Common;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.Service.SystemCommon.Factories
{
    public class DataMappingMgt : iDataMappingMgt
    {
        public DataTable GetByPage(vmDataMapping data)
        {
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            if (data.TRAN_TYPE_ID == "1") //Company Wise SSType Setup For area less menue
            {
                objCmd.CommandText = "DATA_MAPPING.GET_COMPANY_SSTYP";
                objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("COMPANY_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.COMPANY_OID) ? null : data.COMPANY_OID;
                objCmd.Parameters.Add("SSTYP_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.SSTYP_OID) ? null : data.SSTYP_OID;
                objCmd.Parameters.Add("PageNumber", OracleDbType.Decimal).Value = data.pageNumber;
                objCmd.Parameters.Add("PageSize", OracleDbType.Decimal).Value = data.pageSize;
            }
            else if (data.TRAN_TYPE_ID == "2")//Sales Group Wise Product Group Setup For area less menue (Many to many)
            {
                objCmd.CommandText = "DATA_MAPPING.GET_SDVNT_SPROG";
                objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("SDVNT_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.SDVNT_OID) ? null : data.SDVNT_OID;
                objCmd.Parameters.Add("SPROG_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.SDVNT_OID) ? null : data.SPROG_OID;
                objCmd.Parameters.Add("PageNumber", OracleDbType.Decimal).Value = data.pageNumber;
                objCmd.Parameters.Add("PageSize", OracleDbType.Decimal).Value = data.pageSize;
            }
            else if (data.TRAN_TYPE_ID == "3")//User, Group and Brand Setup For area less menue (Many to many)
            {
                objCmd.CommandText = "DATA_MAPPING.GET_USER_GROUP_BRAND";
                objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("USER_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.USER_OID) ? null : data.USER_OID;
                objCmd.Parameters.Add("GROUP_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.GROUP_OID) ? null : data.GROUP_OID;
                objCmd.Parameters.Add("BRAND_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.BRAND_OID) ? null : data.BRAND_OID;
                objCmd.Parameters.Add("PageNumber", OracleDbType.Decimal).Value = data.pageNumber;
                objCmd.Parameters.Add("PageSize", OracleDbType.Decimal).Value = data.pageSize;
            }
            else if (data.TRAN_TYPE_ID == "4")//User, Group, Brand and SKU  Setup For area less menue (Many to many)
            {
                objCmd.CommandText = "DATA_MAPPING.GET_USER_GROUP_BRAND_SKU";
                objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("USER_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.USER_OID) ? null : data.USER_OID;
                objCmd.Parameters.Add("GROUP_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.GROUP_OID) ? null : data.GROUP_OID;
                objCmd.Parameters.Add("BRAND_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.BRAND_OID) ? null : data.BRAND_OID;
                objCmd.Parameters.Add("SKU_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.SKU_OID) ? null : data.SKU_OID;
                objCmd.Parameters.Add("PageNumber", OracleDbType.Decimal).Value = data.pageNumber;
                objCmd.Parameters.Add("PageSize", OracleDbType.Decimal).Value = data.pageSize;
            }
            else if (data.TRAN_TYPE_ID == "5")//National Setup-Non Sales Person For National Hierarchy
            {
                objCmd.CommandText = "DATA_MAPPING.GET_USER_REPORT_PERMISSION_NONSP";
                objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("USER_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.USER_OID) ? null : data.USER_OID;
                objCmd.Parameters.Add("NTNAL_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.NTNAL_OID) ? null : data.NTNAL_OID;
                objCmd.Parameters.Add("ROLE_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.ROLE_OID) ? null : data.ROLE_OID;
                objCmd.Parameters.Add("PageNumber", OracleDbType.Decimal).Value = data.pageNumber;
                objCmd.Parameters.Add("PageSize", OracleDbType.Decimal).Value = data.pageSize;
            }
            else if (data.TRAN_TYPE_ID == "6")//Create MIS Portal Temporary User
            {
                objCmd.CommandText = "DATA_MAPPING.GET_TEMP_USER_MIS_PORTAL";
                objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("USER_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.USER_OID) ? null : data.USER_OID;
                objCmd.Parameters.Add("USER_FULLNAME", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.USER_FULLNAME) ? null : data.USER_FULLNAME;
                objCmd.Parameters.Add("PageNumber", OracleDbType.Decimal).Value = data.pageNumber;
                objCmd.Parameters.Add("PageSize", OracleDbType.Decimal).Value = data.pageSize;
            }
            else if (data.TRAN_TYPE_ID == "7")//Create MIS Portal User Menu Mode Permission
            {
                objCmd.CommandText = "DATA_MAPPING.GET_USERMENUMODE_PERMISSION";
                objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("USER_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.USER_OID) ? null : data.USER_OID;
                objCmd.Parameters.Add("MODULE_ID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.CMN_OID) ? null : data.CMN_OID;
                objCmd.Parameters.Add("PageNumber", OracleDbType.Decimal).Value = data.pageNumber;
                objCmd.Parameters.Add("PageSize", OracleDbType.Decimal).Value = data.pageSize;
            }
            else if (data.TRAN_TYPE_ID == "8")//Create MIS Portal User Menu Mode Permission
            {
                objCmd.CommandText = "DATA_MAPPING.GET_USER_SDVNT";
                objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("USER_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.USER_OID) ? null : data.USER_OID;
                objCmd.Parameters.Add("SDVNT_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.SDVNT_OID) ? null : data.SDVNT_OID;
                objCmd.Parameters.Add("PageNumber", OracleDbType.Decimal).Value = data.pageNumber;
                objCmd.Parameters.Add("PageSize", OracleDbType.Decimal).Value = data.pageSize;
            }
            else if (data.TRAN_TYPE_ID == "9")//Create MIS Portal Temporary User
            {
                objCmd.CommandText = "DATA_MAPPING.GET_USER_SALE_SOFT";
                objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("USER_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.USER_OID) ? null : data.USER_OID;
                objCmd.Parameters.Add("USER_FULLNAME", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.USER_FULLNAME) ? null : data.USER_FULLNAME;
                objCmd.Parameters.Add("PageNumber", OracleDbType.Decimal).Value = data.pageNumber;
                objCmd.Parameters.Add("PageSize", OracleDbType.Decimal).Value = data.pageSize;
            }
            else if (data.TRAN_TYPE_ID == "10")//Sales Group Wise Product Group Setup For area less menue (Many to many)
            {
                objCmd.CommandText = "DATA_MAPPING.GET_SDVNT_SPROD";
                objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("SDVNT_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.SDVNT_OID) ? null : data.SDVNT_OID;
                objCmd.Parameters.Add("PROD_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.SKU_OID) ? null : data.SKU_OID;
                objCmd.Parameters.Add("PageNumber", OracleDbType.Decimal).Value = data.pageNumber;
                objCmd.Parameters.Add("PageSize", OracleDbType.Decimal).Value = data.pageSize;
            }

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetData(objCmd);

            return dt;
        }

        public string SaveUpdate(vmDataMapping data)
        {
            string result = string.Empty;
            var util = new Utils();
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            using (objCmd.Connection = con)
            {
                if (data.TRAN_TYPE_ID == "1") //Company Wise SSType Setup For area less menue
                {
                    objCmd.CommandText = "DATA_MAPPING.SET_COMPANY_SSTYP";
                    objCmd.Parameters.Add("P_RESULT", OracleDbType.Varchar2, 20).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("COMPANY_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.COMPANY_OID) ? null : data.COMPANY_OID;
                    objCmd.Parameters.Add("SSTYP_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.SSTYP_OID) ? null : data.SSTYP_OID;
                }
                else if (data.TRAN_TYPE_ID == "2")//Sales Group Wise Product Group Setup For area less menue (Many to many)
                {
                    objCmd.CommandText = "DATA_MAPPING.SET_SDVNT_SPROG";
                    objCmd.Parameters.Add("P_RESULT", OracleDbType.Varchar2, 20).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("SDVNT_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.SDVNT_OID) ? null : data.SDVNT_OID;
                    objCmd.Parameters.Add("SPROG_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.SPROG_OID) ? null : data.SPROG_OID;
                }
                else if (data.TRAN_TYPE_ID == "3")//User, Group and Brand Setup For area less menue (Many to many)
                {
                    objCmd.CommandText = "DATA_MAPPING.SET_USER_GROUP_BRAND";
                    objCmd.Parameters.Add("P_RESULT", OracleDbType.Varchar2, 20).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("USER_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.USER_OID) ? null : data.USER_OID;
                    objCmd.Parameters.Add("GROUP_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.GROUP_OID) ? null : data.GROUP_OID;
                    objCmd.Parameters.Add("BRAND_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.BRAND_OID) ? null : data.BRAND_OID;
                }
                else if (data.TRAN_TYPE_ID == "4")//User, Group, Brand and SKU  Setup For area less menue (Many to many)
                {
                    objCmd.CommandText = "DATA_MAPPING.SET_USER_GROUP_BRAND_SKU";
                    objCmd.Parameters.Add("P_RESULT", OracleDbType.Varchar2, 20).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("USER_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.USER_OID) ? null : data.USER_OID;
                    objCmd.Parameters.Add("GROUP_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.GROUP_OID) ? null : data.GROUP_OID;
                    objCmd.Parameters.Add("BRAND_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.BRAND_OID) ? null : data.BRAND_OID;
                    objCmd.Parameters.Add("SKU_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.SKU_OID) ? null : data.SKU_OID;
                }
                else if (data.TRAN_TYPE_ID == "5")//National Setup-Non Sales Person For National Hierarchy
                {
                    objCmd.CommandText = "DATA_MAPPING.SET_USER_REPORT_PERMISSION_NONSP";
                    objCmd.Parameters.Add("P_RESULT", OracleDbType.Varchar2, 20).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("USER_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.USER_OID) ? null : data.USER_OID;
                    objCmd.Parameters.Add("NTNAL_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.NTNAL_OID) ? null : data.NTNAL_OID;
                    objCmd.Parameters.Add("ROLE_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.ROLE_OID) ? null : data.ROLE_OID;
                    objCmd.Parameters.Add("IS_ENABLE", OracleDbType.Varchar2).Value = util.BoolVal(data.IS_ENABLE);
                    objCmd.Parameters.Add("IS_UPDATE", OracleDbType.Varchar2).Value = util.BoolVal(data.IS_UPDATE);
                }
                else if (data.TRAN_TYPE_ID == "6")//Create MIS Portal Temporary User
                {
                    string encryptedPassword = string.IsNullOrEmpty(data.USER_PASS) ? null : EncryptAndDecrypt.Encrypt(data.USER_PASS, "sblw-3hn8-sqoy20");
                    objCmd.CommandText = "DATA_MAPPING.SET_PUT_TEMP_USER_MIS_PORTAL";
                    objCmd.Parameters.Add("P_RESULT", OracleDbType.Varchar2, 20).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("USER_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.USER_OID) ? null : data.USER_OID;
                    objCmd.Parameters.Add("USER_FULLNAME", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.USER_FULLNAME) ? null : data.USER_FULLNAME;
                    objCmd.Parameters.Add("USER_HASHPASS", OracleDbType.Varchar2).Value = encryptedPassword;
                    objCmd.Parameters.Add("USER_PASSWORD", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.USER_PASS) ? null : data.USER_PASS;
                    objCmd.Parameters.Add("IS_ACTIVE", OracleDbType.Decimal).Value = util.BoolVal(data.IS_ENABLE);
                    objCmd.Parameters.Add("ENTRYBY", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.LOGGED_USER) ? null : data.LOGGED_USER;
                    objCmd.Parameters.Add("ENTRYPC", OracleDbType.Varchar2).Value = HostService.GetLocalIPAddress();
                    objCmd.Parameters.Add("IS_UPDATE", OracleDbType.Varchar2).Value = util.BoolVal(data.IS_UPDATE);
                }
                else if (data.TRAN_TYPE_ID == "7")//Create MIS Portal Temporary User
                {
                    objCmd.CommandText = "DATA_MAPPING.SET_PUT_USERMENUMODE_PERMISSION";
                    objCmd.Parameters.Add("P_RESULT", OracleDbType.Varchar2, 20).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("JsonData", OracleDbType.Clob).Value = data.JSON_DATA;
                }
                else if (data.TRAN_TYPE_ID == "8")//Sales Group Wise Product Group Setup For area less menue (Many to many)
                {
                    objCmd.CommandText = "DATA_MAPPING.SET_USER_SDVNT";
                    objCmd.Parameters.Add("P_RESULT", OracleDbType.Varchar2, 20).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("USER_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.USER_OID) ? null : data.USER_OID;
                    objCmd.Parameters.Add("SDVNT_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.SDVNT_OID) ? null : data.SDVNT_OID;
                    objCmd.Parameters.Add("IS_ACTIVE", OracleDbType.Varchar2).Value = util.BoolVal(data.IS_ENABLE);
                }
                else if (data.TRAN_TYPE_ID == "9")//Sales Group Wise Product Group Setup For area less menue (Many to many)
                {
                    objCmd.CommandText = "DATA_MAPPING.SET_PUT_USER_SALE_SOFT_V3";
                    objCmd.Parameters.Add("P_RESULT", OracleDbType.Varchar2, 20).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("USER_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.USER_OID) ? null : data.USER_OID;
                    objCmd.Parameters.Add("USER_FULLNAME", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.USER_FULLNAME) ? null : data.USER_FULLNAME;
                    objCmd.Parameters.Add("USER_PASSWORD", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.USER_PASS) ? null : data.USER_PASS;
                    objCmd.Parameters.Add("IS_ACTIVE", OracleDbType.Decimal).Value = util.BoolVal(data.IS_ENABLE);
                    objCmd.Parameters.Add("ENTRYBY", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.LOGGED_USER) ? null : data.LOGGED_USER;
                    objCmd.Parameters.Add("ENTRYPC", OracleDbType.Varchar2).Value = HostService.GetLocalIPAddress();
                    objCmd.Parameters.Add("IS_UPDATE", OracleDbType.Varchar2).Value = util.BoolVal(data.IS_UPDATE);
                }
                else if (data.TRAN_TYPE_ID == "10")//Sales Group Wise Product Group Setup For area less menue (Many to many)
                {
                    objCmd.CommandText = "DATA_MAPPING.SET_SDVNT_SPROD";
                    objCmd.Parameters.Add("P_RESULT", OracleDbType.Varchar2, 20).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("SDVNT_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.SDVNT_OID) ? null : data.SDVNT_OID;
                    objCmd.Parameters.Add("SDVNT_SCTYP", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.CMN_OID) ? null : data.CMN_OID;
                    objCmd.Parameters.Add("SPROD_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.SKU_OID) ? null : data.SKU_OID;
                    objCmd.Parameters.Add("SDVNT_SCOMP", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.COMPANY_OID) ? null : data.COMPANY_OID;
                    objCmd.Parameters.Add("IS_ACTIVE", OracleDbType.Varchar2).Value = util.BoolVal(data.IS_ENABLE);
                }

                objCmd.Connection.Open();
                objCmd.ExecuteNonQuery();
                objCmd.Connection.Close();

                result = objCmd.Parameters["P_RESULT"].Value.ToString();
            }

            return result;
        }

        public string Delete(vmDataMapping data)
        {
            string result = string.Empty;

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            using (objCmd.Connection = con)
            {
                if (data.TRAN_TYPE_ID == "1") //Company Wise SSType Setup For area less menue
                {
                    objCmd.CommandText = "DATA_MAPPING.DELETE_COMPANY_SSTYP";
                    objCmd.Parameters.Add("P_RESULT", OracleDbType.Varchar2, 20).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("P_COMPANY_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.COMPANY_OID) ? null : data.COMPANY_OID;
                    objCmd.Parameters.Add("P_SSTYP_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.SSTYP_OID) ? null : data.SSTYP_OID;
                }
                else if (data.TRAN_TYPE_ID == "2")//Sales Group Wise Product Group Setup For area less menue (Many to many)
                {
                    objCmd.CommandText = "DATA_MAPPING.DELETE_SDVNT_SPROG";
                    objCmd.Parameters.Add("P_RESULT", OracleDbType.Varchar2, 20).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("SDVNT_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.SDVNT_OID) ? null : data.SDVNT_OID;
                    objCmd.Parameters.Add("SPROG_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.SPROG_OID) ? null : data.SPROG_OID;
                }
                else if (data.TRAN_TYPE_ID == "3")//User, Group and Brand Setup For area less menue (Many to many)
                {
                    objCmd.CommandText = "DATA_MAPPING.DELETE_USER_GROUP_BRAND";
                    objCmd.Parameters.Add("P_RESULT", OracleDbType.Varchar2, 20).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("USER_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.USER_OID) ? null : data.USER_OID;
                    objCmd.Parameters.Add("GROUP_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.GROUP_OID) ? null : data.GROUP_OID;
                    objCmd.Parameters.Add("BRAND_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.BRAND_OID) ? null : data.BRAND_OID;
                }
                else if (data.TRAN_TYPE_ID == "4")//User, Group, Brand and SKU  Setup For area less menue (Many to many)
                {
                    objCmd.CommandText = "DATA_MAPPING.DELETE_USER_GROUP_BRAND_SKU";
                    objCmd.Parameters.Add("P_RESULT", OracleDbType.Varchar2, 20).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("USER_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.USER_OID) ? null : data.USER_OID;
                    objCmd.Parameters.Add("GROUP_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.GROUP_OID) ? null : data.GROUP_OID;
                    objCmd.Parameters.Add("BRAND_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.BRAND_OID) ? null : data.BRAND_OID;
                    objCmd.Parameters.Add("SKU_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.SKU_OID) ? null : data.SKU_OID;
                }
                else if (data.TRAN_TYPE_ID == "5")//National Setup-Non Sales Person For National Hierarchy
                {
                    objCmd.CommandText = "DATA_MAPPING.DELETE_USER_REPORT_PERMISSION_NONSP";
                    objCmd.Parameters.Add("P_RESULT", OracleDbType.Varchar2, 20).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("USER_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.USER_OID) ? null : data.USER_OID;
                    objCmd.Parameters.Add("NTNAL_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.NTNAL_OID) ? null : data.NTNAL_OID;
                    objCmd.Parameters.Add("ROLE_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.ROLE_OID) ? null : data.ROLE_OID;
                }
                else if (data.TRAN_TYPE_ID == "6")//Create MIS Portal Temporary User
                {
                    objCmd.CommandText = "DATA_MAPPING.DELETE_TEMP_USER_MIS_PORTAL";
                    objCmd.Parameters.Add("P_RESULT", OracleDbType.Varchar2, 20).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("USER_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.USER_OID) ? null : data.USER_OID;
                }
                else if (data.TRAN_TYPE_ID == "8")//Create MIS Portal Temporary User
                {
                    objCmd.CommandText = "DATA_MAPPING.DELETE_USER_SDVNT";
                    objCmd.Parameters.Add("P_RESULT", OracleDbType.Varchar2, 20).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("USER_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.USER_OID) ? null : data.USER_OID;
                    objCmd.Parameters.Add("SDVNT_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.SDVNT_OID) ? null : data.SDVNT_OID;
                }
                else if (data.TRAN_TYPE_ID == "9")//Create MIS Portal Temporary User
                {
                    objCmd.CommandText = "DATA_MAPPING.DELETE_USER_SDVNT";
                    objCmd.Parameters.Add("P_RESULT", OracleDbType.Varchar2, 20).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("USER_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.USER_OID) ? null : data.USER_OID;
                    objCmd.Parameters.Add("SDVNT_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.SDVNT_OID) ? null : data.SDVNT_OID;
                }
                else if (data.TRAN_TYPE_ID == "10")//Sales Group Wise Product Group Setup For area less menue (Many to many)
                {
                    objCmd.CommandText = "DATA_MAPPING.DELETE_SDVNT_SPROD";
                    objCmd.Parameters.Add("P_RESULT", OracleDbType.Varchar2, 20).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("SDVNT_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.SDVNT_OID) ? null : data.SDVNT_OID;
                    objCmd.Parameters.Add("SPROD_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.SKU_OID) ? null : data.SKU_OID;
                }

                objCmd.Connection.Open();
                objCmd.ExecuteNonQuery();
                objCmd.Connection.Close();

                result = objCmd.Parameters["P_RESULT"].Value.ToString();
            }

            return result;
        }

        #region Line Product Save Update
        public string SaveUpdateLineProduct(List<vmDataMapping> dataList)
        {
            string result = string.Empty, res=string.Empty; int newCount = 0, existCount = 0, errCount = 0;
            var util = new Utils();
            //vmDataMapping data = new vmDataMapping();
            foreach (var data in dataList)
            {
                OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);

                OracleCommand objCmd = new OracleCommand();
                objCmd.CommandType = CommandType.StoredProcedure;
                using (objCmd.Connection = con)
                {
                    objCmd.CommandText = "DATA_MAPPING.SET_SDVNT_SPROD";
                    objCmd.Parameters.Add("P_RESULT", OracleDbType.Varchar2, 20).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("SDVNT_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.SDVNT_OID) ? null : data.SDVNT_OID;
                    objCmd.Parameters.Add("SDVNT_SCTYP", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.CMN_OID) ? null : data.CMN_OID;
                    objCmd.Parameters.Add("SPROD_OID", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.SKU_OID) ? null : data.SKU_OID;
                    objCmd.Parameters.Add("SDVNT_SCOMP", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(data.COMPANY_OID) ? null : data.COMPANY_OID;
                    objCmd.Parameters.Add("IS_ACTIVE", OracleDbType.Varchar2).Value = util.BoolVal(data.IS_ENABLE);

                    objCmd.Connection.Open();
                    objCmd.ExecuteNonQuery();
                    objCmd.Connection.Close();

                    result = objCmd.Parameters["P_RESULT"].Value.ToString();
                    if (result == "1")
                        newCount++;
                    if (result == "-1")
                        existCount++;
                    if (result == "0")
                        errCount++;
                }
            }

            res = "Save : "+newCount.ToString() + ", Exist : " + existCount.ToString() + ", Error : " + errCount.ToString();
            return res;
        }
        #endregion Line Product Save Update
    }
}