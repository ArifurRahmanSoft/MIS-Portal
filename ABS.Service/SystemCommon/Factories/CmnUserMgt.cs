using CTGroup.Data.BaseFactories;
using CTGroup.Data.BaseInterfaces;
using CTGroup.Models;
using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.Service.SystemCommon.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTGroup.Service.AllServiceClasses;
using CTGroup.Utility;
using Oracle.ManagedDataAccess.Client;
using CTGroup.Utility.Common;
using System.Data;
using System.Configuration;
using CTGroup.OracleModel;
using ABS.Utility;
using System.Diagnostics;
using CTGroup.OracleModel.ViewModel.SystemCommon;
using Newtonsoft.Json;
using System.Net.Http;

namespace CTGroup.Service.SystemCommon.Factories
{
    public class CmnUserMgt : iCmnUserMgt
    {

        private iGenericFactory_EF<T_CMNUSERAUTHENTICATION> GenericFactory_EF_AuthenticatedUser = null;

        public List<vmAuthenticatedUser> Get_CmnUserAuthenticationBackup(vmLoginUser model)
        {
            var util = new Utils();
            List<vmAuthenticatedUser> objAuthUser = null;
            string encryptedPassword = EncryptAndDecrypt.Encrypt(model.UserPassw, "sblw-3hn8-sqoy20");
            try
            {
                if (model != null)
                {
                    OracleCommand objCmd = new OracleCommand();

                    //if (model.EmpID == "00001" || model.EmpID == "00003")
                    //{
                    //    objCmd.CommandText = "monirtesintg.Get_CmnUserAuthentication_Internal";
                    //}
                    //else if (model.EmpID == "00999" || model.EmpID == "00888")
                    //{
                    //    objCmd.CommandText = "monirtesintg.Get_SpecialAuthentication";
                    //}
                    //else
                    //{
                    //    objCmd.CommandText = "monirtesintg.Get_CmnUserAuthentication";
                    //}
                    objCmd.CommandText = "monirtesintg.Get_CmnUserAuthentication";
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.Parameters.Add("P_UserLogin", OracleDbType.Varchar2).Value = model.EmpID;
                    objCmd.Parameters.Add("P_Password", OracleDbType.Varchar2).Value = encryptedPassword;
                    objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
                    DataTable dt = classDt.GetData(objCmd);

                    objAuthUser = dt.AsEnumerable().Select(dataRow => new vmAuthenticatedUser
                    {
                        UserID = model.EmpID,
                        CompanyID = dataRow.Field<decimal>("COMPANYID"),
                        IsFirstLogin = dataRow.Field<decimal>("ISFIRSTLOGIN"),

                        ReturnValue = dataRow.Field<decimal>("RETURNVALUE"),
                        CompanyName = dataRow.Field<string>("COMPANYNAME"),
                        UserFullName = dataRow.Field<string>("USERFULLNAME"),
                        CompanyShortName = dataRow.Field<string>("COMPANYSHORTNAME"),
                        IsTemp = util.BoolVal(dataRow.Field<string>("ISTEMP"))
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                var frame = new StackTrace(true).GetFrame(0);
                var filename = frame.GetFileName();
                var line = frame.GetFileLineNumber();

                Utils u = new Utils();
                u.LogWrite(e, filename, line);

                e.ToString();


            }
            return objAuthUser;
        }

        public List<vmAuthenticatedUser> Get_CmnUserAuthentication(vmLoginUser model)
        {
            var util = new Utils();
            List<vmAuthenticatedUser> objAuthUser = null;
            string encryptedPassword = EncryptAndDecrypt.Encrypt(model.UserPassw, "sblw-3hn8-sqoy20");
            try
            {
                if (model != null)
                {
                    OracleCommand objCmd = new OracleCommand();

                    objCmd.CommandText = "monirtesintg.Get_CmnUserAuthentication";
                    objCmd.CommandType = CommandType.StoredProcedure;

                    objCmd.Parameters.Add("P_UserLogin", OracleDbType.Varchar2).Value = model.EmpID;
                    objCmd.Parameters.Add("P_Password", OracleDbType.Varchar2).Value = encryptedPassword;
                    objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
                    DataTable dt = classDt.GetData(objCmd);

                    objAuthUser = dt.AsEnumerable().Select(dataRow => new vmAuthenticatedUser
                    {
                        UserID = model.EmpID,
                        CompanyID = dataRow.Field<decimal>("COMPANYID"),
                        IsFirstLogin = dataRow.Field<decimal>("ISFIRSTLOGIN"),

                        ReturnValue = dataRow.Field<decimal>("RETURNVALUE"),
                        CompanyName = dataRow.Field<string>("COMPANYNAME"),
                        UserFullName = dataRow.Field<string>("USERFULLNAME"),
                        CompanyShortName = dataRow.Field<string>("COMPANYSHORTNAME"),
                        IsTemp = util.BoolVal(dataRow.Field<string>("ISTEMP"))
                    }).ToList();

                    if (objAuthUser != null && objAuthUser.Count > 0)
                    {
                        if (!objAuthUser[0].IsTemp)
                        {
                            object loggeddata = new { EmpID = model.EmpID, UserPassw = model.UserPassw };
                            string loginUrl = "https://cssap.citygroupbd.com/acmsapi/api/usersetup/verifyuser";
                            var serializedPackage = JsonConvert.SerializeObject(loggeddata);
                            var content = new StringContent(serializedPackage, Encoding.UTF8, "application/json");
                            HttpClient httpClient = new HttpClient();
                            using (HttpResponseMessage resapidata = httpClient.PostAsync(loginUrl, content).Result)
                            {
                                var resdatas = resapidata.Content.ReadAsStringAsync().Result;
                                if (resapidata.IsSuccessStatusCode)
                                {
                                    string resdata = JsonConvert.DeserializeObject(resdatas.ToString()).ToString();
                                    string[] spdata = resdata.ToString().Split('~');

                                    if (spdata[0] != "1")
                                    {
                                        objAuthUser = new List<vmAuthenticatedUser>();
                                    }
                                    else
                                    {
                                        string rs = UpdateMISCentralUserAuthentication(model.EmpID, objAuthUser[0].UserFullName, encryptedPassword, model.UserPassw);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var frame = new StackTrace(true).GetFrame(0);
                var filename = frame.GetFileName();
                var line = frame.GetFileLineNumber();

                Utils u = new Utils();
                u.LogWrite(e, filename, line);

                e.ToString();


            }
            return objAuthUser;
        }

        private string UpdateMISCentralUserAuthentication(string UserId, string UserName, string HashPass, string Upass)
        {
            string res = "0";
            OracleCommand cmd = new OracleCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "monirtesintg.Get_UpdateAuthentication";
            cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("P_UserLoginID", OracleDbType.Varchar2).Value = UserId;
            cmd.Parameters.Add("P_UserFullName", OracleDbType.Varchar2).Value = UserName;
            cmd.Parameters.Add("P_HashPass", OracleDbType.Varchar2).Value = HashPass;
            cmd.Parameters.Add("P_Password", OracleDbType.Varchar2).Value = Upass;
            cmd.Parameters.Add("P_UpdatePC", OracleDbType.Varchar2).Value = HostService.GetLocalIPAddress();

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetDataBasic(cmd);
            if (dt.Rows.Count > 0) {
                res = dt.Rows[0].Field<string>("IS_CHANGED");
            }

            return res;
        }

        public IEnumerable<vmEmployee> GetUser(vmCmnParameters objcmnParam, out long recordsTotal)
        {
            IEnumerable<vmEmployee> objUser = null;
            IEnumerable<vmEmployee> objBuildingMasterWithOutPaging = null;
            recordsTotal = 0;

            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "SETTINGS.Get_ApplicationUser";
            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetData(objCmd);
            objBuildingMasterWithOutPaging = ConvertDataTableToGenericList.BindList<vmEmployee>(dt);
            //objUser = objBuildingMasterWithOutPaging.Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();

            recordsTotal = objBuildingMasterWithOutPaging.Count();
            return objBuildingMasterWithOutPaging;
        }

        #region Create
        /// <summary>
        /// Save Data To Database
        /// <para>Use it when save data through a stored procedure</para>
        /// </summary>


        /// <summary>
        /// Save Data To Database
        /// <para>Use it when save data through a stored procedure</para>
        /// </summary>

        #endregion

        #region Read

        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through a stored procedure</para>
        /// </summary>

        public string getCurrentPassword(int companyID, string loggedUser)
        {
            GenericFactory_EF_AuthenticatedUser = new CmnUserUserAuthentication_EF();

            vmLoginUser loguser = new vmLoginUser();

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
            con.Open();

            string query = "SELECT CONFIRMPASSWORD FROM T_CMNUSERAUTHENTICATION WHERE USERID = " + loggedUser + " ";
            OracleCommand cmd = new OracleCommand(query, con);
            DataTable t1 = new DataTable();
            using (OracleDataAdapter a = new OracleDataAdapter(cmd))
            {
                a.Fill(t1);
            }
            con.Close();

            string currentPassword = t1.Rows[0]["CONFIRMPASSWORD"].ToString();

            return currentPassword;
        }

        public int ChangePassword(vmLoginUser model)
        {
            int result = 0;
            try
            {
                string UpdatePC = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(1).ToString();
                //EncryptAndDecrypt ead = new EncryptAndDecrypt();
                string encryptedPassword = EncryptAndDecrypt.Encrypt(model.Password, "sblw-3hn8-sqoy20");


                OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
                OracleCommand objCmd = new OracleCommand();
                try
                {
                    using (objCmd.Connection = con)
                    {
                        objCmd.CommandType = CommandType.StoredProcedure;
                        objCmd.CommandText = "SETTINGS.UPDATE_USERPASSWORD";
                        objCmd.Parameters.Add("P_USERLOGIN", OracleDbType.Varchar2).Value = model.UserLogin;
                        objCmd.Parameters.Add("P_PASSWORD", OracleDbType.Varchar2).Value = model.Password;
                        objCmd.Parameters.Add("P_ENCRYPTPASSWORD", OracleDbType.Varchar2).Value = encryptedPassword;
                        objCmd.Parameters.Add("P_UPDATEBY", OracleDbType.Varchar2).Value = model.UpdateBy;
                        objCmd.Parameters.Add("P_UPDATEON", OracleDbType.Date).Value = DateTime.Now;
                        objCmd.Parameters.Add("P_UPDATEPC", OracleDbType.Varchar2).Value = UpdatePC;

                        objCmd.Connection.Open();
                        objCmd.ExecuteNonQuery();

                        result = 1;
                    }
                }
                catch (OracleException exception)
                {
                    var frame = new StackTrace(true).GetFrame(0);
                    var filename = frame.GetFileName();
                    var line = frame.GetFileLineNumber();
                }
                finally
                {
                    objCmd.Connection.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }



        #endregion

        //public string getCurrentPassword(int companyID, int loggedUser)
        //{
        //    GenericFactory_EF_AuthenticatedUser = new CmnUserUserAuthentication_EF();

        //    string currentPassword = GenericFactory_EF_AuthenticatedUser.FindBy(x => x.UserID == loggedUser).FirstOrDefault().ConfirmPassword;
        //    return currentPassword;
        //}

        //public int ChangePassword(Models.CmnUserAuthentication model)
        // {
        //     int result = 0;
        //     GenericFactory_EF_AuthenticatedUser = new CmnUserUserAuthentication_EF();
        //     try
        //     {
        //         CmnUserAuthentication _objUserAuthentication = GenericFactory_EF_AuthenticatedUser.FindBy(x => x.UserID == model.UserID).FirstOrDefault();

        //         _objUserAuthentication.Password = model.Password;
        //         _objUserAuthentication.ConfirmPassword = model.Password;
        //         _objUserAuthentication.UpdateBy = model.UpdateBy;
        //         _objUserAuthentication.UpdatePc = HostService.GetIP();
        //         _objUserAuthentication.UpdateOn = DateTime.Now;
        //         GenericFactory_EF_AuthenticatedUser.Update(_objUserAuthentication);
        //         GenericFactory_EF_AuthenticatedUser.Save();
        //         result = 1;
        //     }
        //     catch (Exception)
        //     {                               
        //     }
        //     return result;
        // }


        //public int CheckLoginID(CmnUserAuthentication Autuser)
        //{
        //    int isexist = 0;
        //    using (ERP_Entities _ctx = new ERP_Entities())
        //    {
        //        try
        //        {
        //            CmnUserAuthentication _UserAut = _ctx.CmnUserAuthentications.Where(x => x.LoginID == Autuser.LoginID && x.IsDeleted == false).FirstOrDefault();
        //            if (_UserAut != null)
        //            {
        //                isexist = 1;
        //            }
        //        }
        //        catch
        //        {
        //            isexist = 0;

        //        }
        //        return isexist;

        //    }
        //}
    }
}
