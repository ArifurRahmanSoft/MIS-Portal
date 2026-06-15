using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel.ViewModel.Sales;
using CTGroup.Service.Sales.Interfaces;
using CTGroup.Utility.Common;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace CTGroup.Service.Sales.Factories
{
    public class AutoRiceCollectionMgt : iAutoRiceCollectionMgt
    {
        public string SaveAutoRiceCollection(vmAutoRiceCollection model)
        {
            string MasterOID = string.Empty;

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
            OracleCommand objCmd = new OracleCommand();
            try
            {
                    using (objCmd.Connection = con)
                    {
                        objCmd.CommandType = CommandType.StoredProcedure;
                        if (model.OID != null)
                        {
                            objCmd.CommandText = "PKG_AUTORICE.UPDATE_AUTO_RICE_COLLECTION";
                            objCmd.Parameters.Add("P_OID", OracleDbType.Varchar2).Value = model.OID;
                            objCmd.Parameters.Add("P_CASHCOLLECTION", OracleDbType.Decimal).Value = model.CASHCOLLECTION;
                            objCmd.Parameters.Add("P_CHEQUECOLLECTION", OracleDbType.Decimal).Value = model.CHEQUECOLLECTION;
                            objCmd.Parameters.Add("P_TTCOLLECTION", OracleDbType.Decimal).Value = model.TTCOLLECTION;
                            objCmd.Parameters.Add("P_LCCOLLECTION", OracleDbType.Decimal).Value = model.LCCOLLECTION;
                            objCmd.Parameters.Add("P_OTHERCOLLECTION", OracleDbType.Decimal).Value = model.OTHERCOLLECTION;
                            objCmd.Parameters.Add("P_SALETYPE", OracleDbType.Varchar2).Value = model.SALETYPE;
                            objCmd.Parameters.Add("P_LOCATIONOID", OracleDbType.Varchar2).Value = model.LOCATIONOID;
                            objCmd.Parameters.Add("P_UPDATEBY", OracleDbType.Varchar2).Value = model.ENTRYUSER;
                            objCmd.Parameters.Add("P_UPDATEDATE", OracleDbType.Date).Value = DateTime.Now;
                                                        
                            objCmd.Connection.Open();
                            objCmd.ExecuteNonQuery();
                            objCmd.Connection.Close();

                            //MasterOID = model.OID.ToString();
                            MasterOID = "2";
                        }
                        else
                        {
                            objCmd.CommandText = "PKG_AUTORICE.SET_AUTO_RICE_COLLECTION";
                            objCmd.Parameters.Add("P_OID", OracleDbType.Varchar2, 1).Direction = ParameterDirection.Output;
                            objCmd.Parameters.Add("P_CASHCOLLECTION", OracleDbType.Decimal).Value = model.CASHCOLLECTION;
                            objCmd.Parameters.Add("P_CHEQUECOLLECTION", OracleDbType.Decimal).Value = model.CHEQUECOLLECTION;
                            objCmd.Parameters.Add("P_TTCOLLECTION", OracleDbType.Decimal).Value = model.TTCOLLECTION;
                            objCmd.Parameters.Add("P_LCCOLLECTION", OracleDbType.Decimal).Value = model.LCCOLLECTION;
                            objCmd.Parameters.Add("P_OTHERCOLLECTION", OracleDbType.Decimal).Value = model.OTHERCOLLECTION;
                            objCmd.Parameters.Add("P_SALETYPE", OracleDbType.Varchar2).Value = model.SALETYPE;
                            objCmd.Parameters.Add("P_LOCATIONOID", OracleDbType.Varchar2).Value = model.LOCATIONOID;
                            //objCmd.Parameters.Add("P_SALEDATE", OracleDbType.Date).Value = Convert.ToDateTime(DateTime.Now.Date.ToString("dd/MM/yyyy")); //model.SALEDATE;


                            objCmd.Parameters.Add("P_ENTRYUSER", OracleDbType.Varchar2).Value = model.ENTRYUSER;
                            objCmd.Parameters.Add("P_ENTRYDATE", OracleDbType.Date).Value = DateTime.Now;
                            //objCmd.Parameters.Add("P_CREATED_IP", OracleDbType.Varchar2).Value = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(1).ToString();
                                                       
                            objCmd.Connection.Open();
                            objCmd.ExecuteNonQuery();
                            objCmd.Connection.Close();

                            MasterOID = objCmd.Parameters["P_OID"].Value.ToString();
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
            finally
            {
                objCmd.Connection.Close();
            }
            return MasterOID;
        }

        public string GetUserType(string loggeduser)
        {
            string usertype = "";
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);

            con.Open();
            string query = "select USERTYPE from T_CMNMENUPER_USER_TYPE where STAFFID = "  + loggeduser +  " and menuid = 53";

            OracleCommand cmd = new OracleCommand(query, con);

            DataTable t1 = new DataTable();
            using (OracleDataAdapter a = new OracleDataAdapter(cmd))
            {
                a.Fill(t1);
            }
            con.Close();

            usertype = t1.Rows[0]["USERTYPE"].ToString();

            return usertype;
        }


        #region Read         
        public IEnumerable<vmAutoRiceCollection> GetAutoRiceCollection(vmCmnParameters objcmnParam, out int recordsTotal)
        {


            string usertype = GetUserType(objcmnParam.loggeduser);


            IEnumerable<vmAutoRiceCollection> objUser = null;
            IEnumerable<vmAutoRiceCollection> objUserWithOutPaging = null;
            recordsTotal = 0;

            OracleCommand objCmd = new OracleCommand();
            if (usertype == "bulk")
                objCmd.CommandText = "PKG_AUTORICE.GET_AUTO_RICE_COLLECTION";
            if (usertype == "marketing")
                objCmd.CommandText = "PKG_AUTORICE.GET_AUTO_RICE_COLLECTION_MKT";

            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("P_LOGGEDUSER", OracleDbType.Varchar2).Value = objcmnParam.loggeduser;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetData(objCmd);
            objUserWithOutPaging = ConvertDataTableToGenericList.BindList<vmAutoRiceCollection>(dt);
            objUser = objUserWithOutPaging.OrderByDescending(x => x.OID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();

            recordsTotal = objUserWithOutPaging.Count();
            return objUser;
        }
        #endregion
    }
}
