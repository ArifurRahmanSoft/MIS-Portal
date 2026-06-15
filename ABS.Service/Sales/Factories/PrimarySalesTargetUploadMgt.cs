using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using CTGroup.Service.Sales.Interfaces;
using CTGroup.Models.ViewModel.Sales;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using CTGroup.OracleModel.ViewModel.Sales;
using CTGroup.Utility.Common;
using System.Diagnostics;
using System.IO;

namespace CTGroup.Service.Sales.Factories
{
    public class PrimarySalesTargetUploadMgt : iPrimarySalesTargetUploadMgt
    {
        public List<vmDistributorTarget> DuplicateDataUploadChecking(DateTime startDate, DateTime endDate, string fileName)
        {
            string areaNational = "";

            if (fileName.StartsWith("Sun"))
                areaNational = "Sun";

            if (fileName.StartsWith("Consumer"))
                areaNational = "Consumer";

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
            con.Open();
            string query = "select START_DATE EXISTINGDATE from T_CMNDOCUMENT where START_DATE >= '" + startDate.ToString("dd-MMMM-yy")
                         + "' and END_DATE <= '" + endDate.ToString("dd-MMMM-yy") + "' AND ISDELETED = 0 "
                         + "and TRANSACTIONTYPE = 'PrimarySalesDistributorTarget' and AREA_NATIONAL =  '" + areaNational + "'   ";

            OracleCommand cmd = new OracleCommand(query, con);
            DataTable t1 = new DataTable();
            using (OracleDataAdapter a = new OracleDataAdapter(cmd))
            {
                a.Fill(t1);
            }
            con.Close();

            List<vmDistributorTarget> objDistributor = null;
            objDistributor = t1.AsEnumerable().Select(dataRow => new vmDistributorTarget
            {
                EXISTINGDATE = dataRow.Field<DateTime>("EXISTINGDATE")
            }).ToList();

            return objDistributor;
        }
        public bool UploadBulkData(List<vmDistributorTarget> bulkData, DateTime startDate, DateTime endDate, string LoggedUser, string fileName)
        {
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);

            bool returnValue = false;
            bool isProcessCompleted = false;

            if (fileName.StartsWith("Consumer")) // this is consumer data processing
            {
                try
                {
                    string query = @"insert into T_PRI_SALES_DIST_TAR_TEMP (CUST_ID, CUST_NAME, BCT_MT, BCT_SACK, TASO_CTN, TASO_MT, 
                                TCR_SACK, TCR_MT, TMO_CTN, TMO_MT, TSG_SACK, TSG_MT, JMW_CTN, TLD_SACK, TLD_MT, NVO_CTN,
                                NVO_MT,  TA_SACK, TA_MT, MD_SACK, MD_MT, SZ_SACK, SZ_MT, TWA_SACK, TWA_MT, TCN_CTN, TCN_MT, 
                                RICE_5_10_KG_SACK, RICE_5_10_KG_MT, TMU_MT, TMU_SACK, TFM_MT, TFM_CTN, THM_MT, THM_CTN, 
                                TBRM_MT, TBRM_CTN, TMM_MT, TMM_CTN, TBM_MT, TBM_CTN, TPIS_MT, TPIS_SACK, TCM_CTN, TCM_MT) 
                                values 
                              (:CUST_ID, :CUST_NAME, :BCT_MT, :BCT_SACK, :TASO_CTN, :TASO_MT, 
                               :TCR_SACK, :TCR_MT, :TMO_CTN, :TMO_MT, :TSG_SACK, :TSG_MT, :JMW_CTN, :TLD_SACK, :TLD_MT, :NVO_CTN,
                               :NVO_MT, :TA_SACK, :TA_MT, :MD_SACK, :MD_MT, :SZ_SACK, :SZ_MT, :TWA_SACK, :TWA_MT, :TCN_CTN, :TCN_MT, 
                               :RICE_5_10_KG_SACK, :RICE_5_10_KG_MT, :TMU_MT, :TMU_SACK, :TFM_MT, :TFM_CTN, :THM_MT, :THM_CTN, :TBRM_MT, 
                               :TBRM_CTN, :TMM_MT, :TMM_CTN, :TBM_MT, :TBM_CTN, :TPIS_MT, :TPIS_SACK, :TCM_CTN, :TCM_MT)";

                    con.Open();
                    using (var command = con.CreateCommand())
                    {
                        command.CommandText = query;
                        command.CommandType = CommandType.Text;
                        command.BindByName = true;
                        // In order to use ArrayBinding, the ArrayBindCount property
                        // of OracleCommand object must be set to the number of records to be inserted
                        command.ArrayBindCount = bulkData.Count;
                        command.Parameters.Add(":CUST_ID", OracleDbType.Varchar2, bulkData.Select(c => c.CUST_ID).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":CUST_NAME", OracleDbType.Varchar2, bulkData.Select(c => c.CUST_NAME).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":TASO_CTN", OracleDbType.Double, bulkData.Select(c => c.TASO_CTN).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":TASO_MT", OracleDbType.Double, bulkData.Select(c => c.TASO_MT).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":TCR_SACK", OracleDbType.Double, bulkData.Select(c => c.TCR_SACK).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":TCR_MT", OracleDbType.Double, bulkData.Select(c => c.TCR_MT).ToArray(), ParameterDirection.Input);


                        command.Parameters.Add(":TMO_CTN", OracleDbType.Double, bulkData.Select(c => c.TMO_CTN).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":TMO_MT", OracleDbType.Double, bulkData.Select(c => c.TMO_MT).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":TSG_SACK", OracleDbType.Double, bulkData.Select(c => c.TSG_SACK).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":TSG_MT", OracleDbType.Double, bulkData.Select(c => c.TSG_MT).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":JMW_CTN", OracleDbType.Double, bulkData.Select(c => c.JMW_CTN).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":TLD_SACK", OracleDbType.Double, bulkData.Select(c => c.TLD_SACK).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":TLD_MT", OracleDbType.Double, bulkData.Select(c => c.TLD_MT).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":NVO_CTN", OracleDbType.Double, bulkData.Select(c => c.NVO_CTN).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":NVO_MT", OracleDbType.Double, bulkData.Select(c => c.NVO_MT).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":BCT_SACK", OracleDbType.Double, bulkData.Select(c => c.BCT_SACK).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":BCT_MT", OracleDbType.Double, bulkData.Select(c => c.BCT_MT).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":TA_SACK", OracleDbType.Double, bulkData.Select(c => c.TA_SACK).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":TA_MT", OracleDbType.Double, bulkData.Select(c => c.TA_MT).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":MD_SACK", OracleDbType.Double, bulkData.Select(c => c.MD_SACK).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":MD_MT", OracleDbType.Double, bulkData.Select(c => c.MD_MT).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":SZ_SACK", OracleDbType.Double, bulkData.Select(c => c.SZ_SACK).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":SZ_MT", OracleDbType.Double, bulkData.Select(c => c.SZ_MT).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":TCN_CTN", OracleDbType.Double, bulkData.Select(c => c.TCN_CTN).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":TCN_MT", OracleDbType.Double, bulkData.Select(c => c.TCN_MT).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":TWA_SACK", OracleDbType.Double, bulkData.Select(c => c.TWA_SACK).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":TWA_MT", OracleDbType.Double, bulkData.Select(c => c.TWA_MT).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":RICE_5_10_KG_SACK", OracleDbType.Double, bulkData.Select(c => c.RICE_5_10_KG_SACK).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":RICE_5_10_KG_MT", OracleDbType.Double, bulkData.Select(c => c.RICE_5_10_KG_MT).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":TMU_MT", OracleDbType.Double, bulkData.Select(c => c.TMU_MT).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":TMU_SACK", OracleDbType.Double, bulkData.Select(c => c.TMU_SACK).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":TFM_MT", OracleDbType.Double, bulkData.Select(c => c.TFM_MT).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":TFM_CTN", OracleDbType.Double, bulkData.Select(c => c.TFM_CTN).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":THM_MT", OracleDbType.Double, bulkData.Select(c => c.THM_MT).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":THM_CTN", OracleDbType.Double, bulkData.Select(c => c.THM_CTN).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":TBRM_MT", OracleDbType.Double, bulkData.Select(c => c.TBRM_MT).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":TBRM_CTN", OracleDbType.Double, bulkData.Select(c => c.TBRM_CTN).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":TMM_MT", OracleDbType.Double, bulkData.Select(c => c.TMM_MT).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":TMM_CTN", OracleDbType.Double, bulkData.Select(c => c.TMM_CTN).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":TBM_MT", OracleDbType.Double, bulkData.Select(c => c.TBM_MT).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":TBM_CTN", OracleDbType.Double, bulkData.Select(c => c.TBM_CTN).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":TPIS_MT", OracleDbType.Double, bulkData.Select(c => c.TPIS_MT).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":TPIS_SACK", OracleDbType.Double, bulkData.Select(c => c.TPIS_SACK).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":TCM_CTN", OracleDbType.Double, bulkData.Select(c => c.TCM_CTN).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":TCM_MT", OracleDbType.Double, bulkData.Select(c => c.TCM_MT).ToArray(), ParameterDirection.Input);

                        int result = command.ExecuteNonQuery();

                        if (result == bulkData.Count)
                        {
                            returnValue = true;
                        }
                    }
                }
                catch (OracleException exception)
                {
                    var frame = new StackTrace(true).GetFrame(0);
                    var filename = frame.GetFileName();
                    var line = frame.GetFileLineNumber();

                    Utils u = new Utils();
                    u.LogWrite(exception, filename, line);
                }
                finally
                {
                    con.Close();
                }
            }
            else if (fileName.StartsWith("Sun")) //// this is Sun Data Processing
            {
                try
                {
                    string query = @"insert into T_PRI_SALES_DIST_TAR_TEMP (CUST_ID, CUST_NAME
                                                                            , SATA_MT, SMDA_MT, SSJI_MT, SSG_MT, SSO_MT, NVO_MT, SMO_MT, SPT_MT, TCR_MT, TLD_MT
                                                                            , SATA_SACK, SMDA_SACK, SSJI_SACK, SSG_SACK, SSO_CTN, NVO_CTN, SMO_CTN, SPT_SACK, TCR_SACK, TLD_SACK
                                                                            , JMW_CTN
                                                                            , TCN_MT, RICE_5_10_KG_MT, TMU_MT, TFM_MT, THM_MT, TBM_MT, TMM_MT, TBRM_MT
                                                                            , TCN_CTN, RICE_5_10_KG_SACK, TMU_SACK, TFM_CTN, THM_CTN, TBM_CTN, TMM_CTN, TBRM_CTN
                                                                            , SWA_MT, SCR_MT, SPIS_MT
                                                                            , SWA_SACK, SCR_SACK, SPIS_SACK
                                                                            , NDW_CTN) 
                                                                            values 
                                                                            (:CUST_ID, :CUST_NAME
                                                                            , :SATA_MT, :SMDA_MT, :SSJI_MT, :SSG_MT, :SSO_MT, :NVO_MT, :SMO_MT, :SPT_MT, :TCR_MT, :TLD_MT
                                                                            , :SATA_SACK, :SMDA_SACK, :SSJI_SACK, :SSG_SACK, :SSO_CTN, :NVO_CTN, :SMO_CTN, :SPT_SACK, :TCR_SACK, :TLD_SACK
                                                                            , :JMW_CTN
                                                                            , :TCN_MT, :RICE_5_10_KG_MT, :TMU_MT, :TFM_MT, :THM_MT, :TBM_MT, :TMM_MT, :TBRM_MT
                                                                            , :TCN_CTN, :RICE_5_10_KG_SACK, :TMU_SACK, :TFM_CTN, :THM_CTN, :TBM_CTN, :TMM_CTN, :TBRM_CTN
                                                                            , :SWA_MT, :SCR_MT, :SPIS_MT
                                                                            , :SWA_SACK, :SCR_SACK, :SPIS_SACK
                                                                            , :NDW_CTN)";

                    con.Open();
                    using (var command = con.CreateCommand())
                    {
                        command.CommandText = query;
                        command.CommandType = CommandType.Text;
                        command.BindByName = true;
                        // In order to use ArrayBinding, the ArrayBindCount property
                        // of OracleCommand object must be set to the number of records to be inserted
                        command.ArrayBindCount = bulkData.Count;

                        command.Parameters.Add(":CUST_ID", OracleDbType.Varchar2, bulkData.Select(c => c.CUST_ID).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":CUST_NAME", OracleDbType.Varchar2, bulkData.Select(c => c.CUST_NAME).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":SSJI_MT", OracleDbType.Double, bulkData.Select(c => c.SSJI_MT).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":SSJI_SACK", OracleDbType.Double, bulkData.Select(c => c.SSJI_SACK).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":SSG_SACK", OracleDbType.Double, bulkData.Select(c => c.SSG_SACK).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":SSG_MT", OracleDbType.Double, bulkData.Select(c => c.SSG_MT).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":SSO_MT", OracleDbType.Double, bulkData.Select(c => c.SSO_MT).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":SSO_CTN", OracleDbType.Double, bulkData.Select(c => c.SSO_CTN).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":SPT_SACK", OracleDbType.Double, bulkData.Select(c => c.SPT_SACK).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":SPT_MT", OracleDbType.Double, bulkData.Select(c => c.SPT_MT).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":SMO_CTN", OracleDbType.Double, bulkData.Select(c => c.SMO_CTN).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":SMO_MT", OracleDbType.Double, bulkData.Select(c => c.SMO_MT).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":SMDA_MT", OracleDbType.Double, bulkData.Select(c => c.SMDA_MT).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":SMDA_SACK", OracleDbType.Double, bulkData.Select(c => c.SMDA_SACK).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":SATA_MT", OracleDbType.Double, bulkData.Select(c => c.SATA_MT).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":SATA_SACK", OracleDbType.Double, bulkData.Select(c => c.SATA_SACK).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":NVO_CTN", OracleDbType.Double, bulkData.Select(c => c.NVO_CTN).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":NVO_MT", OracleDbType.Double, bulkData.Select(c => c.NVO_MT).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":TLD_SACK", OracleDbType.Double, bulkData.Select(c => c.TLD_SACK).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":TLD_MT", OracleDbType.Double, bulkData.Select(c => c.TLD_MT).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":TCR_MT", OracleDbType.Double, bulkData.Select(c => c.TCR_MT).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":TCR_SACK", OracleDbType.Double, bulkData.Select(c => c.TCR_SACK).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":SWA_MT", OracleDbType.Double, bulkData.Select(c => c.SWA_MT).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":SWA_SACK", OracleDbType.Double, bulkData.Select(c => c.SWA_SACK).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":SCR_MT", OracleDbType.Double, bulkData.Select(c => c.SCR_MT).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":SCR_SACK", OracleDbType.Double, bulkData.Select(c => c.SCR_SACK).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":SPIS_MT", OracleDbType.Double, bulkData.Select(c => c.SPIS_MT).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":SPIS_SACK", OracleDbType.Double, bulkData.Select(c => c.SPIS_SACK).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":TCN_MT", OracleDbType.Double, bulkData.Select(c => c.TCN_MT).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":TCN_CTN", OracleDbType.Double, bulkData.Select(c => c.TCN_CTN).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":RICE_5_10_KG_MT", OracleDbType.Double, bulkData.Select(c => c.RICE_5_10_KG_MT).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":RICE_5_10_KG_SACK", OracleDbType.Double, bulkData.Select(c => c.RICE_5_10_KG_SACK).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":TMU_MT", OracleDbType.Double, bulkData.Select(c => c.TMU_MT).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":TMU_SACK", OracleDbType.Double, bulkData.Select(c => c.TMU_SACK).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":TFM_MT", OracleDbType.Double, bulkData.Select(c => c.TFM_MT).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":TFM_CTN", OracleDbType.Double, bulkData.Select(c => c.TFM_CTN).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":THM_MT", OracleDbType.Double, bulkData.Select(c => c.THM_MT).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":THM_CTN", OracleDbType.Double, bulkData.Select(c => c.THM_CTN).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":TBM_MT", OracleDbType.Double, bulkData.Select(c => c.TBM_MT).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":TBM_CTN", OracleDbType.Double, bulkData.Select(c => c.TBM_CTN).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":TMM_MT", OracleDbType.Double, bulkData.Select(c => c.TMM_MT).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":TMM_CTN", OracleDbType.Double, bulkData.Select(c => c.TMM_CTN).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":TBRM_MT", OracleDbType.Double, bulkData.Select(c => c.TBRM_MT).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":TBRM_CTN", OracleDbType.Double, bulkData.Select(c => c.TBRM_CTN).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":JMW_CTN", OracleDbType.Double, bulkData.Select(c => c.JMW_CTN).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":NDW_CTN", OracleDbType.Double, bulkData.Select(c => c.NDW_CTN).ToArray(), ParameterDirection.Input);

                        int result = command.ExecuteNonQuery();

                        if (result == bulkData.Count)
                        {
                            returnValue = true;
                        }
                    }
                }
                catch (OracleException exception)
                {
                    var frame = new StackTrace(true).GetFrame(0);
                    var filename = frame.GetFileName();
                    var line = frame.GetFileLineNumber();

                    Utils u = new Utils();
                    u.LogWrite(exception, filename, line);
                }
                finally
                {
                    con.Close();
                }
            }
            else if (fileName.StartsWith("JMW")) //// this is Sun Data Processing
            {
                try
                {
                    string query = @"insert into T_PRI_SALES_DIST_TAR_TEMP (CUST_ID, CUST_NAME, JMW_CTN) 
                                                    values 
                                                                          ( :CUST_ID, :CUST_NAME, :JMW_CTN)";

                    con.Open();
                    using (var command = con.CreateCommand())
                    {
                        command.CommandText = query;
                        command.CommandType = CommandType.Text;
                        command.BindByName = true;
                        // In order to use ArrayBinding, the ArrayBindCount property
                        // of OracleCommand object must be set to the number of records to be inserted
                        command.ArrayBindCount = bulkData.Count;

                        command.Parameters.Add(":CUST_ID", OracleDbType.Varchar2, bulkData.Select(c => c.CUST_ID).ToArray(), ParameterDirection.Input);
                        command.Parameters.Add(":CUST_NAME", OracleDbType.Varchar2, bulkData.Select(c => c.CUST_NAME).ToArray(), ParameterDirection.Input);

                        command.Parameters.Add(":JMW_CTN", OracleDbType.Double, bulkData.Select(c => c.JMW_CTN).ToArray(), ParameterDirection.Input);

                        int result = command.ExecuteNonQuery();

                        if (result == bulkData.Count)
                        {
                            returnValue = true;
                        }
                    }
                }
                catch (OracleException exception)
                {
                    var frame = new StackTrace(true).GetFrame(0);
                    var filename = frame.GetFileName();
                    var line = frame.GetFileLineNumber();

                    Utils u = new Utils();
                    u.LogWrite(exception, filename, line);
                }
                finally
                {
                    con.Close();
                }
            }


            if (returnValue)
            {
                isProcessCompleted = ProcessData(startDate, endDate, LoggedUser, fileName);
            }
            return isProcessCompleted;
        }

        public bool ProcessData(DateTime startDate, DateTime endDate, string LoggedUser, string fileName)
        {
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);

            try
            {
                using (OracleCommand objCmd = new OracleCommand())
                {
                    using (objCmd.Connection = con)
                    {
                        objCmd.CommandType = CommandType.StoredProcedure;
                        objCmd.Parameters.Add("P_START_DATE", OracleDbType.Date).Value = startDate;
                        objCmd.Parameters.Add("P_END_DATE", OracleDbType.Date).Value = endDate;
                        objCmd.Parameters.Add("P_CREATED_BY", OracleDbType.Varchar2).Value = LoggedUser;
                        objCmd.Parameters.Add("P_CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                        objCmd.Parameters.Add("P_CREATED_IP", OracleDbType.Varchar2).Value = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(1).ToString();

                        if (fileName.StartsWith("Consumer"))
                            objCmd.CommandText = "Primary_SalesTarget_VS_Achievement.Set_ProcessDistTarget";

                        if (fileName.StartsWith("Sun"))
                            objCmd.CommandText = "Primary_SalesTarget_VS_Achievement.Set_ProcessDistTarget_Sun";

                        if (fileName.StartsWith("JMW"))
                            objCmd.CommandText = "Primary_SalesTarget_VS_Achievement.Set_ProcessDistTarget_JMW";

                        objCmd.Connection.Open();
                        objCmd.ExecuteNonQuery();
                        objCmd.Connection.Close();
                    }
                }
            }
            catch (OracleException exception)
            {
                var frame = new StackTrace(true).GetFrame(0);
                var filename = frame.GetFileName();
                var line = frame.GetFileLineNumber();

                Utils u = new Utils();
                u.LogWrite(exception, filename, line);
            }
            finally
            {
                con.Close();
            }
            return true;
        }
        public IEnumerable<vmDistributorTargetMaster> GetDistTargetDocUploadMaster(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmDistributorTargetMaster> objvmPIMaster = null;
            IEnumerable<vmDistributorTargetMaster> objvmPIMasterWithOutPaging = null;
            recordsTotal = 0;

            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "Primary_SalesTarget_VS_Achievement.Get_DistPrimaryTargetUploadMaster";
            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetData(objCmd);
            objvmPIMasterWithOutPaging = ConvertDataTableToGenericList.BindList<vmDistributorTargetMaster>(dt);
            objvmPIMaster = objvmPIMasterWithOutPaging.OrderByDescending(x => x.DIST_TARGET_ID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();

            recordsTotal = objvmPIMasterWithOutPaging.Count();
            return objvmPIMaster;
        }
        public int DeleteDistributorTargetRecord(vmCmnParameters objcmnParam, Int64 DocumentId)
        {
            int result = 0;

            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
            try
            {
                using (OracleCommand objCmd = new OracleCommand())
                {
                    using (objCmd.Connection = con)
                    {
                        objCmd.CommandType = CommandType.StoredProcedure;
                        objCmd.CommandText = "Primary_SalesTarget_VS_Achievement.DELETE_DISTTARGETRECORDS";
                        objCmd.Parameters.Add("DOCUMENTIDIN", OracleDbType.Decimal).Value = DocumentId;
                        objCmd.Parameters.Add("P_STARTDATE", OracleDbType.Date).Value = objcmnParam.startDate;
                        objCmd.Parameters.Add("P_ENDDATE", OracleDbType.Date).Value = objcmnParam.endDate;
                        objCmd.Parameters.Add("IS_DELETED", OracleDbType.Varchar2).Value = "1";
                        objCmd.Parameters.Add("DELETE_BY", OracleDbType.Varchar2).Value = objcmnParam.loggeduser;
                        objCmd.Parameters.Add("DELETE_ON", OracleDbType.Date).Value = DateTime.Now;
                        objCmd.Parameters.Add("DELETE_IP", OracleDbType.Varchar2).Value = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(1).ToString();
                        objCmd.Connection.Open();
                        objCmd.ExecuteNonQuery();
                        objCmd.Connection.Close();
                        result = 1;
                    }
                }
            }
            catch (OracleException exception)
            {
                result = -1;

                var frame = new StackTrace(true).GetFrame(0);
                var filename = frame.GetFileName();
                var line = frame.GetFileLineNumber();

                Utils u = new Utils();
                u.LogWrite(exception, filename, line);
            }
            finally
            {
                con.Close();
            }
            return result;
        }
    }
}

