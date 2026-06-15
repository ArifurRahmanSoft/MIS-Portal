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
using CTGroup.Utility;
using System.Web;

namespace CTGroup.Web.Areas.Sales.api
{
    [RoutePrefix("Sales/api/CardConsumerDataUpload")]
    public class CardConsumerDataUploadController : ApiController
    {
        private iCardConsumerDataUploadMgt _objDocumentService = null;

        public static string LoggedUser;

        public CardConsumerDataUploadController()
        {
            _objDocumentService = new CardConsumerDataUploadMgt();
        }

        public List<vmDistributorTarget> KeepTargetDateRange(object[] data)
        {
            vmCmnParameters cmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            LoggedUser = cmnParam.loggeduser;
            return null;
        }


        [HttpPost, BasicAuthorization]
        public IHttpActionResult CardConsumerReportData(object[] data)
        {
            IEnumerable<vmCardConsumerDataUpload> objReportData = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objReportData = _objDocumentService.CardConsumerReportData(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objReportData
            });
        }

        [HttpPost]
        public dynamic UploadDocumentsViewBackup()
        {
            string sPath = string.Empty; string result = string.Empty;
            bool IsBDUpdate = false;

            dynamic ListDocuments = new List<dynamic>();
            List<vmCardConsumerDataUpload> objvmCardConsumerDataUpload = null;

            try
            {
                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                int totalfile = hfc.Count;

                for (int i = 0; i < totalfile; i++)
                {
                    var directory = @"" + WebConfigurationManager.AppSettings["CardConsumerDataUpload"] + "";

                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    string dirName = new DirectoryInfo(directory).Name;

                    System.Web.HttpPostedFile hpf = hfc[i];
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

                            // for excel reading using access oledb connection
                            //string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                            //dt = ConvertXSLXtoDataTable(filePath, connString);

                            DataTable dt = null;
                            dt = ConvertXSLXtoDataTable(filePath);

                            System.Data.DataColumn UPLOAD_BY = new System.Data.DataColumn("UPLOAD_BY", typeof(System.String));
                            UPLOAD_BY.DefaultValue = LoggedUser;
                            dt.Columns.Add(UPLOAD_BY);

                            objvmCardConsumerDataUpload = ConvertDataTableToGenericList.BindListExcelData<vmCardConsumerDataUpload>(dt);
                            IsBDUpdate = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                var frame = new StackTrace(true).GetFrame(0);
                var filename = frame.GetFileName();
                var line = frame.GetFileLineNumber();

                Utils u = new Utils();
                u.LogWrite(ex, filename, line);
            }
            return Json(new
            {
                IsBDUpdate,
                objvmCardConsumerDataUpload
            });
        }

        [HttpPost]
        public dynamic UploadDocumentsView()
        {
            string sPath = string.Empty; string result = string.Empty;
            bool IsBDUpdate = false;

            dynamic ListDocuments = new List<dynamic>();
            List<vmCardConsumerDataUpload> objvmCardConsumerDataUpload = null;
            var util = new Utils();
            try
            {
                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                var directory = @"" + WebConfigurationManager.AppSettings["CardConsumerDataUpload"] + "";
                string dirName = new DirectoryInfo(directory).Name;
                int totalfile = hfc.Count;

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

                            System.Data.DataColumn UPLOAD_BY = new System.Data.DataColumn("UPLOAD_BY", typeof(System.String));
                            UPLOAD_BY.DefaultValue = LoggedUser;
                            dt.Columns.Add(UPLOAD_BY);

                            objvmCardConsumerDataUpload = ConvertDataTableToGenericList.BindListExcelData<vmCardConsumerDataUpload>(dt);
                            IsBDUpdate = true;
                            //}
                        }

                        hpf.InputStream.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                var frame = new StackTrace(true).GetFrame(0);
                var filename = frame.GetFileName();
                var line = frame.GetFileLineNumber();

                Utils u = new Utils();
                u.LogWrite(ex, filename, line);
            }
            return Json(new
            {
                IsBDUpdate,
                objvmCardConsumerDataUpload
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


        [HttpPost, BasicAuthorization]
        public HttpResponseMessage UploadDocumentsSave(List<vmCardConsumerDataUpload> Listmodel)
        {
            string result = "";
            try
            {
                if (ModelState.IsValid && Listmodel.Count > 0)
                {
                    result = _objDocumentService.UploadDocumentsSave(Listmodel);
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

                    //for (int i = 0; i < Sheets.Rows.Count; i++)
                    for (int i = 0; i < 1; i++)
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
                var frame = new StackTrace(true).GetFrame(0);
                var filename = frame.GetFileName();
                var line = frame.GetFileLineNumber();

                Utils u = new Utils();
                u.LogWrite(ex, filename, line);
            }
            finally
            {
                oledbConn.Close();
            }
            return dt;
        }

        //[HttpPost, BasicAuthorization]
        //public IHttpActionResult GetDistTargetDocUploadMasterBackup(object[] data)
        //{
        //    IEnumerable<vmCardConsumerMaster> objMasterRecord = null;
        //    vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
        //    int recordsTotal = 0;
        //    try
        //    {
        //        objMasterRecord = _objDocumentService.GetDistTargetDocUploadMasterBackup(objcmnParam, out recordsTotal);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return Json(new
        //    {
        //        recordsTotal,
        //        objMasterRecord
        //    });
        //}

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetDistTargetDocUploadMaster(object[] data)
        {
            DataTable objMasterRecord = null;
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
