using CTGroup.Data.BaseInterfaces;
using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel;
using CTGroup.Service.AllServiceClasses;
using CTGroup.Service.SystemCommon.Interfaces;
using CTGroup.Utility;
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
    public class CmnBrandPermissionMgt : iCmnBrandPermissionMgt
    {
        public IEnumerable<vmCmnBrandPermission> GetBrandPermissionByParams(string loggedUser, int? pageNumber, int? pageSize,
            int? IsPaging, int? pModuleID, string pUserID)

        {
            IEnumerable<vmCmnBrandPermission> objMenues = null;
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

                spQuery = "[Get_CmnBrandPermissionByParam]";
                objMenues = new vmCmnBrandPermission_GF().ExecuteQuery(spQuery, ht);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objMenues;
        }

        public IEnumerable<vmCmnBrandPermission> GetBrandPermissionByParamsUser(string loggedUser, int? pageNumber, int? pageSize,
            int? IsPaging, int? pModuleID, string pUserID)
        {
            // IEnumerable<vmCmnBrandPermission> objMenues = null;
            IEnumerable<vmCmnBrandPermission> objMenuesWithOutPaging = null;

            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "SETTINGS.Get_BrandPermissionByUser";
            objCmd.Parameters.Add("P_LOGGEDUSER", OracleDbType.Varchar2).Value = loggedUser;
            objCmd.Parameters.Add("P_MODULEID", OracleDbType.Decimal).Value = pModuleID;
            objCmd.Parameters.Add("P_USERID", OracleDbType.Varchar2).Value = pUserID;
            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            objCmd.CommandType = CommandType.StoredProcedure;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetData(objCmd);
            objMenuesWithOutPaging = ConvertDataTableToGenericList.BindList<vmCmnBrandPermission>(dt);

            return objMenuesWithOutPaging;
        }
        public string SaveBrandPermission(List<vmCmnBrandPermission> listModel)
        {
            List<vmCmnBrandPermission> listInsertModel = new List<vmCmnBrandPermission>();
            string RESULT = "";
            try
            {
                OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);

                string BrandPermissionList = "";
                BrandPermissionList = GetBrandPermissionList(listModel);

                string SKUPermissionList = "";
                SKUPermissionList = GetSKUPermissionList(listModel[0].ListSKUmodel);

                try
                {
                    using (OracleCommand objCmd = new OracleCommand())
                    {
                        using (objCmd.Connection = con)
                        {
                            objCmd.CommandType = CommandType.StoredProcedure;

                            objCmd.CommandText = "TestSKUMapping.SET_TEAMMAPPINGBRAND";
                            objCmd.Parameters.Add("P_RESULT", OracleDbType.Varchar2, 35).Direction = ParameterDirection.Output;
                            objCmd.Parameters.Add("P_NATIONALTEAMOID", OracleDbType.Varchar2).Value = listModel[0].NATIONALTEAMOID;
                            objCmd.Parameters.Add("P_BrandPermissionLIST", OracleDbType.Varchar2, 10000).Value = BrandPermissionList;
                            //objCmd.Parameters.Add("P_SKUPermissionLIST", OracleDbType.Varchar2, 10000).Value = listModel[0].ListSKUNew;
                            //objCmd.Parameters.Add("P_SKUPermissionLIST", OracleDbType.Varchar2, 10000).Value = listModel[0].ListSKUmodel;
                            objCmd.Parameters.Add("P_SKUPermissionLIST", OracleDbType.Varchar2, 10000).Value = SKUPermissionList;
                            //objCmd.Parameters.Add("P_SKUPermissionLIST", OracleDbType.Varchar2, 10000).Value = ListSKUmodel;

                            objCmd.Connection.Open();
                            objCmd.ExecuteNonQuery();
                            objCmd.Connection.Close();

                            RESULT = objCmd.Parameters["P_RESULT"].Value.ToString();
                        }
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
                RESULT = "1";
            }
            return RESULT;
        }
        public string GetBrandPermissionList(List<vmCmnBrandPermission> docList)
        {
            string multivalue = "";

            foreach (vmCmnBrandPermission doc in docList)
            {
                vmCmnBrandPermission objDocNew = new vmCmnBrandPermission();
                objDocNew.BRANDOID = doc.BRANDOID;
                objDocNew.NATIONALTEAMOID = doc.NATIONALTEAMOID;
                objDocNew.LOGGEDUSERID = doc.LOGGEDUSERID;
                objDocNew.CreatePc = HostService.GetLocalIPAddress(); //System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(1).ToString();

                string singlevlaue = "";

                singlevlaue = "x" + ':' + objDocNew.BRANDOID + ':' + objDocNew.LOGGEDUSERID +
                                    ':' + objDocNew.CreatePc + ';';

                multivalue += singlevlaue;
            }
            return multivalue;
        }

        public string GetSKUPermissionList(List<vmCmnBrandSKUPermission> docList)
        {
            string multivalue = "";

            foreach (vmCmnBrandSKUPermission doc in docList)
            {
                vmCmnBrandSKUPermission objDocNew = new vmCmnBrandSKUPermission();
                objDocNew.NATIONALOID = doc.NATIONALOID;
                objDocNew.BRANDOID = doc.BRANDOID;
                objDocNew.PRODUCTOID = doc.PRODUCTOID;
                objDocNew.ENABLE_CHECK = doc.ENABLE_CHECK;
                objDocNew.ISUPDATE = doc.ISUPDATE;
                objDocNew.LOGGEDUSERID = doc.LOGGEDUSERID;
                objDocNew.CREATEPC = HostService.GetLocalIPAddress();//System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(1).ToString();

                string singlevlaue = "";

                singlevlaue = "x" + ':' + objDocNew.NATIONALOID + ':' + objDocNew.BRANDOID + ':' + objDocNew.PRODUCTOID + ':' + (objDocNew.ISUPDATE == true ? "1" : "0") + ':' + (objDocNew.ENABLE_CHECK == true ? "1" : "0") +
                              ':' + objDocNew.LOGGEDUSERID +
                               ':' + objDocNew.CREATEPC + ';';

                multivalue += singlevlaue;
            }
            return multivalue;
        }
    }
}
