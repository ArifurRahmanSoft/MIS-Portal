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
    public class TentativeSaleMgt : iTentativeSaleMgt
    {
        public string SaveTentativeSale(vmTentativeSale model)
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
                        objCmd.CommandText = "PKG_TENTATIVESALE.UPDATE_TENTATIVE_SALE";
                        objCmd.Parameters.Add("P_OID", OracleDbType.Varchar2).Value = model.OID;
                        objCmd.Parameters.Add("P_AMOUNT", OracleDbType.Decimal).Value = model.AMOUNT;
                        objCmd.Parameters.Add("P_LOCATIONOID", OracleDbType.Varchar2).Value = model.LOCATIONOID;

                        objCmd.Parameters.Add("P_UPDATEBY", OracleDbType.Varchar2).Value = model.ENTRYUSER;
                        objCmd.Parameters.Add("P_UPDATEDATE", OracleDbType.Date).Value = DateTime.Now;

                        objCmd.Connection.Open();
                        objCmd.ExecuteNonQuery();

                        //MasterOID = model.OID.ToString();
                        MasterOID = objCmd.Parameters["P_OID"].Value.ToString();
                    }
                    else
                    {
                        objCmd.CommandText = "PKG_TENTATIVESALE.SET_TENTATIVE_SALE";
                        objCmd.Parameters.Add("P_OID", OracleDbType.Varchar2, 100).Direction = ParameterDirection.Output;
                        objCmd.Parameters.Add("P_AMOUNT", OracleDbType.Decimal).Value = model.AMOUNT;
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
                if (exception.ToString().Contains("unique constraint"))
                {
                    MasterOID = "duplicate";
                }
                else
                {
                    MasterOID = exception.ToString();
                }
            }
            finally
            {
                objCmd.Connection.Close();
            }
            return MasterOID;
        }

        #region Read         
        public IEnumerable<vmTentativeSale> GetTentativeSale(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmTentativeSale> objUser = null;
            IEnumerable<vmTentativeSale> objUserWithOutPaging = null;
            recordsTotal = 0;

            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "PKG_TENTATIVESALE.GET_TENTATIVE_SALE";
            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            objCmd.Parameters.Add("P_LOGGEDUSER", OracleDbType.Varchar2).Value = objcmnParam.loggeduser;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetData(objCmd);
            objUserWithOutPaging = ConvertDataTableToGenericList.BindList<vmTentativeSale>(dt);
            objUser = objUserWithOutPaging.OrderByDescending(x => x.OID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();

            recordsTotal = objUserWithOutPaging.Count();
            return objUser;
        }
        #endregion
        
    }
}
