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
using System.Web;
using CTGroup.Utility;
using Newtonsoft.Json.Linq;

namespace ABS.Web.Areas.Sales.api
{
    [RoutePrefix("Sales/api/AutoRiceSalesUpload")]
    public class AutoRiceSalesUploadController : ApiController
    {
        private iAutoRiceSalesUpload _objDocumentService = null;

        public static DateTime startDate, endDate;
        public static string LoggedUser;

        public AutoRiceSalesUploadController()
        {
            _objDocumentService = new AutoRiceSalesUploadMgt();
        }
        public string InsertDocumentListBak(object[] data)
        {
            string DocumentId = "";
            List<vmDocuments> Documents = JsonConvert.DeserializeObject<List<vmDocuments>>(data[0].ToString());
            string multivalue = "";
            foreach (vmDocuments doc in Documents)
            {
                vmDocuments objDocument = new vmDocuments();
                objDocument.DocumentPathID = 3; // this values comes from database.
                objDocument.TransactionTypeName = "AutoRicePrimarySales";
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
                using (objCmd.Connection = con)
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "Primary_SalesTarget_VS_Achievement.SET_DistTargetDoc";
                    objCmd.Parameters.Add("P_DOCUMENT_ID", OracleDbType.Long, 35).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("P_CREATED_BY", OracleDbType.Varchar2).Value = LoggedUser;
                    objCmd.Parameters.Add("P_CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                    objCmd.Parameters.Add("P_CREATED_IP", OracleDbType.Varchar2).Value = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(1).ToString();
                    objCmd.Parameters.Add("P_START_DATE", OracleDbType.Date).Value = startDate;
                    objCmd.Parameters.Add("P_END_DATE", OracleDbType.Date).Value = endDate;
                    objCmd.Parameters.Add("P_DOCUMENTLIST", OracleDbType.Varchar2, 10000).Value = multivalue;

                    objCmd.Connection.Open();
                    objCmd.ExecuteNonQuery();
                    objCmd.Connection.Close();

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
        public dynamic UploadDocumentsBackup()
        {
            string sPath = string.Empty; string result = string.Empty;
            object BDUpdate = null;

            dynamic ListDocuments = new List<dynamic>();
            List<vmAutoRiceSalesUpload> objvmAutoRiceSalesUpload = null;

            try
            {
                HttpFileCollection hfc = HttpContext.Current.Request.Files;
                vmCmnParameters cparam = JsonConvert.DeserializeObject<vmCmnParameters>(HttpContext.Current.Request.Form["data"]);

                int totalfile = hfc.Count;

                for (int i = 0; i < totalfile; i++)
                {
                    var directory = @"" + WebConfigurationManager.AppSettings["AutoRiceSalesUpload"] + "";

                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    string dirName = new DirectoryInfo(directory).Name;

                    HttpPostedFile hpf = hfc[i];
                    if (hpf.ContentLength > 0)
                    {
                        if (!File.Exists(sPath + Path.GetFileName(hpf.FileName)))
                        {
                            string newName = DateTime.Now.ToString("ddMMMyyhhmmsstt");
                            string exttension = System.IO.Path.GetExtension(hpf.FileName);
                            int fileSerial = i + 1;
                            //string fileName = dirName + "_" + newName + fileSerial + exttension;
                            string fileName = dirName + "_" + newName + exttension;
                            string filePath = directory + fileName;

                            hpf.SaveAs(filePath);
                            ListDocuments.Add(new ExpandoObject());
                            ListDocuments[i].FileId = fileSerial;
                            ListDocuments[i].FileType = hpf.ContentType;
                            ListDocuments[i].FileName = fileName;
                            ListDocuments[i].FileSize = hpf.ContentLength;
                            ListDocuments[i].FilePath = filePath;
                            ListDocuments[i].ModelState = "Inserted";
                            hpf.InputStream.Dispose();

                            DataTable dt = null;
                            dt = ConvertXSLXtoDataTables(filePath);
                            objvmAutoRiceSalesUpload = ConvertDataTableToGenericList.AutoRiceSalesUploadLists<vmAutoRiceSalesUpload>(dt);
                            BDUpdate = _objDocumentService.UploadBulkDatas(objvmAutoRiceSalesUpload, startDate, endDate, cparam.loggeduser);
                        }
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
                BDUpdate,
                ListDocuments,
                objvmAutoRiceSalesUpload
            });
        }

        [HttpPost]
        public dynamic UploadDocuments()
        {
            string sPath = string.Empty; string result = string.Empty;
            object BDUpdate = null;

            dynamic ListDocuments = new List<dynamic>();
            List<vmAutoRiceSalesUpload> objvmAutoRiceSalesUpload = null;
            var util = new Utils();
            try
            {
                HttpFileCollection hfc = HttpContext.Current.Request.Files;
                vmCmnParameters cparam = JsonConvert.DeserializeObject<vmCmnParameters>(HttpContext.Current.Request.Form["data"]);
                var directory = @"" + WebConfigurationManager.AppSettings["AutoRiceSalesUpload"] + "";
                string dirName = new DirectoryInfo(directory).Name;
                int totalfile = hfc.Count;
                for (int i = 0; i < totalfile; i++)
                {
                    HttpPostedFile hpf = hfc[i];
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

                            //DataTable dt = null;
                            //dt = ConvertXSLXtoDataTables(filePath);

                            dynamic resFile = JsonConvert.DeserializeObject(res);
                            byte[] sr = resFile[0].data;
                            DataTable dt = ConvertXSLXStreamtoDataTable(sr, fileName);

                            objvmAutoRiceSalesUpload = ConvertDataTableToGenericList.AutoRiceSalesUploadLists<vmAutoRiceSalesUpload>(dt);
                            BDUpdate = _objDocumentService.UploadBulkDatas(objvmAutoRiceSalesUpload, startDate, endDate, cparam.loggeduser);
                            //}
                            //}
                        }

                        hpf.InputStream.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return Json(new
            {
                BDUpdate,
                ListDocuments,
                objvmAutoRiceSalesUpload
            });
        }

        [HttpPost]
        public string InsertDocumentList(object[] data)
        {
            string DocumentId = "";
            List<vmDocuments> Documents = JsonConvert.DeserializeObject<List<vmDocuments>>(data[0].ToString());
            vmCmnParameters cparam = JsonConvert.DeserializeObject<vmCmnParameters>(data[1].ToString());
            string multivalue = "";
            foreach (vmDocuments doc in Documents)
            {
                vmDocuments objDocument = new vmDocuments();
                objDocument.DocumentPathID = 4; // this values comes from database.
                objDocument.TransactionTypeName = "AutoRiceSalesUpload";
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
                using (objCmd.Connection = con)
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "AUTORICESALESUPLOAD.SET_CmnDocument";
                    objCmd.Parameters.Add("P_DOCUMENT_ID", OracleDbType.Long, 35).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("P_CREATED_BY", OracleDbType.Varchar2).Value = cparam.loggeduser;
                    objCmd.Parameters.Add("P_CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                    objCmd.Parameters.Add("P_CREATED_IP", OracleDbType.Varchar2).Value = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(1).ToString();
                    objCmd.Parameters.Add("P_START_DATE", OracleDbType.Date).Value = startDate;
                    objCmd.Parameters.Add("P_END_DATE", OracleDbType.Date).Value = endDate;
                    objCmd.Parameters.Add("P_DOCUMENTLIST", OracleDbType.Varchar2, 10000).Value = multivalue;

                    objCmd.Connection.Open();
                    objCmd.ExecuteNonQuery();
                    objCmd.Connection.Close();

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

        public static DataTable ConvertXSLXtoDataTables(HttpPostedFile hpf)
        {
            DataTable dt = new DataTable();
            try
            {
                Utils utl = new Utils();
                dt = utl.ExcelToDataTable(hpf);
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

        public static DataTable ConvertXSLXtoDataTables(string filePath)
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

        [HttpPost]
        public IHttpActionResult DeleteDistributorTargetRecord(object[] data)
        {
            int result = 0;
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
                result = -1;
            }
            return Json(new
            {
                result
            });
        }
        public static DataTable ConvertXSLXtoDataTable(string strFilePath, string connString)
        {
            OleDbConnection oledbConn = new OleDbConnection(connString);
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                oledbConn.Open();
                using (DataTable Sheets = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null))
                {

                    for (int i = 0; i < Sheets.Rows.Count; i++)
                    {
                        string worksheets = Sheets.Rows[i]["TABLE_NAME"].ToString();
                        OleDbCommand cmd = new OleDbCommand(String.Format("SELECT * FROM [{0}]", worksheets), oledbConn);
                        OleDbDataAdapter oleda = new OleDbDataAdapter();
                        oleda.SelectCommand = cmd;

                        oleda.Fill(ds);
                    }

                    dt = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                oledbConn.Close();
            }
            return dt;
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetDistTargetDocUploadMaster(object[] data)
        {
            IEnumerable<vmDistributorTargetMaster> objMasterRecord = null;
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

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetCmnDocument(object[] data)
        {
            IEnumerable<vmCmnDocument> objMasterRecord = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objMasterRecord = _objDocumentService.GetCmnDocument(objcmnParam, out recordsTotal);
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
