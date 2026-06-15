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
    public class AutoRiceSalesByProductMgt : iAutoRiceSalesByProductMgt
    {
        public string SaveAutoRiceSaleByProduct(vmAutoRiceSaleByProduct model)
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
                        objCmd.CommandText = "PKG_AUTORICE.UPDATE_AUTO_RICE_SALE_BY_PRODUCT";
                        objCmd.Parameters.Add("P_OID", OracleDbType.Varchar2).Value = model.OID;
                        objCmd.Parameters.Add("P_CINIGURASALE_BULK", OracleDbType.Decimal).Value = model.CINIGURASALE_BULK;
                        objCmd.Parameters.Add("P_CINIGURASALE_CONSUMER", OracleDbType.Decimal).Value = model.CINIGURASALE_CONSUMER;
                        objCmd.Parameters.Add("P_MASURDALSALE_BULK", OracleDbType.Decimal).Value = model.MASURDALSALE_BULK;
                        objCmd.Parameters.Add("P_MASURDALSALE_CONSUMER", OracleDbType.Decimal).Value = model.MASURDALSALE_CONSUMER;
                        objCmd.Parameters.Add("P_RICESALE_BULK", OracleDbType.Decimal).Value = model.RICESALE_BULK;
                        objCmd.Parameters.Add("P_RICESALE_CONSUMER", OracleDbType.Decimal).Value = model.RICESALE_CONSUMER;

                        objCmd.Parameters.Add("P_UPDATEBY", OracleDbType.Varchar2).Value = model.ENTRYUSER;
                        objCmd.Parameters.Add("P_UPDATEDATE", OracleDbType.Date).Value = DateTime.Now;

                        objCmd.Connection.Open();
                        objCmd.ExecuteNonQuery();
                        objCmd.Connection.Close();
                        MasterOID = "2";
                    }
                    else
                    {
                        objCmd.CommandText = "PKG_AUTORICE.SET_AUTO_RICE_SALE_BY_PRODUCT";
                        objCmd.Parameters.Add("P_OID", OracleDbType.Varchar2, 1).Direction = ParameterDirection.Output;
                        objCmd.Parameters.Add("P_CINIGURASALE_BULK", OracleDbType.Decimal).Value = model.CINIGURASALE_BULK;
                        objCmd.Parameters.Add("P_CINIGURASALE_CONSUMER", OracleDbType.Decimal).Value = model.CINIGURASALE_CONSUMER;
                        objCmd.Parameters.Add("P_MASURDALSALE_BULK", OracleDbType.Decimal).Value = model.MASURDALSALE_BULK;
                        objCmd.Parameters.Add("P_MASURDALSALE_CONSUMER", OracleDbType.Decimal).Value = model.MASURDALSALE_CONSUMER;
                        objCmd.Parameters.Add("P_RICESALE_BULK", OracleDbType.Decimal).Value = model.RICESALE_BULK;
                        objCmd.Parameters.Add("P_RICESALE_CONSUMER", OracleDbType.Decimal).Value = model.RICESALE_CONSUMER;
                        objCmd.Parameters.Add("P_ENTRYUSER", OracleDbType.Varchar2).Value = model.ENTRYUSER;
                        objCmd.Parameters.Add("P_ENTRYDATE", OracleDbType.Date).Value = DateTime.Now;

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
            }
            finally
            {
                objCmd.Connection.Close();
            }
            return MasterOID;
        }

        #region Read         
        public IEnumerable<vmAutoRiceSaleByProduct> GetAutoRiceSalesByProduct(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmAutoRiceSaleByProduct> objUser = null;
            IEnumerable<vmAutoRiceSaleByProduct> objUserWithOutPaging = null;
            recordsTotal = 0;

            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_AUTORICE.GET_AUTO_RICE_SALE_BY_PRODUCT";

            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("P_LOGGEDUSER", OracleDbType.Varchar2).Value = objcmnParam.loggeduser;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetData(objCmd);
            objUserWithOutPaging = ConvertDataTableToGenericList.BindList<vmAutoRiceSaleByProduct>(dt);
            objUser = objUserWithOutPaging.OrderByDescending(x => x.OID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();

            recordsTotal = objUserWithOutPaging.Count();
            return objUser;
        }
        #endregion
    }
}
