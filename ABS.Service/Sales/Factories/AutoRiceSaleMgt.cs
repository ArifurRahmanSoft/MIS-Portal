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
    public class AutoRiceSaleMgt : iAutoRiceSaleMgt
    {
        public string SaveRentCollector(vmAutoRiceSale model)
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
                        objCmd.CommandText = "PKG_AUTORICE.UPDATE_AUTO_RICE_SALE";
                        objCmd.Parameters.Add("P_OID", OracleDbType.Varchar2).Value = model.OID;
                        objCmd.Parameters.Add("P_MARKETINGSALE", OracleDbType.Decimal).Value = model.MARKETINGSALE;
                        objCmd.Parameters.Add("P_BULKDEPTSALE", OracleDbType.Decimal).Value = model.BULKDEPTSALE;
                        objCmd.Parameters.Add("P_SALETYPE", OracleDbType.Varchar2).Value = model.SALETYPE;
                        objCmd.Parameters.Add("P_SALEDATE", OracleDbType.Date).Value = model.SALEDATE;

                        objCmd.Parameters.Add("P_UPDATEBY", OracleDbType.Varchar2).Value = model.ENTRYUSER;
                        objCmd.Parameters.Add("P_UPDATEDATE", OracleDbType.Date).Value = DateTime.Now;

                        objCmd.Connection.Open();
                        objCmd.ExecuteNonQuery();

                        MasterOID = "2";
                    }
                    else
                    {
                        objCmd.CommandText = "PKG_AUTORICE.SET_AUTO_RICE_SALE";
                        objCmd.Parameters.Add("P_OID", OracleDbType.Varchar2, 1).Direction = ParameterDirection.Output;
                        objCmd.Parameters.Add("P_MARKETINGSALE", OracleDbType.Decimal).Value = model.MARKETINGSALE;
                        objCmd.Parameters.Add("P_BULKDEPTSALE", OracleDbType.Decimal).Value = model.BULKDEPTSALE;
                        objCmd.Parameters.Add("P_SALETYPE", OracleDbType.Varchar2).Value = model.SALETYPE;
                        objCmd.Parameters.Add("P_SALEDATE", OracleDbType.Date).Value = model.SALEDATE;
                        objCmd.Parameters.Add("P_LOCATIONOID", OracleDbType.Varchar2).Value = model.LOCATIONOID;


                        objCmd.Parameters.Add("P_ENTRYUSER", OracleDbType.Varchar2).Value = model.ENTRYUSER;
                        objCmd.Parameters.Add("P_ENTRYDATE", OracleDbType.Date).Value = DateTime.Now;

                        objCmd.Connection.Open();
                        objCmd.ExecuteNonQuery();

                        MasterOID = objCmd.Parameters["P_OID"].Value.ToString();
                    }
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
            return MasterOID;
        }

        #region Read         
        public IEnumerable<vmAutoRiceSale> GetRentCollector(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmAutoRiceSale> objUser = null;
            IEnumerable<vmAutoRiceSale> objUserWithOutPaging = null;
            recordsTotal = 0;

            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_AUTORICE.GET_AUTO_RICE_SALE";
            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("P_LOGGEDUSER", OracleDbType.Varchar2).Value = objcmnParam.loggeduser;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetData(objCmd);
            objUserWithOutPaging = ConvertDataTableToGenericList.BindList<vmAutoRiceSale>(dt);
            objUser = objUserWithOutPaging.OrderByDescending(x => x.OID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();

            recordsTotal = objUserWithOutPaging.Count();
            return objUser;
        }
        #endregion

        #region Delete       
        //public int DeleteUser(int? id, int? CompanyID, int? LoggedUser)
        //{
        //    int result = 0;
        //    try
        //    {
        //        if ((id > 0) && (CompanyID > 0))
        //        {
        //            using (GenericFactoryFor_User = new vmUser_GF())
        //            {
        //                Hashtable ht = new Hashtable();
        //                ht.Add("CompanyID", CompanyID);
        //                ht.Add("LoggedUser", LoggedUser);

        //                ht.Add("UserID", id);

        //                string spQuery = "[Delete_CmnUser]";
        //                result = GenericFactoryFor_User.ExecuteCommand(spQuery, ht);
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return result;
        //}

        #endregion
    }
}
