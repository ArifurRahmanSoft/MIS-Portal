using CTGroup.Models;
using CTGroup.Models.ViewModel.Sales;
using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel.ViewModel.Sales;
using CTGroup.Service.Sales.Factories;
using CTGroup.Service.Sales.Interfaces;
using CTGroup.Service.SystemCommon.Interfaces;
using CTGroup.Utility.Common;
using CTGroup.Web.Attributes;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Description;
using CTGroup.Web.Areas.Sales.Controllers;
using System.Text;
using CTGroup.Utility;
using System.Web;

namespace ABS.Web.Areas.Sales.api
{
    [RoutePrefix("Sales/api/SOWiseDocumentUpload")]
    public class SOWiseDocumentUploadController : ApiController
    {
        private iSOWiseDistTargetDocumentUploadMgt _objDocumentService = null;

        public static DateTime startDate, endDate;
        public static string LoggedUser;

        public SOWiseDocumentUploadController()
        {
            _objDocumentService = new SOWiseDistTargetDocumentUploadMgt();
        }

        public string KeepTargetDateRange(object[] data)
        {
            string message = "";
            vmDistributorTargetMaster itemMaster = JsonConvert.DeserializeObject<vmDistributorTargetMaster>(data[0].ToString());
            vmCmnParameters cmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[1].ToString());

            startDate = itemMaster.START_DATE;
            endDate = itemMaster.END_DATE;
            LoggedUser = cmnParam.loggeduser;

            List<vmDistributorTargetSOWise> test = _objDocumentService.DuplicateDataUploadChecking(startDate, endDate);

            if (itemMaster.START_DATE.Day != 1)
            {
                message = "Start date should be first date of the month";
            }
            if (itemMaster.END_DATE.Day != DateTime.DaysInMonth(itemMaster.END_DATE.Year, itemMaster.END_DATE.Month))
            {
                message = "End date should be last date of the month";
            }
            if (test.Count > 0)
            {
                message = "Target file already exists for this date range.";
            }
            return message;
        }
        public string InsertDocumentList(object[] data)
        {
            string DocumentId = "";
            List<vmDocuments> Documents = JsonConvert.DeserializeObject<List<vmDocuments>>(data[0].ToString());
            string multivalue = "";
            foreach (vmDocuments doc in Documents)
            {
                vmDocuments objDocument = new vmDocuments();
                objDocument.DocumentPathID = 1;
                objDocument.TransactionTypeName = "DistributorTargetSOWise";
                objDocument.FileName = doc.FileName;
                objDocument.DocumentTypeName = doc.FileType;

                string singlevlaue = "";

                singlevlaue = "x" + ':' + objDocument.DocumentPathID + ':' + objDocument.DocumentTypeName + ':' + objDocument.FileName +
                              ':' + objDocument.TransactionTypeName + ':' + ';';

                multivalue += singlevlaue;
            }
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
            OracleCommand objCmd = new OracleCommand();
            try
            {
                string clientIpAddress = HostService.GetLocalIPAddress();

                using (objCmd.Connection = con)
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "DATAUPLOAD.SET_DistTargetDoc_SOWise";
                    objCmd.Parameters.Add("P_DOCUMENT_ID", OracleDbType.Long, 35).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("P_CREATED_BY", OracleDbType.Varchar2).Value = LoggedUser;
                    objCmd.Parameters.Add("P_CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                    objCmd.Parameters.Add("P_CREATED_IP", OracleDbType.Varchar2).Value = clientIpAddress;
                    objCmd.Parameters.Add("P_START_DATE", OracleDbType.Date).Value = startDate;
                    objCmd.Parameters.Add("P_END_DATE", OracleDbType.Date).Value = endDate;
                    objCmd.Parameters.Add("P_DOCUMENTLIST", OracleDbType.Varchar2, 10000).Value = multivalue;

                    objCmd.Connection.Open();
                    objCmd.ExecuteNonQuery();

                    DocumentId = objCmd.Parameters["P_DOCUMENT_ID"].Value.ToString();
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
            return "";
        }

        [HttpPost]
        public dynamic UploadDocumentsView()
        {
            string sPath = string.Empty; string result = "1";
            StringBuilder dataMismatch = new StringBuilder();

            dynamic ListDocuments = new List<dynamic>();
            List<vmDistributorTargetSOWise> objvmPIMasterWithOutPaging = null;
            var util = new Utils();
            try
            {
                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                int totalfile = hfc.Count;
                var directory = @"" + WebConfigurationManager.AppSettings["SOWiseDistributorTargetNew"] + "";
                string dirName = new DirectoryInfo(directory).Name;
                for (int i = 0; i < totalfile; i++)
                {
                    System.Web.HttpPostedFile hpf = hfc[i];
                    if (hpf.ContentLength > 0)
                    {
                        //if (!File.Exists(sPath + Path.GetFileName(hpf.FileName)))
                        //{
                        string newName = DateTime.Now.ToString("ddMMMyyhhmmsstt");
                        string exttension = System.IO.Path.GetExtension(hpf.FileName);
                        int fileSerial = i + 1;
                        //string fileName = dirName + "_" + newName + fileSerial + exttension;
                        string fileName = dirName + "_" + newName + exttension;
                        fileName = fileName.Replace("/", "_");
                        string filePath = directory + fileName;

                        string res = util.UploadAssistant(hpf, fileName, directory);
                        if (!string.IsNullOrEmpty(res))
                        {
                            //hpf.SaveAs(filePath);
                            ListDocuments.Add(new ExpandoObject());
                            ListDocuments[i].FileId = fileSerial;
                            ListDocuments[i].FileType = hpf.ContentType;
                            ListDocuments[i].FileName = fileName;
                            ListDocuments[i].FileSize = hpf.ContentLength;
                            ListDocuments[i].FilePath = filePath;
                            ListDocuments[i].ModelState = "Inserted";

                            // for excel reading using access oledb connection
                            //string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                            //dt = ConvertXSLXtoDataTable(filePath, connString);

                            //DataTable dt = null;
                            //dt = ConvertXSLXtoDataTable(filePath);
                            dynamic resFile = JsonConvert.DeserializeObject(res);
                            byte[] sr = resFile[0].data;
                            DataTable dt = ConvertXSLXStreamtoDataTable(sr, fileName);

                            // validaing the so and distributor mapping

                            for (int start = 0; start < dt.Rows.Count; start++)
                            {
                                if (dt.Rows[start]["CUST_ID"].ToString() != "")
                                {
                                    OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
                                    con.Open();
                                    string query = "SELECT sprsn_name, scust_info_name  FROM CITYN.T_SPRSN salesperson " +
                                        "inner join CITYN.T_SCUSR sfnd on " +
                                        "salesperson.oid = sfnd.scusr_sprsn " +
                                        "inner join CITYN.T_SCUST_INFO DISTRIBUTOR on sfnd.scusr_scust_info = DISTRIBUTOR.oid " +
                                        "where salesperson.sprsn_text = '" + dt.Rows[start]["SO_ID"].ToString() + "' and " +
                                        "scust_info_text = '" + dt.Rows[start]["CUST_ID"] + "' and SCUST_INFO_ACTV = 1 ";
                                    OracleCommand cmd = new OracleCommand(query, con);
                                    DataTable t1 = new DataTable();
                                    using (OracleDataAdapter a = new OracleDataAdapter(cmd))
                                    {
                                        a.Fill(t1);
                                    }

                                    if (t1.Rows.Count == 0)
                                    {
                                        string mismatch = "Distributor code " + dt.Rows[start]["CUST_ID"] + " and SO code " + dt.Rows[start]["SO_ID"] + " mismatch. ";

                                        con.Close();
                                        dataMismatch.Append(mismatch);
                                        //dataMismatch.Append(mismatch + System.Environment.NewLine);
                                        dataMismatch.ToString();
                                        result = "0";
                                    }
                                    con.Close();
                                }
                            }

                            objvmPIMasterWithOutPaging = ConvertDataTableToGenericList.BindListFiltered<vmDistributorTargetSOWise>(dt);
                        }

                        hpf.InputStream.Dispose();
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return Json(new
            {
                result,
                dataMismatch,
                ListDocuments,
                objvmPIMasterWithOutPaging
            });
        }


        [HttpPost, BasicAuthorization]
        public HttpResponseMessage UploadDocumentsSaveSOWiseDistTarget(List<vmDistributorTargetSOWise> Listmodel)
        {
            string result = "";
            try
            {
                if (ModelState.IsValid && Listmodel.Count > 0)
                {
                    result = _objDocumentService.UploadDocumentsSaveSOWiseDistTarget(Listmodel, startDate, endDate, LoggedUser);
                }
                else
                {
                    result = "";
                }
            }
            catch (Exception e)
            {
                e.ToString();
                result = "";
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        public IHttpActionResult DeleteDistributorTargetRecord(object[] data)
        {
            string result = string.Empty;
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
                Int64 DocumentId = Convert.ToInt64(data[1]);
                if (DocumentId > 0)
                {
                    result = _objDocumentService.DeleteDistributorTargetRecord(objcmnParam, DocumentId);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                result = "0";
            }
            return Json(new
            {
                result
            });
        }


        public static DataTable ConvertXSLXtoDataTable(string filePath)
        {
            DataTable dt = new DataTable();
            try
            {
                Utils utl = new Utils();
                dt = utl.ExcelToDataTable(filePath);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i][1] = dt.Rows[i][1].ToString().PadLeft(6, '0');
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return dt;
        }

        public static DataTable ConvertXSLXStreamtoDataTable(byte[] fileByte, string fileName)
        {
            DataTable dt = new DataTable();
            try
            {
                Utils utl = new Utils();
                dt = utl.FileStreamExcelToDataTable(fileByte, fileName);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i][1] = dt.Rows[i][1].ToString().PadLeft(6, '0');
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return dt;
        }

        // for excel reading using access oledb connection
        //public static DataTable ConvertXSLXtoDataTable(string strFilePath, string connString)
        //{
        //    OleDbConnection oledbConn = new OleDbConnection(connString);
        //    DataTable dt = new DataTable();
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        oledbConn.Open();
        //        using (DataTable Sheets = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null))
        //        {

        //            for (int i = 0; i < Sheets.Rows.Count; i++)
        //            {
        //                string worksheets = Sheets.Rows[i]["TABLE_NAME"].ToString();
        //                OleDbCommand cmd = new OleDbCommand(String.Format("SELECT * FROM [{0}]", worksheets), oledbConn);
        //                OleDbDataAdapter oleda = new OleDbDataAdapter();
        //                oleda.SelectCommand = cmd;

        //                oleda.Fill(ds);
        //            }

        //            dt = ds.Tables[0];
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    finally
        //    {
        //        oledbConn.Close();
        //    }
        //    return dt;
        //}

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetDistTargetDocUploadMaster(object[] data)
        {
            IEnumerable<vmDistributorTargetSOWiseMaster> objMasterRecord = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objMasterRecord = _objDocumentService.GetDistTargetDocUploadMaster(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objMasterRecord
            });
        }
    }
}
