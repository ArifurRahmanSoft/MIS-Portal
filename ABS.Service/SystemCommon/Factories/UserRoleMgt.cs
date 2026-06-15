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
    public class UserRoleMgt: iUserRoleMgt
    {
        public IEnumerable<vmUserRole> GetByPage(vmCmnParameters cparam)
        {
            IEnumerable<vmUserRole> objUserRole = null;

            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "USERROLEMENUPERMISSION.Get_UserRole";
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("PageNumber", OracleDbType.Decimal).Value = cparam.pageNumber;
            objCmd.Parameters.Add("PageSize", OracleDbType.Decimal).Value = cparam.pageSize;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetData(objCmd);
            objUserRole = ConvertDataTableToGenericList.BindList<vmUserRole>(dt);

            return objUserRole;
        }

        public string SaveUpdate(vmUserRole urole)
        {
            string result = string.Empty;
            string IpAddress = HostService.GetLocalIPAddress();
            
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
            OracleCommand objCmd = new OracleCommand();
            using (objCmd.Connection = con)
            {
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "USERROLEMENUPERMISSION.Set_UserRole";
                objCmd.Parameters.Add("P_RESULT", OracleDbType.Varchar2, 1).Direction = ParameterDirection.Output;
                objCmd.Parameters.Add("P_USERROLEID", OracleDbType.Decimal).Value = urole.USERROLEID;
                objCmd.Parameters.Add("P_ROLEID", OracleDbType.Decimal).Value = urole.ROLEID;
                objCmd.Parameters.Add("P_USERID", OracleDbType.Varchar2).Value = urole.USERID;
                objCmd.Parameters.Add("P_CREATEBY", OracleDbType.Varchar2).Value = urole.CREATEBY;
                objCmd.Parameters.Add("P_CREATEPC", OracleDbType.Varchar2).Value = IpAddress;

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
                objCmd.CommandText = "USERROLEMENUPERMISSION.Del_UserRole";
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