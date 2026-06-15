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
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace ABS.Web.Areas.Sales.api
{
    [RoutePrefix("Sales/api/PrimarySalesTargetUpload")]
    public class PrimarySalesTargetUploadController : ApiController
    {
        private iPrimarySalesTargetUploadMgt _objDocumentService = null;

        public static DateTime startDate, endDate;
        public static string LoggedUser;
        public static string FileName;

        public PrimarySalesTargetUploadController()
        {
            _objDocumentService = new PrimarySalesTargetUploadMgt();
        }
        public HttpResponseMessage KeepTargetDateRange(object[] data)
        {
            vmDistributorTargetMaster itemMaster = JsonConvert.DeserializeObject<vmDistributorTargetMaster>(data[0].ToString());
            vmCmnParameters cmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[1].ToString());
            startDate = itemMaster.START_DATE;
            endDate = itemMaster.END_DATE;
            LoggedUser = cmnParam.loggeduser;
            FileName = itemMaster.FILE_NAME;


            var firstDayOfMonth = new DateTime(startDate.Year, startDate.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            List<vmDistributorTarget> distributorTargets = _objDocumentService.DuplicateDataUploadChecking(startDate, endDate, itemMaster.FILE_NAME);

            if (startDate > endDate)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Date selection is not okay");
            }
            else if (startDate.Month != endDate.Month)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Target should be in same month !!!");
            }
            else if (firstDayOfMonth != startDate || lastDayOfMonth != endDate)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Please select first date and last date of a month !!!");
            }
            else if (distributorTargets.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Target data already exists for this date range !!!");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
        }
        public string InsertDocumentList(object[] data)
        {
            string DocumentId = "";
            List<vmDocuments> Documents = JsonConvert.DeserializeObject<List<vmDocuments>>(data[0].ToString());
            string multivalue = "";
            string salesArea = "";
            foreach (vmDocuments doc in Documents)
            {
                vmDocuments objDocument = new vmDocuments();
                objDocument.DocumentPathID = 2; // this values comes from database.
                objDocument.TransactionTypeName = "PrimarySalesDistributorTarget";
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
                    if (FileName.StartsWith("Consumer"))
                        salesArea = "Consumer";

                    if (FileName.StartsWith("Sun"))
                        salesArea = "Sun";

                    if (FileName.StartsWith("JMW"))
                        salesArea = "JMW";

                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "Primary_SalesTarget_VS_Achievement.SET_DistTargetDoc";
                    objCmd.Parameters.Add("P_DOCUMENT_ID", OracleDbType.Long, 35).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("P_CREATED_BY", OracleDbType.Varchar2).Value = LoggedUser;
                    objCmd.Parameters.Add("P_CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                    objCmd.Parameters.Add("P_CREATED_IP", OracleDbType.Varchar2).Value = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(1).ToString();
                    objCmd.Parameters.Add("P_START_DATE", OracleDbType.Date).Value = startDate;
                    objCmd.Parameters.Add("P_END_DATE", OracleDbType.Date).Value = endDate;
                    objCmd.Parameters.Add("P_DOCUMENTLIST", OracleDbType.Varchar2, 10000).Value = multivalue;
                    objCmd.Parameters.Add("P_AREA_NATIONAL", OracleDbType.Varchar2).Value = salesArea;

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
            bool IsBDUpdate = false;

            dynamic ListDocuments = new List<dynamic>();
            List<vmDistributorTarget> objvmPIMasterWithOutPaging = null;

            try
            {
                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                int totalfile = hfc.Count;

                for (int i = 0; i < totalfile; i++)
                {
                    var directory = @"" + WebConfigurationManager.AppSettings["DistributorPrimaryTarget"] + "";

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
                            string datetime = DateTime.Now.ToString("ddMMMyyhhmmsstt");
                            string exttension = System.IO.Path.GetExtension(hpf.FileName);
                            int fileSerial = i + 1;
                            string fileName = Path.GetFileNameWithoutExtension(hpf.FileName) + "_" + datetime + exttension;
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

                            //string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                            //DataTable dt = null;
                            //dt = ConvertXSLXtoDataTable(filePath, connString);

                            DataTable dt = null;
                            dt = ConvertXSLXtoDataTable(filePath);

                            objvmPIMasterWithOutPaging = ConvertDataTableToGenericList.BindListFiltered<vmDistributorTarget>(dt);

                            IsBDUpdate = _objDocumentService.UploadBulkData(objvmPIMasterWithOutPaging, startDate, endDate, LoggedUser, hpf.FileName);

                            if (IsBDUpdate)
                            {
                                // ViewBag.Result = "Excel Uploaded Successfully !!!";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return Json(new
            {
                IsBDUpdate,
                ListDocuments,
                objvmPIMasterWithOutPaging
            });
        }

        [HttpPost]
        public dynamic UploadDocuments()
        {
            string sPath = string.Empty; string result = string.Empty; bool IsBDUpdate = false;
            dynamic ListDocuments = new List<dynamic>();
            List<vmDistributorTarget> objvmPIMasterWithOutPaging = null;
            var util = new Utils();
            try
            {
                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                int totalfile = hfc.Count;
                var directory = @"" + WebConfigurationManager.AppSettings["DistributorPrimaryTarget"] + "";
                for (int i = 0; i < totalfile; i++)
                {
                    System.Web.HttpPostedFile hpf = hfc[i];
                    if (hpf.ContentLength > 0)
                    {
                        string datetime = DateTime.Now.ToString("ddMMMyyhhmmsstt");
                        string exttension = System.IO.Path.GetExtension(hpf.FileName);
                        int fileSerial = i + 1;
                        string fileName = Path.GetFileNameWithoutExtension(hpf.FileName) + "_" + datetime + exttension;
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

                            //string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                            //DataTable dt = null;
                            //dt = ConvertXSLXtoDataTable(filePath, connString);

                            //DataTable dt = null;
                            //dt = ConvertXSLXtoDataTable(virtualPath);

                            dynamic resFile = JsonConvert.DeserializeObject(res);
                            byte[] sr = resFile[0].data;
                            DataTable dt = ConvertXSLXStreamtoDataTable(sr, fileName);

                            objvmPIMasterWithOutPaging = ConvertDataTableToGenericList.BindListFiltered<vmDistributorTarget>(dt);

                            IsBDUpdate = _objDocumentService.UploadBulkData(objvmPIMasterWithOutPaging, startDate, endDate, LoggedUser, hpf.FileName);

                            if (IsBDUpdate)
                            {
                                // ViewBag.Result = "Excel Uploaded Successfully !!!";
                            }
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
                IsBDUpdate,
                ListDocuments,
                objvmPIMasterWithOutPaging
            });
        }

        [HttpPost]
        public dynamic UploadDocument_Test()
        {
            string sPath = string.Empty; string result = string.Empty;
            bool IsBDUpdate = false;
            dynamic ListDocuments = new List<dynamic>();
            List<vmDistributorTarget> objvmPIMasterWithOutPaging = null;
            var util = new Utils();
            try
            {
                var req = System.Web.HttpContext.Current.Request;
                var hdr = req.Headers["filename"];
                var hfc = req.Files;
                int totalfile = hfc.Count;
                string url = "https://app.citygroupbd.com:222/api/DMSFileUpload/dms?directorypath=";
                for (int i = 0; i < totalfile; i++)
                {
                    var directory = @"" + WebConfigurationManager.AppSettings["DistributorPrimaryTarget"] + "";
                    string datetime = DateTime.Now.ToString("ddMMMyyhhmmsstt");
                    string exttension = System.IO.Path.GetExtension(hfc[i].FileName);
                    int fileSerial = i + 1;
                    string fileName = Path.GetFileNameWithoutExtension(hfc[i].FileName) + "_" + datetime + exttension;
                    fileName = fileName.Replace("/", "_");

                    string res = util.UploadAssistant_Test(hfc[i], fileName, directory);
                    dynamic resFile = JsonConvert.DeserializeObject(res);
                    //byte[] sr = resFile[0].data; //actual
                    byte[] sr = resFile.data; //test
                    DataTable dt = ConvertXSLXStreamtoDataTable(sr, fileName);
                    //string res = util.UploadAssistant(fileb, fileName, directory);
                    //DataTable dt = ConvertXSLXStreamtoDataTable(filea);
                    //if (res == "1")
                    //{
                    //    //DataTable dt = null;
                    //    //dt = ConvertXSLXtoDataTable(fetchDirectory);

                    //    //dt = ConvertXSLXStreamtoDataTable(hfc[i]);
                    //}
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return Json(new
            {
                IsBDUpdate,
                ListDocuments,
                objvmPIMasterWithOutPaging
            });
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

        //public static DataTable ConvertXSLXStreamtoDataTable_Test(byte[] fileByte, string fileName)
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        Utils utl = new Utils();
        //        dt = utl.FileStreamExcelToDataTable(fileByte, fileName);

        //        List<SOPRODUCT> sProdList = new List<SOPRODUCT>();
        //        SOPRODUCT sProd = new SOPRODUCT();
        //        List<SOPRODCOL> ColList = new List<SOPRODCOL>();
        //        SOPRODCOL col = new SOPRODCOL();                
        //        if (dt.Rows.Count > 0)
        //        {
        //            int i = 0;
        //            foreach (DataColumn column in dt.Columns)
        //            {
        //                if (i > 2)
        //                {
        //                    col = new SOPRODCOL();
        //                    col.COLNAME = column.ColumnName;
        //                    ColList.Add(col);
        //                }

        //                i++;
        //            }

        //            if (ColList.Count > 0)
        //            {
        //                foreach (DataRow dr in dt.Rows)
        //                {
        //                    foreach (var cols in ColList)
        //                    {
        //                        sProd = new SOPRODUCT();
        //                        sProd.PRODUCT_LINE = dr.ItemArray[0].ToString();
        //                        sProd.DIST_CODE = dr.ItemArray[1].ToString();
        //                        sProd.SO_CODE = dr.ItemArray[2].ToString();
        //                        sProd.SKU_CODE = cols.COLNAME;
        //                        sProd.CTN_QTY = string.IsNullOrEmpty(dr[cols.COLNAME].ToString()) ? 0 : Convert.ToDecimal(dr[cols.COLNAME]);
        //                        sProdList.Add(sProd);
        //                    }
        //                }

        //                if (sProdList.Count > 0) { 
                            
        //                }
        //            }
        //        }

        //        //if (dt.Rows.Count > 0)
        //        //{
        //        //    for (int i = 0; i < dt.Rows.Count; i++)
        //        //    {
        //        //        var rLine = dt.Rows[i];
        //        //    }
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return dt;
        //}

        //private class SOPRODUCT
        //{
        //    public string PRODUCT_LINE { get; set; }
        //    public string DIST_CODE { get; set; }
        //    public string SO_CODE { get; set; }
        //    public string SKU_CODE { get; set; }
        //    public decimal CTN_QTY { get; set; }
        //}

        //private class SOPRODCOL
        //{
        //    public string COLNAME { get; set; }
        //}

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
    }
}
