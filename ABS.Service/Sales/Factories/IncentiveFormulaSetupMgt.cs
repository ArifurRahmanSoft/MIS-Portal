using CTGroup.Data.BaseInterfaces;
using CTGroup.Models;
using CTGroup.Models.ViewModel.Sales;
using CTGroup.Service.Sales.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using CTGroup.Service.AllServiceClasses;
using CTGroup.Models.ViewModel.SystemCommon;
//using CTGroup.Utility;
using Oracle.ManagedDataAccess;
using Oracle.ManagedDataAccess.EntityFramework;
using Oracle.ManagedDataAccess.Client;
using System.Data.Odbc;
using System.Data;
using System.Configuration;
using CTGroup.OracleModel;
using ABS.Service;
using CTGroup.Utility.Common;
using CTGroup.OracleModel.ViewModel.SystemCommon;
using CTGroup.OracleModel.ViewModel.Sales;

namespace CTGroup.Service.Sales.Factories
{
    public class IncentiveFormulaSetupMgt : iIncentiveFormulaSetupMgt
    {       
       // private iGenericFactory<T_DIST_TAR_MASTER> IncentiveFormulaSetupMaster_GF = null;
        //private iGenericFactory<vmIncentiveFormulaSetupDetail> GenericFactory_vmIncentiveFormulaSetupDetail_GF = null;

        public List<vmDistributor> GetDivision(int? pageNumber, int? pageSize, int? IsPaging)
        {
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);

            con.Open();
            string query = "SELECT OID, SCUST_INFO_TEXT, SCUST_INFO_NAME FROM CITYN.T_SCUST_INFO";

            OracleCommand cmd = new OracleCommand(query, con);

            DataTable t1 = new DataTable();
            using (OracleDataAdapter a = new OracleDataAdapter(cmd))
            {
                a.Fill(t1);
            }
            con.Close();

            List<vmDistributor> objDistributor = null;
            objDistributor = t1.AsEnumerable().Select(dataRow => new vmDistributor
            {
                OID = dataRow.Field<string>("OID"),
                SCUST_INFO_TEXT = dataRow.Field<string>("SCUST_INFO_TEXT"),
                SCUST_INFO_NAME = dataRow.Field<string>("SCUST_INFO_NAME"),
            }).ToList();

            return objDistributor;
        }

        public List<vmDistributor> GetDistributor(int? pageNumber, int? pageSize, int? IsPaging)
        {
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);

            con.Open();
            string query = "SELECT OID, SCUST_INFO_TEXT, SCUST_INFO_NAME FROM CITYN.T_SCUST_INFO";

            OracleCommand cmd = new OracleCommand(query, con);

            DataTable t1 = new DataTable();
            using (OracleDataAdapter a = new OracleDataAdapter(cmd))
            {
                a.Fill(t1);
            }
            con.Close();

            List<vmDistributor> objDistributor = null;
            objDistributor = t1.AsEnumerable().Select(dataRow => new vmDistributor
            {
                OID = dataRow.Field<string>("OID"),
                SCUST_INFO_TEXT = dataRow.Field<string>("SCUST_INFO_TEXT"),
                SCUST_INFO_NAME = dataRow.Field<string>("SCUST_INFO_NAME"),
            }).ToList();

            return objDistributor;
        }

        
        //public IEnumerable<vmIncentiveFormulaSetupDetail> GetIncentiveFormulaSetupDetail(Int64 activePI)
        //{
        //    IEnumerable<vmIncentiveFormulaSetupDetail> objIncentiveFormulaSetupDetails = null;

        //    GenericFactory_vmIncentiveFormulaSetupDetail_GF = new vmIncentiveFormulaSetupDetail_GF();

        //    string spQuery = string.Empty;
        //    try
        //    {
        //        Hashtable ht = new Hashtable();
        //        ht.Add("BookingID", activePI);
        //        spQuery = "[Get_SalBookingDetail]";
        //        objIncentiveFormulaSetupDetails = GenericFactory_vmIncentiveFormulaSetupDetail_GF.ExecuteQuery(spQuery, ht);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return objIncentiveFormulaSetupDetails;
        //}

      
        public IEnumerable<vmIncentiveFormulaSetupMaster> GetIncentiveFormulaSetupMaster(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmIncentiveFormulaSetupMaster> objvmPIMaster = null;
            IEnumerable<vmIncentiveFormulaSetupMaster> objvmPIMasterWithOutPaging = null;
            recordsTotal = 0;
            
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "INCENTIVE.Get_DistTargetMaster";
            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetData(objCmd);
            objvmPIMasterWithOutPaging = ConvertDataTableToGenericList.BindList<vmIncentiveFormulaSetupMaster>(dt);
            objvmPIMaster = objvmPIMasterWithOutPaging.OrderByDescending(x => x.INCEN_ID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();

            recordsTotal = objvmPIMasterWithOutPaging.Count();
            return objvmPIMaster;
        }
        //public string DeleteMasterDetail(vmCmnParameters objcmnParam)
        //{
        //    IncentiveFormulaSetupMaster_GF = new IncentiveFormulaSetupMASTER_GF();
        //    string result = string.Empty;
        //    string spQuery = string.Empty;
        //    try
        //    {
        //        Hashtable ht = new Hashtable();
        //        ht.Add("CompanyID", objcmnParam.loggedCompany);
        //        ht.Add("LoggedUser", objcmnParam.loggeduser);
        //        ht.Add("IncentiveFormulaSetupID", objcmnParam.id);
        //        spQuery = "[Delete_BookingMasterDetail]";
        //        result = IncentiveFormulaSetupMaster_GF.ExecuteCommandString(spQuery, ht);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return result;
        //}
        public string SaveUpdateIncentiveFormulaSetup(vmIncentiveFormulaSetupMaster itemMaster)
        {
            string OutParam_ID = string.Empty;

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
            OracleCommand objCmd = new OracleCommand();

            #region INCENTIVE FORMULA
            if (itemMaster.BRAND_ID != null)
            {
                try
                {
                    using (objCmd.Connection = con)
                    {
                        objCmd.CommandType = CommandType.StoredProcedure;
                        objCmd.CommandText = "INCENTIVE.Set_IncentiveFormula";

                        objCmd.Parameters.Add("INCEN_ID", OracleDbType.Decimal, 35).Direction = ParameterDirection.Output;
                        objCmd.Parameters.Add("BRAND_ID", OracleDbType.Varchar2).Value = itemMaster.BRAND_ID;

                        objCmd.Parameters.Add("P_SALE_PERCENT", OracleDbType.Decimal).Value = itemMaster.P_SALE_PERCENT;

                        objCmd.Parameters.Add("S_SALE_PERCENT", OracleDbType.Decimal).Value = itemMaster.S_SALE_PERCENT;
                        objCmd.Parameters.Add("INCEN_PERCEN", OracleDbType.Decimal).Value = itemMaster.INCEN_PERCEN;
                        objCmd.Parameters.Add("START_DATE", OracleDbType.Date).Value = itemMaster.START_DATE;
                        objCmd.Parameters.Add("END_DATE", OracleDbType.Date).Value = itemMaster.END_DATE;
                        objCmd.Parameters.Add("SALES_FORCE_TYPE_ID", OracleDbType.Varchar2).Value = itemMaster.SALES_FORCE_TYPE_ID.Trim();
                        objCmd.Parameters.Add("MINACHNEXTMONTHCONS", OracleDbType.Decimal).Value = itemMaster.MINACHNEXTMONTHCONS;
                        objCmd.Parameters.Add("REMARKS", OracleDbType.Varchar2).Value = itemMaster.Remarks;
                        objCmd.Parameters.Add("IS_ACTIVE", OracleDbType.Varchar2).Value = "1";
                        objCmd.Parameters.Add("CREATED_BY", OracleDbType.Int32).Value = itemMaster.CREATED_BY;
                        objCmd.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                        objCmd.Parameters.Add("CREATED_IP", OracleDbType.Varchar2).Value = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(1).ToString();

                        objCmd.Connection.Open();
                        objCmd.ExecuteNonQuery();

                        OutParam_ID = objCmd.Parameters["INCEN_ID"].Value.ToString();
                    }
                }
                catch (OracleException exception)
                {
                    Console.WriteLine(exception.Message);
                    // may be you shouldn't return 0 here possibly throw;
                }
                finally
                {
                    objCmd.Connection.Close();
                }
            }
            #endregion INCENTIVE FORMULA           


            return OutParam_ID;
        }

        public string SaveUpdateIncentiveRateSetup(vmIncentiveRateDistRatio incentiveRate, List<vmIncentiveRateDistRatio> listDistRatio)
        {
            string OutParam_ID = string.Empty;

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
            OracleCommand objCmd = new OracleCommand();

            #region INCENTIVE RATIO
            if (incentiveRate.BRAND_ID != null)
            {
                string INS_RATE_DIST_RATIO = "";
                INS_RATE_DIST_RATIO = GetDetails(listDistRatio);
                try
                {
                    using (objCmd.Connection = con)
                    {
                        objCmd.CommandType = CommandType.StoredProcedure;
                        objCmd.CommandText = "INCENTIVE.Set_IncentiveRate";

                        objCmd.Parameters.Add("RATE_ID", OracleDbType.Decimal, 35).Direction = ParameterDirection.Output;
                        objCmd.Parameters.Add("BRAND_ID", OracleDbType.Varchar2).Value = incentiveRate.BRAND_ID;
                        objCmd.Parameters.Add("PER_QUANTITY", OracleDbType.Decimal).Value = incentiveRate.PER_QUANTITY;
                        objCmd.Parameters.Add("TAKA_AMOUNT", OracleDbType.Decimal).Value = incentiveRate.TAKA_AMOUNT;
                        objCmd.Parameters.Add("START_DATE", OracleDbType.Date).Value = incentiveRate.START_DATE;
                        objCmd.Parameters.Add("END_DATE", OracleDbType.Date).Value = incentiveRate.END_DATE;
                        objCmd.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;

                        objCmd.Parameters.Add("INS_RATE_DIST_RATIO_LIST", OracleDbType.Varchar2, 10000).Value = INS_RATE_DIST_RATIO;

                        objCmd.Connection.Open();
                        objCmd.ExecuteNonQuery();

                        OutParam_ID = objCmd.Parameters["RATE_ID"].Value.ToString();
                    }
                }
                catch (OracleException exception)
                {
                    Console.WriteLine(exception.Message);
                    // may be you shouldn't return 0 here possibly throw;
                }
                finally
                {
                    objCmd.Connection.Close();
                }
            }
            #endregion INCENTIVE RATIO

            return OutParam_ID;
        }

        public string SaveUpdateIncentiveAchievementRatio(vmIncentiveAchievementRatio incentiveAchievementRatio, List<vmIncentiveAchievementRatio> listIncentiveAchievementRatio)
        {
            string OutParam_ID = string.Empty;

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
            OracleCommand objCmd = new OracleCommand();

            #region INCENTIVE ACHIVEMENT RATIO
            if (incentiveAchievementRatio.INCEN_FORMU_ID != 0)
            {
                string INS_ACHIVE_RATIO = "";
                INS_ACHIVE_RATIO = GetDetailAchieveRatio(listIncentiveAchievementRatio);
                try
                {
                    using (objCmd.Connection = con)
                    {
                        objCmd.CommandType = CommandType.StoredProcedure;
                        objCmd.CommandText = "INCENTIVE.Set_IncenAchieveRatio";

                        objCmd.Parameters.Add("ACH_RATIO_ID", OracleDbType.Decimal, 35).Direction = ParameterDirection.Output;
                        objCmd.Parameters.Add("INCEN_FORMU_ID", OracleDbType.Decimal).Value = incentiveAchievementRatio.INCEN_FORMU_ID;
                        objCmd.Parameters.Add("SF_TYPE", OracleDbType.Varchar2).Value = incentiveAchievementRatio.SF_TYPE;
                        objCmd.Parameters.Add("START_DATE", OracleDbType.Date).Value = incentiveAchievementRatio.START_DATE;
                        objCmd.Parameters.Add("END_DATE", OracleDbType.Date).Value = incentiveAchievementRatio.END_DATE;
                        objCmd.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;

                        objCmd.Parameters.Add("INS_ACH_RATIO_LIST", OracleDbType.Varchar2, 10000).Value = INS_ACHIVE_RATIO;

                        objCmd.Connection.Open();
                        objCmd.ExecuteNonQuery();

                        OutParam_ID = objCmd.Parameters["ACH_RATIO_ID"].Value.ToString();
                    }
                }
                catch (OracleException exception)
                {
                    Console.WriteLine(exception.Message);
                    // may be you shouldn't return 0 here possibly throw;
                }
                finally
                {
                    objCmd.Connection.Close();
                }
            }
            #endregion

            return OutParam_ID;
        }

        public string GetDetails(List<vmIncentiveRateDistRatio> itemDetails)
        {
            string details = "";            

            foreach (var item in itemDetails)
            {
                string detail = "";

                string SF_TYPE = "";
                decimal? EACH_SF_TK_AMOUNT = 0;
                string CREATED_ON = DateTime.Now.Date.ToString("dd/MM/yyyy");

                SF_TYPE = item.SF_TYPE;
                EACH_SF_TK_AMOUNT = item.EACH_SF_TK_AMOUNT;

                detail = "x" + ':' + SF_TYPE + ':' + EACH_SF_TK_AMOUNT + ':' + CREATED_ON + ';';

                details += detail;
            }
            return details;
        }
        public string GetDetailAchieveRatio(List<vmIncentiveAchievementRatio> itemDetails)
        {
            string details = "";

            foreach (var item in itemDetails)
            {
                string detail = "";
                string BRAND_ID = "";

                decimal PRIMARY_PERC = 0;
                decimal? SECONDARY_PERC = 0;
                decimal PERCENTAGE_TK = 0;
                string CREATED_ON = DateTime.Now.Date.ToString("dd/MM/yyyy");

                BRAND_ID = item.BRAND_ID;
                PRIMARY_PERC = item.PRIMARY_PERC;
                SECONDARY_PERC = item.SECONDARY_PERC;
                PERCENTAGE_TK = item.PERCENTAGE_TK;

                detail = "x" + ':' + BRAND_ID + ':' + PRIMARY_PERC + ':' + SECONDARY_PERC + ':' + PERCENTAGE_TK + ':' + CREATED_ON + ';';

                details += detail;
            }
            return details;
        }

        public IEnumerable<vmBrandSKU> GetBrand(vmCmnParameters objcmnParam)
        {
            IEnumerable<vmBrandSKU> objBrand = null;

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);

            con.Open();
            string query = "SELECT OID AS BRANDID, SBRND_TEXT, SBRND_NAME, IDAT, EDAT FROM CITYN.T_SBRND";

            OracleCommand cmd = new OracleCommand(query, con);

            DataTable t1 = new DataTable();
            using (OracleDataAdapter a = new OracleDataAdapter(cmd))
            {
                a.Fill(t1);
            }
            con.Close();

            objBrand = t1.AsEnumerable().Select(dataRow => new vmBrandSKU
            {
                BRANDID = dataRow.Field<string>("BRANDID"),
                SBRND_TEXT = dataRow.Field<string>("SBRND_TEXT"),
                SBRND_NAME = dataRow.Field<string>("SBRND_NAME"),
                IDAT = dataRow.Field<DateTime>("IDAT"),
                EDAT = dataRow.Field<DateTime>("EDAT")
            }).ToList();
           
            return objBrand;
        }

        public IEnumerable<vmIncentiveFormulaSetupMaster> GetIncentiveFormula(vmCmnParameters objcmnParam)
        {
            IEnumerable<vmIncentiveFormulaSetupMaster> objIncenFormula = null;

            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "INCENTIVE.Get_IncentiveFormula";
            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetData(objCmd);

            objIncenFormula = dt.AsEnumerable().Select(dataRow => new vmIncentiveFormulaSetupMaster
            {
                INCEN_ID = dataRow.Field<decimal>("INCEN_ID"),
                BRAND_ID = dataRow.Field<string>("BRAND_ID")
            }).ToList();

            return objIncenFormula;
        }

        public IEnumerable<vmBrandSKU> GetBrandPopUp(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmBrandSKU> objBrand = null;
            IEnumerable<vmBrandSKU> objBrandWithOutPaging = null;
            recordsTotal = 0;

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);

            con.Open();
            string query = "SELECT OID AS BRANDID, SBRND_TEXT, SBRND_NAME, IDAT, EDAT FROM CITYN.T_SBRND";

            OracleCommand cmd = new OracleCommand(query, con);

            DataTable t1 = new DataTable();
            using (OracleDataAdapter a = new OracleDataAdapter(cmd))
            {
                a.Fill(t1);
            }
            con.Close();

            objBrandWithOutPaging = t1.AsEnumerable().Select(dataRow => new vmBrandSKU
            {
                BRANDID = dataRow.Field<string>("BRANDID"),
                SBRND_TEXT = dataRow.Field<string>("SBRND_TEXT"),
                SBRND_NAME = dataRow.Field<string>("SBRND_NAME"),
                IDAT = dataRow.Field<DateTime>("IDAT"),
                EDAT = dataRow.Field<DateTime>("EDAT")
            }).ToList();



            objBrand = objBrandWithOutPaging.OrderBy(x => x.BRANDID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();

            recordsTotal = objBrandWithOutPaging.Count();
            return objBrand;
        }
    }
}
