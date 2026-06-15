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
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

namespace CTGroup.Service.Sales.Factories
{
    public class SOWiseDistTargetUploadMgt : iSOWiseDistTargetUploadMgt
    {
        public List<vmDistributorTargetSOWise> DuplicateDataUploadChecking(DateTime startDate, DateTime endDate)
        {
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);

            con.Open();

            string query = "select START_DATE EXISTINGDATE from T_CMNDOCUMENT where START_DATE >= '" + startDate.ToString("dd-MMMM-yy")
                + "' and END_DATE <= '" + endDate.ToString("dd-MMMM-yy") + "' AND ISDELETED = 0 and TRANSACTIONTYPE = 'SOWiseDistributorTargetNew'";

            OracleCommand cmd = new OracleCommand(query, con);

            DataTable t1 = new DataTable();
            using (OracleDataAdapter a = new OracleDataAdapter(cmd))
            {
                a.Fill(t1);
            }
            con.Close();

            List<vmDistributorTargetSOWise> objDistributor = null;
            objDistributor = t1.AsEnumerable().Select(dataRow => new vmDistributorTargetSOWise
            {
                EXISTINGDATE = dataRow.Field<DateTime>("EXISTINGDATE")
            }).ToList();

            return objDistributor;
        }

        public IEnumerable<vmDistributorTargetSOWiseMaster> GetDistTargetDocUploadMaster(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmDistributorTargetSOWiseMaster> objvmPIMaster = null;
            IEnumerable<vmDistributorTargetSOWiseMaster> objvmPIMasterWithOutPaging = null;
            recordsTotal = 0;

            OracleCommand objCmd = new OracleCommand();
            objCmd.CommandText = "DATAUPLOAD_NEW.Get_DistTargetUploadMaster_SO";
            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
            DataTable dt = classDt.GetData(objCmd);
            objvmPIMasterWithOutPaging = ConvertDataTableToGenericList.BindList<vmDistributorTargetSOWiseMaster>(dt);
            objvmPIMaster = objvmPIMasterWithOutPaging.OrderByDescending(x => x.DIST_TARGET_ID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();

            recordsTotal = objvmPIMasterWithOutPaging.Count();
            return objvmPIMaster;
        }
        public string DeleteDistributorTargetRecord(vmCmnParameters objcmnParam, Int64 DocumentId)
        {
            Int32 currenttime = Convert.ToInt32(DateTime.Now.ToString("MMyyyy"));
            Int32 filetime = Convert.ToInt32(objcmnParam.startDate.ToString("MMyyyy"));

            string result = string.Empty;
            if (currenttime <= filetime)
            {
                OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
                OracleCommand objCmd = new OracleCommand();

                string clientIpAddress = HostService.GetLocalIPAddress();

                try
                {
                    using (objCmd.Connection = con)
                    {
                        objCmd.CommandType = CommandType.StoredProcedure;
                        objCmd.CommandText = "DATAUPLOAD_NEW.DELETE_SECONDARY_SALES_TARGET_SO_WISE_RECORDS";
                        objCmd.Parameters.Add("P_RESULT", OracleDbType.Varchar2).Direction = ParameterDirection.Output;
                        objCmd.Parameters.Add("DOCUMENTIDIN", OracleDbType.Decimal).Value = DocumentId;
                        objCmd.Parameters.Add("P_STARTDATE", OracleDbType.Date).Value = objcmnParam.startDate;
                        objCmd.Parameters.Add("P_ENDDATE", OracleDbType.Date).Value = objcmnParam.endDate;
                        objCmd.Parameters.Add("IS_DELETED", OracleDbType.Varchar2).Value = "1";
                        objCmd.Parameters.Add("DELETE_BY", OracleDbType.Varchar2).Value = objcmnParam.loggeduser;
                        objCmd.Parameters.Add("DELETE_ON", OracleDbType.Date).Value = DateTime.Now;
                        objCmd.Parameters.Add("DELETE_IP", OracleDbType.Varchar2).Value = clientIpAddress;
                        objCmd.Connection.Open();
                        objCmd.ExecuteNonQuery();
                        result = "1";
                    }
                }
                catch (OracleException exception)
                {
                    result = "0";

                    var frame = new StackTrace(true).GetFrame(0);
                    var filename = frame.GetFileName();
                    var line = frame.GetFileLineNumber();

                    Utils u = new Utils();
                    u.LogWrite(exception, filename, line);
                }
                finally
                {
                    objCmd.Connection.Close();
                }
            }
            else
            {
                result = "0";
            }
            return result;
        }

        public DataTable UploadSoWiseDistributorTarget(string _JsonData, DateTime startDate, DateTime endDate, string LoggedUser, string nationalId)
        {
            DataTable dt = new DataTable();
            try
            {
                OracleCommand ocmd = new OracleCommand();
                ocmd.CommandType = CommandType.StoredProcedure;
                ocmd.CommandText = "DATAUPLOAD_NEW.UPLOAD_SO_DIST_TARGET_TEMP";
                ocmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                ocmd.Parameters.Add("JsonData", OracleDbType.Clob).Value = _JsonData;
                ocmd.Parameters.Add("NationalOID", OracleDbType.Varchar2).Value = nationalId;
                ocmd.Parameters.Add("StartDate", OracleDbType.Date).Value = startDate;
                ocmd.Parameters.Add("EndDate", OracleDbType.Date).Value = endDate;
                ocmd.Parameters.Add("CreatedBy", OracleDbType.Varchar2).Value = LoggedUser;
                ocmd.Parameters.Add("CreatedPC", OracleDbType.Varchar2).Value = HostService.GetLocalIPAddress();
                ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
                dt = classDt.GetDataBasic(ocmd);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return dt;
        }

        public DataTable UploadSoWiseDistributorTargets(string _JsonData, DateTime startDate, DateTime endDate, string LoggedUser)
        {
            DataTable dt = new DataTable();
            try
            {
                OracleCommand ocmd = new OracleCommand();
                ocmd.CommandType = CommandType.StoredProcedure;
                ocmd.CommandText = "DATAUPLOAD_NEW.UPLOAD_SO_DIST_TARGET_TEMP";
                ocmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                ocmd.Parameters.Add("JsonData", OracleDbType.Clob).Value = _JsonData;
                ocmd.Parameters.Add("StartDate", OracleDbType.Date).Value = startDate;
                ocmd.Parameters.Add("EndDate", OracleDbType.Date).Value = endDate;
                ocmd.Parameters.Add("CreatedBy", OracleDbType.Varchar2).Value = LoggedUser;
                ocmd.Parameters.Add("CreatedPC", OracleDbType.Varchar2).Value = HostService.GetLocalIPAddress();
                ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();
                dt = classDt.GetData(ocmd);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return dt;
        }

        public string FinalSaveToUploadSoWiseTarget()
        {
            string result = string.Empty;
            OracleCommand ocmd = new OracleCommand();
            try
            {
                OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
                using (ocmd.Connection = con)
                {
                    ocmd.CommandType = CommandType.StoredProcedure;
                    ocmd.CommandText = "DATAUPLOAD_NEW.SET_SECONDARY_SO_WISE_TARGET_UPLOAD";
                    ocmd.Parameters.Add("P_RESULT", OracleDbType.Varchar2, 10).Direction = ParameterDirection.Output;
                    ocmd.Connection.Open();
                    ocmd.ExecuteNonQuery();
                    result = ocmd.Parameters["P_RESULT"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                result = "0";
                ex.ToString();
            }
            finally
            {
                ocmd.Connection.Close();
            }

            return result;
        }
    }
}

