using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel.ViewModel.SystemCommon;
using CTGroup.Service.SystemCommon.Interfaces;
using CTGroup.Utility;
using CTGroup.Utility.Common;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace CTGroup.Service.SystemCommon.Factories
{
    public class RoleMenuMgt : iRoleMenuMgt
    {
        public IEnumerable<vmRoleMenu> GetByPage(vmCmnParameters cparam)
        {
            IEnumerable<vmRoleMenu> objRoleMenu = null;

            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "USERROLEMENUPERMISSION.Get_RoleMenuByRole";
            objCmd.Parameters.Add("P_ROLEID", OracleDbType.Decimal).Value = cparam.id;
            objCmd.Parameters.Add("P_LOGGEDUSERID", OracleDbType.Varchar2).Value = cparam.loggeduser;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetData(objCmd);
            objRoleMenu = ConvertDataTableToGenericList.BindList<vmRoleMenu>(dt);            

            return objRoleMenu;
        }

        public string SaveUpdate(string JsonData)
        {
            string result = string.Empty;

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
            OracleCommand objCmd = new OracleCommand();
            using (objCmd.Connection = con)
            {
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "USERROLEMENUPERMISSION.SET_ROLEMENUASSIGN";
                objCmd.Parameters.Add("P_RESULT", OracleDbType.Varchar2, 1).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("JsonData", OracleDbType.Clob).Value = JsonData;
                objCmd.Parameters.Add("TRANSACTIONPC", OracleDbType.Varchar2).Value = HostService.GetLocalIPAddress();

                objCmd.Connection.Open();
                objCmd.ExecuteNonQuery();
                objCmd.Connection.Close();

                result = objCmd.Parameters["P_RESULT"].Value.ToString();
            }

            return result;
        }

        public string SaveRole(string roleName, string loggedUserId)
        {
            string result = string.Empty;

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
            OracleCommand objCmd = new OracleCommand();
            using (objCmd.Connection = con)
            {
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "USERROLEMENUPERMISSION.SET_ROLE";
                objCmd.Parameters.Add("P_RESULT", OracleDbType.Varchar2, 1).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("P_ROLENAME", OracleDbType.Varchar2).Value = roleName;
                objCmd.Parameters.Add("LOGGED_USER_ID", OracleDbType.Varchar2).Value = loggedUserId;
                objCmd.Parameters.Add("TRANSACTIONPC", OracleDbType.Varchar2).Value = HostService.GetLocalIPAddress();

                objCmd.Connection.Open();
                objCmd.ExecuteNonQuery();
                objCmd.Connection.Close();

                result = objCmd.Parameters["P_RESULT"].Value.ToString();
            }

            return result;
        }

        public string Delete(vmCmnParameters cparam)
        {
            string result = string.Empty;

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
            OracleCommand objCmd = new OracleCommand();
            using (objCmd.Connection = con)
            {
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "USERROLEMENUPERMISSION.Del_RoleMenu";
                objCmd.Parameters.Add("P_RESULT", OracleDbType.Varchar2, 1).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("P_USERROLEID", OracleDbType.Decimal).Value = cparam.id;

                objCmd.Connection.Open();
                objCmd.ExecuteNonQuery();
                objCmd.Connection.Close();

                result = objCmd.Parameters["P_RESULT"].Value.ToString();
            }

            return result;
        }
    }
}
