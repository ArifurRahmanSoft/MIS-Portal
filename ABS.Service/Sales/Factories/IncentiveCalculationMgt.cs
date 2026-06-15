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
    public class IncentiveCalculationMgt : iIncentiveCalculationMgt
    {
       
       // private iGenericFactory<T_DIST_TAR_MASTER> IncentiveCalculationMaster_GF = null;
       // private iGenericFactory<vmIncentiveCalculationDetail> GenericFactory_vmIncentiveCalculationDetail_GF = null;

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

        public IEnumerable<vmIncentiveCalculationMaster> GetIncentiveCalculationMaster(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmIncentiveCalculationMaster> objvmPIMaster = null;
            IEnumerable<vmIncentiveCalculationMaster> objvmPIMasterWithOutPaging = null;
            recordsTotal = 0;
            
            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "INCENTIVE.Get_DistTargetMaster";
            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetData(objCmd);
            objvmPIMasterWithOutPaging = ConvertDataTableToGenericList.BindList<vmIncentiveCalculationMaster>(dt);
            objvmPIMaster = objvmPIMasterWithOutPaging.OrderByDescending(x => x.DIST_TARGET_ID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();

            recordsTotal = objvmPIMasterWithOutPaging.Count();
            return objvmPIMaster;
        }
        public string GetDetails(List<vmIncentiveCalculationDetail> itemDetails)
        {
            string details = "";

            foreach (var item in itemDetails)
            {
                string detail = "";
                int UOMID = 0;
                string BRANDID = "";
                string CREATEDIP = "1.1.1.1";
                int CREATEDBY = 1;
                decimal? QUANTITY = 0;

                BRANDID = item.BRANDID;
                QUANTITY = item.QUANTITY;
                UOMID = 8;
                CREATEDIP = "1.1.1.1";
                string CREATEDDATE = DateTime.Now.Date.ToString("dd/MM/yyyy");


                CREATEDBY = 1;

                detail = "x" + ':' + BRANDID + ':' + QUANTITY 
                             + ':' + UOMID + ':' + CREATEDIP + ':' + CREATEDDATE + ':' + CREATEDBY + ';';

                details += detail;
            }
            return details;
        }
        public IEnumerable<vmIncentiveCalculationMaster> CalculatePrimarySale(vmCmnParameters objcmnParam)
        {

            List<vmIncentiveCalculationMaster> sSalesAreas = new List<vmIncentiveCalculationMaster>();

            OracleCommand objCmd = new OracleCommand();

            objCmd.CommandText = "INCENTIVE.Get_CalculatePrimarySale";

            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("p_distributorId", OracleDbType.Varchar2).Value = objcmnParam.distributorId;
            objCmd.Parameters.Add("p_startDate", OracleDbType.Varchar2).Value = objcmnParam.startDate;
            objCmd.Parameters.Add("p_endDate", OracleDbType.Varchar2).Value = objcmnParam.endDate;

            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();

            DataTable dt = classDt.GetData(objCmd);
            sSalesAreas = ConvertDataTableToGenericList.BindList<vmIncentiveCalculationMaster>(dt);
            return sSalesAreas;
        }

        public IEnumerable<vmIncentiveCalculationMaster> GetTargetPrimarySecondarySale(vmCmnParameters objcmnParam)
        {

            List<vmIncentiveCalculationMaster> sSalesAreas = new List<vmIncentiveCalculationMaster>();

            OracleCommand objCmd = new OracleCommand();

            objCmd.CommandText = "INCENTIVE.Get_TargetPrimarySecondarySale";

            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("p_distributorId", OracleDbType.Varchar2).Value = objcmnParam.distributorId;
            objCmd.Parameters.Add("p_startDate", OracleDbType.Varchar2).Value = objcmnParam.startDate;
            objCmd.Parameters.Add("p_endDate", OracleDbType.Varchar2).Value = objcmnParam.endDate;

            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();

            DataTable dt = classDt.GetData(objCmd);
            sSalesAreas = ConvertDataTableToGenericList.BindList<vmIncentiveCalculationMaster>(dt);
            return sSalesAreas;
        }

        public IEnumerable<vmIncentiveCalculationMaster> IncentiveCalculation(vmCmnParameters objcmnParam)
        {

            List<vmIncentiveCalculationMaster> sSalesAreas = new List<vmIncentiveCalculationMaster>();

            OracleCommand objCmd = new OracleCommand();

            objCmd.CommandText = "INCENTIVEDATAPROCESS.Get_IncentiveCalculation";

            objCmd.CommandType = CommandType.StoredProcedure;
            
            objCmd.Parameters.Add("p_startDate", OracleDbType.Varchar2).Value = objcmnParam.startDate;
            objCmd.Parameters.Add("p_endDate", OracleDbType.Varchar2).Value = objcmnParam.endDate;

            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();

            DataTable dt = classDt.GetData(objCmd);
            sSalesAreas = ConvertDataTableToGenericList.BindList<vmIncentiveCalculationMaster>(dt);
            return sSalesAreas;
        }
    }
}
