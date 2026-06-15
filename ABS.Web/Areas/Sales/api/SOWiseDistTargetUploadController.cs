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
    [RoutePrefix("Sales/api/SOWiseDistTargetUpload")]
    public class SOWiseDistTargetUploadController : ApiController
    {
        private iSOWiseDistTargetUploadMgt _objDocumentService = null;

        public static DateTime startDate, endDate;
        public static string LoggedUser;
        public static string nationalId;
        public SOWiseDistTargetUploadController()
        {
            _objDocumentService = new SOWiseDistTargetUploadMgt();
        }

        public string KeepTargetDateRange(object[] data)
        {
            string message = "";
            vmDistributorTargetMaster itemMaster = JsonConvert.DeserializeObject<vmDistributorTargetMaster>(data[0].ToString());
            vmCmnParameters cmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[1].ToString());

            startDate = itemMaster.START_DATE;
            endDate = itemMaster.END_DATE;
            LoggedUser = cmnParam.loggeduser;

            //List<vmDistributorTargetSOWise> test = _objDocumentService.DuplicateDataUploadChecking(startDate, endDate);

            if (itemMaster.START_DATE.Day != 1)
            {
                message = "Start date should be first date of the month";
            }
            if (itemMaster.END_DATE.Day != DateTime.DaysInMonth(itemMaster.END_DATE.Year, itemMaster.END_DATE.Month))
            {
                message = "End date should be last date of the month";
            }
            //if (test.Count > 0)
            //{
            //    message = "Target file already exists for this date range.";
            //}
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
                objDocument.DocumentPathID = 3;
                objDocument.TransactionTypeName = "SOWiseDistributorTargetNew";
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
                    objCmd.CommandText = "DATAUPLOAD_NEW.SET_DistTargetDoc_SOWise";
                    objCmd.Parameters.Add("P_DOCUMENT_ID", OracleDbType.Long, 35).Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add("P_CREATED_BY", OracleDbType.Varchar2).Value = LoggedUser;
                    objCmd.Parameters.Add("P_CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                    objCmd.Parameters.Add("P_CREATED_IP", OracleDbType.Varchar2).Value = clientIpAddress;
                    objCmd.Parameters.Add("P_START_DATE", OracleDbType.Date).Value = startDate;
                    objCmd.Parameters.Add("P_END_DATE", OracleDbType.Date).Value = endDate;
                    objCmd.Parameters.Add("P_DOCUMENTLIST", OracleDbType.Varchar2, 10000).Value = multivalue;
                    objCmd.Parameters.Add("P_AREA_NATIONAL", OracleDbType.Varchar2).Value = nationalId;

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
            string sPath = string.Empty; string result = string.Empty;
            StringBuilder dataMismatch = new StringBuilder();
            List<SOPRODUCT> sProdList = new List<SOPRODUCT>();
            List<SOPRODCOL> colList = new List<SOPRODCOL>();
            dynamic ListDocuments = new List<dynamic>();
            DataTable excelDt = new DataTable();
            DataTable misDt = new DataTable();
            var util = new Utils();
            nationalId = string.Empty;
            try
            {
                nationalId = System.Web.HttpContext.Current.Request["nationalid"].ToString();
                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                int totalfile = hfc.Count;
                var directory = @"" + WebConfigurationManager.AppSettings["SOWiseDistributorTargetNew"] + "";
                string dirName = new DirectoryInfo(directory).Name;
                for (int i = 0; i < totalfile; i++)
                {
                    System.Web.HttpPostedFile hpf = hfc[i];
                    if (hpf.ContentLength > 0)
                    {
                        string newName = DateTime.Now.ToString("ddMMMyyhhmmsstt");
                        string exttension = System.IO.Path.GetExtension(hpf.FileName);
                        int fileSerial = i + 1;
                        string fileName = dirName + "_" + newName + exttension;
                        fileName = fileName.Replace("/", "_");
                        string filePath = directory + fileName;

                        string res = util.UploadAssistant(hpf, fileName, directory);
                        if (!string.IsNullOrEmpty(res))
                        {
                            ListDocuments.Add(new ExpandoObject());
                            ListDocuments[i].FileId = fileSerial;
                            ListDocuments[i].FileType = hpf.ContentType;
                            ListDocuments[i].FileName = fileName;
                            ListDocuments[i].FileSize = hpf.ContentLength;
                            ListDocuments[i].FilePath = filePath;
                            ListDocuments[i].ModelState = "Inserted";

                            dynamic resFile = JsonConvert.DeserializeObject(res);
                            byte[] sr = resFile[0].data;
                            dynamic gres = ConvertXSLXStreamtoClass(sr, fileName, nationalId);

                            colList = gres.Content.colList;
                            sProdList = gres.Content.sProdList;                            
                            excelDt = gres.Content.excelDt;
                            misDt= gres.Content.misDt;

                            if (misDt.Rows.Count > 0)
                            {
                                List<SODIST> misList = ConvertDataTableToGenericList.BindList<SODIST>(misDt);

                                foreach (var sd in misList)
                                {
                                    string mismatch = "Distributor code " + sd.DIST_CODE + " and SO code " + sd.SO_CODE + " mismatch. ";
                                    dataMismatch.Append(mismatch);
                                }

                                result = "0";
                            }
                            else {
                                result = "1";
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
                result,
                dataMismatch,
                ListDocuments,
                excelDt,
                colList,
                sProdList,
                misDt
            });
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

        public object ConvertXSLXStreamtoClass(byte[] fileByte, string fileName, string nationalId)
        {
            List<SOPRODUCT> sProdList = new List<SOPRODUCT>();
            List<SOPRODCOL> colList = new List<SOPRODCOL>();
            DataTable excelDt = new DataTable();
            DataTable misDt = new DataTable();
            Utils utl = new Utils();
            try
            {
                excelDt = utl.FileStreamExcelToDataTable(fileByte, fileName);
                SOPRODUCT sProd = new SOPRODUCT();                
                SOPRODCOL col = new SOPRODCOL();
                if (excelDt.Rows.Count > 0)
                {
                    int i = 0;
                    foreach (DataColumn column in excelDt.Columns)
                    {
                        if (i > 2)
                        {
                            col = new SOPRODCOL();
                            col.COLNAME = column.ColumnName;
                            colList.Add(col);
                        }

                        i++;
                    }

                    if (colList.Count > 0)
                    {
                        foreach (DataRow dr in excelDt.Rows)
                        {
                            foreach (var cols in colList)
                            {
                                sProd = new SOPRODUCT();
                                sProd.PROD_LINE = dr.ItemArray[0].ToString();
                                sProd.DIST_CODE = dr.ItemArray[1].ToString();
                                sProd.SO_CODE = dr.ItemArray[2].ToString();
                                sProd.SKU_CODE = cols.COLNAME;
                                sProd.CTN_QTY = string.IsNullOrEmpty(dr[cols.COLNAME].ToString()) ? 0 : Convert.ToDecimal(dr[cols.COLNAME]);
                                sProdList.Add(sProd);
                            }
                        }

                        if (sProdList.Count > 0)
                        {
                            string json = JsonConvert.SerializeObject(sProdList);
                            misDt = _objDocumentService.UploadSoWiseDistributorTarget(json, startDate, endDate, LoggedUser, nationalId);                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return Json(new
            {
                colList,
                sProdList,
                misDt,
                excelDt                
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult FinalSaveToUploadSoWiseTarget(object[] data)
        {
            string result = string.Empty;
            try
            {
                result = _objDocumentService.FinalSaveToUploadSoWiseTarget();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                result
            });
        }

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
