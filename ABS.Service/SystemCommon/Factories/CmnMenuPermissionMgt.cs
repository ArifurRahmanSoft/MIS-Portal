using CTGroup.Data.BaseInterfaces;
using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel;
using CTGroup.Service.AllServiceClasses;
using CTGroup.Service.SystemCommon.Interfaces;
using CTGroup.Utility.Common;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ABS.Service.SystemCommon.Factories
{
    public class CmnMenuPermissionMgt : iCmnMenuPermissionMgt
    {        
        public IEnumerable<vmCmnMenuPermission> GetMenuPermissionByParams(string loggedUser, int? pageNumber, int? pageSize,
            int? IsPaging, int? pModuleID, string pUserID)

        {
            IEnumerable<vmCmnMenuPermission> objMenues = null;
            string spQuery = string.Empty;
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("loggedUser", loggedUser);
                ht.Add("pageNumber", pageNumber);
                ht.Add("pageSize", pageSize);
                ht.Add("IsPaging", IsPaging);
                ht.Add("ModuleID", pModuleID);
                ht.Add("UserID", pUserID);

                spQuery = "[Get_CmnMenuPermissionByParam]";
                objMenues = new vmCmnMenuPermission_GF().ExecuteQuery(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objMenues;
        }

        public IEnumerable<vmCmnMenuPermission> GetMenuPermissionByParamsUser(string loggedUser, int? pageNumber, int? pageSize,
            int? IsPaging, int? pModuleID, string pUserID)
        {
            // IEnumerable<vmCmnMenuPermission> objMenues = null;
            IEnumerable<vmCmnMenuPermission> objMenuesWithOutPaging = null;

            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "SETTINGS.Get_MenuPermissionByUser";
            objCmd.Parameters.Add("P_LOGGEDUSER", OracleDbType.Varchar2).Value = loggedUser;
            objCmd.Parameters.Add("P_MODULEID", OracleDbType.Decimal).Value = pModuleID;
            objCmd.Parameters.Add("P_USERID", OracleDbType.Varchar2).Value = pUserID;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            objCmd.CommandType = CommandType.StoredProcedure;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetData(objCmd);
            objMenuesWithOutPaging = ConvertDataTableToGenericList.BindList<vmCmnMenuPermission>(dt);

            return objMenuesWithOutPaging;
        }
        public string SaveMenuPermission(List<vmCmnMenuPermission> listModel)
        {
            List<vmCmnMenuPermission> listInsertModel = new List<vmCmnMenuPermission>();
            string RESULT = "";

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
            OracleCommand objCmd = new OracleCommand();

            try
            {
                string PermissionList = "";
                PermissionList = GetPermissionList(listModel);
                try
                {
                    using (objCmd.Connection = con)
                    {
                        objCmd.CommandType = CommandType.StoredProcedure;

                        objCmd.CommandText = "SETTINGS.SET_MENUPERMISSION";
                        objCmd.Parameters.Add("P_RESULT", OracleDbType.Varchar2, 35).Direction = ParameterDirection.Output;
                        objCmd.Parameters.Add("P_MODULEID", OracleDbType.Decimal).Value = listModel[0].MODULEID;
                        objCmd.Parameters.Add("P_USERID", OracleDbType.Varchar2).Value = listModel[0].USERID;
                        objCmd.Parameters.Add("P_MENUPERMISSIONLIST", OracleDbType.Varchar2, 10000).Value = PermissionList;

                        objCmd.Connection.Open();
                        objCmd.ExecuteNonQuery();

                        RESULT = objCmd.Parameters["P_RESULT"].Value.ToString();
                    }
                }
                catch (OracleException exception)
                {
                    var frame = new StackTrace(true).GetFrame(0);
                    var filename = frame.GetFileName();
                    var line = frame.GetFileLineNumber();

                    //Utils u = new Utils();
                    //u.LowWrite(exception, filename, line);
                }
                RESULT = "1";
            }
            catch (Exception ex)
            {
                RESULT = "0";
            }
            finally
            {
                objCmd.Connection.Close();
                RESULT = "1";
            }
            return RESULT;
        }

        public string GetPermissionList(List<vmCmnMenuPermission> docList)
        {
            string multivalue = "";
            
            foreach (vmCmnMenuPermission doc in docList)
            {
                vmCmnMenuPermission objDocNew = new vmCmnMenuPermission();
                objDocNew.MENUPERMISSIONID = doc.MENUPERMISSIONID;
                objDocNew.MODULEID = doc.MODULEID;
                objDocNew.MODULENAME = doc.MODULENAME;
                objDocNew.MENUID = doc.MENUID;
                objDocNew.MENUNAME = doc.MENUNAME;
                objDocNew.ENABLEVIEW = doc.ENABLE_VIEW == true? 1 : 0;
                objDocNew.ENABLEINSERT = doc.ENABLE_INSERT == true ? 1 : 0;
                objDocNew.ENABLEUPDATE = doc.ENABLE_UPDATE == true ? 1 : 0;
                objDocNew.ENABLEDELETE = doc.ENABLE_DELETE == true ? 1 : 0;
                objDocNew.USERID = doc.USERID;
                objDocNew.CompanyID = doc.CompanyID;
                objDocNew.CreateBy = 1;
                objDocNew.CreatePc = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(1).ToString();

                string singlevlaue = "";              

                singlevlaue = "x" + ':' + objDocNew.MODULEID + ':' + objDocNew.MENUID + ':' + objDocNew.USERID + ':' + objDocNew.ENABLEVIEW + 
                               ':' + objDocNew.ENABLEINSERT + ':' + objDocNew.ENABLEUPDATE + ':' + objDocNew.ENABLEDELETE + 
                               ':' + objDocNew.CompanyID + ':' + objDocNew.CreateBy + ':' + DateTime.Now.Date.ToString("dd/MM/yyyy") + 
                               ':' + objDocNew.CreatePc + ';';

                multivalue += singlevlaue;
            }
            return multivalue;
        }
    }
}
