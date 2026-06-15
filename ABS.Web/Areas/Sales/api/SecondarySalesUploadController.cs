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
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Description;

namespace ABS.Web.Areas.Sales.api
{
    [RoutePrefix("Sales/api/SecondarySalesUpload")]
    public class SecondarySalesUploadController : ApiController
    {
        private iSecondarySalesUploadMgt _objDocumentService = null;
        public static DateTime startDate, endDate;

        public SecondarySalesUploadController()
        {
            _objDocumentService = new SecondarySalesUploadMgt();
        }

        public string KeepSecondarySalesDate(object[] data)
        {
            vmDistributorTargetMaster itemMaster = JsonConvert.DeserializeObject<vmDistributorTargetMaster>(data[0].ToString());

            startDate = itemMaster.START_DATE;
            endDate = itemMaster.END_DATE;
            return "";
        }

        [HttpPost]
        public dynamic UploadDocuments()
        {
            string sPath = string.Empty; string result = string.Empty;

            dynamic ListDocuments = new List<dynamic>();
            try
            {
                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                int totalfile = hfc.Count;

                for (int i = 0; i < totalfile; i++)
                {
                    var directory = @"" + WebConfigurationManager.AppSettings["SecondarySales"] + "";

                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    string dirName = new DirectoryInfo(directory).Name;

                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

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

                            string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                            DataTable dt = null;
                            dt = ConvertXSLXtoDataTable(filePath, connString);

                            List<vmDistributorTarget> objvmPIMasterWithOutPaging = null;

                            //ConvertDataTableToGenericList classDt = new ConvertDataTableToGenericList();

                            objvmPIMasterWithOutPaging = ConvertDataTableToGenericList.BindListFiltered<vmDistributorTarget>(dt);
                            
                            bool IsDDUpdate = _objDocumentService.UploadBulkData(objvmPIMasterWithOutPaging, startDate, endDate);

                            if (IsDDUpdate)
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
                ListDocuments
            });
        }

        //[HttpPost, BasicAuthorization]
        //public HttpResponseMessage SaveUpdateDocumentList(object[] data)
        //{
        //    string result = string.Empty;
        //    try
        //    {
        //        vmDocuments master = JsonConvert.DeserializeObject<vmDocuments>(data[0].ToString());
        //        List<vmDocuments> ParentDocumentList = JsonConvert.DeserializeObject<List<vmDocuments>>(data[1].ToString());
        //        vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[2].ToString());
        //        result = _objDocumentService.SaveUpdateDocumentList(master, ParentDocumentList, objcmnParam);


        //        //vmDocuments master = JsonConvert.DeserializeObject<vmDocuments>(data[0].ToString());
        //        //if (master.FileId > 0)
        //        //{
        //        //    List<vmDocuments> ParentDocumentList = JsonConvert.DeserializeObject<List<vmDocuments>>(data[1].ToString());
        //        //    vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[2].ToString());
        //        //    result = _objDocumentService.UpdateDocumentList(master, ParentDocumentList, objcmnParam);
        //        //}
        //        //else
        //        //{
        //        //    List<vmDocuments> Documents = JsonConvert.DeserializeObject<List<vmDocuments>>(data[1].ToString());
        //        //    List<vmDocuments> ParentDocumentList = JsonConvert.DeserializeObject<List<vmDocuments>>(data[2].ToString());
        //        //    //List<vmDocuments> DelDocList = JsonConvert.DeserializeObject<List<vmDocuments>>(data[2].ToString());
        //        //    vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[3].ToString());
        //        //    result = _objDocumentService.SaveDocumentList(Documents, ParentDocumentList, objcmnParam);
        //        //}
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //        result = "";
        //    }
        //    return Request.CreateResponse(HttpStatusCode.OK, result);
        //}
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

                    //DataTable tblFiltered = dt.AsEnumerable().Where(row => row.Field<string>("CUST_ID") != string.Empty).CopyToDataTable();
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

        //[HttpPost, BasicAuthorization]
        //public IHttpActionResult GeDocumentTypeList(object[] data)
        //{
        //    List<vmDocuments> DocumentTypeList = null;
        //    vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
        //    try
        //    {
        //        DocumentTypeList = _objDocumentService.GeDocumentTypeList(objcmnParam);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return Json(new
        //    {
        //        DocumentTypeList
        //    });
        //}

        //[HttpPost, BasicAuthorization]
        //public IHttpActionResult GetDocList(object[] data)
        //{
        //    IEnumerable<vmDocuments> DocList = null;
        //    vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
        //    try
        //    {
        //        DocList = _objSystemCommon.GetDocList(objcmnParam).ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return Json(new
        //    {
        //        DocList
        //    });
        //}

        //[HttpPost, BasicAuthorization]
        //public IHttpActionResult GetParentDocList(object[] data)
        //{
        //    IEnumerable<vmDocuments> PDocList = null;
        //    vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
        //    try
        //    {
        //        PDocList = _objDocumentService.GetParentDocList(objcmnParam).ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return Json(new
        //    {
        //        PDocList
        //    });
        //}

        //[HttpPost, BasicAuthorization]
        //public IHttpActionResult DuplicateCheckDocName(object[] data)
        //{
        //    int result = 0;
        //    vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
        //    try
        //    {
        //        result = _objDocumentService.DuplicateCheckDocName(objcmnParam);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return Json(new
        //    {
        //        result
        //    });
        //}

        //[HttpPost, BasicAuthorization]
        //public IHttpActionResult GetDocumentTypeBaseData(object[] data)
        //{
        //    vmDocuments _objDocData = null;
        //    vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
        //    try
        //    {
        //        _objDocData = _objDocumentService.GetDocumentTypeBaseData(objcmnParam);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return Json(new
        //    {
        //        _objDocData
        //    });
        //}

        //[HttpPost, BasicAuthorization]
        //public HttpResponseMessage GetDocumentListByID(object[] data)
        //{
        //    IEnumerable<vmDocuments> DocByID = null;
        //    vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
        //    try
        //    {
        //        DocByID = _objDocumentService.GetDocumentListByID(objcmnParam);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return Request.CreateResponse(HttpStatusCode.OK, DocByID);
        //}

        //[HttpPost, BasicAuthorization]
        //public IHttpActionResult DocumentMasterList(object[] data)
        //{
        //    int recordsTotal = 0;
        //    List<vmDocuments> MasterData = null;
        //    vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
        //    try
        //    {
        //        MasterData = _objDocumentService.DocumentMasterList(objcmnParam, out recordsTotal).ToList();

        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return Json(new
        //    {
        //        recordsTotal,
        //        MasterData
        //    });

        //}

        //[HttpPost]
        //public IHttpActionResult getPathList(object[] data)
        //{
        //    string PathContainer = string.Empty;
        //    lstpath = JsonConvert.DeserializeObject<vmDocuments>(data[0].ToString());
        //    if (lstpath != null) { PathContainer = "1"; } else { PathContainer = "0"; }
        //    return Json(new
        //    {
        //        PathContainer
        //    });
        //}


        //[HttpPost]
        //public IHttpActionResult UploadDocuments(dynamic data)
        //{
        //    string sPath = string.Empty; string result = string.Empty;

        //    dynamic ListBillDocuments = new List<dynamic>(data);
        //    try
        //    {
        //        //var directory = @"E:/Upload/Quotation/";
        //        var directory = @"" + FileDirectory.Bill + "";

        //        if (!Directory.Exists(directory))
        //        {
        //            Directory.CreateDirectory(directory);
        //        }

        //        System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
        //        int totalfile = hfc.Count;

        //        for (int i = 0; i < totalfile; i++)
        //        {
        //            System.Web.HttpPostedFile hpf = hfc[i];
        //            if (hpf.ContentLength > 0)
        //            {
        //                if (!File.Exists(sPath + Path.GetFileName(hpf.FileName)))
        //                {
        //                    string newName = DateTime.Now.ToString("ddMMMyyhhmmsstt");
        //                    string exttension = System.IO.Path.GetExtension(hpf.FileName);
        //                    int fileSerial = i + 1;
        //                    string fileName = "Bill_" + newName + fileSerial + exttension;
        //                    string filePath = directory + fileName;

        //                    hpf.SaveAs(filePath);
        //                    ListBillDocuments.Add(new ExpandoObject());
        //                    ListBillDocuments[i].FileId = fileSerial;
        //                    ListBillDocuments[i].FileType = hpf.ContentType;
        //                    ListBillDocuments[i].FileName = fileName;
        //                    ListBillDocuments[i].FileSize = hpf.ContentLength;
        //                    ListBillDocuments[i].FilePath = filePath;
        //                    ListBillDocuments[i].ModelState = "Inserted";
        //                    hpf.InputStream.Dispose();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //    return Json(new
        //    {
        //        ListBillDocuments
        //    });
        //}

        //[HttpPost]
        //public IHttpActionResult DeleteDocuments(vmDocuments fileinfo)
        //{
        //    int result = 0;
        //    string filePath = fileinfo.FilePath;

        //    try
        //    {
        //        if (filePath != null)
        //        {
        //            System.IO.File.Delete(filePath);
        //            result = 1;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //        result = -1;
        //    }

        //    return Json(new
        //    {
        //        result
        //    });
        //}

        //public IHttpActionResult MoveDocuments(vmDocuments fileinfo)
        //{
        //    string result = string.Empty;
        //    var directory = Path.GetDirectoryName(@"" + fileinfo.FilePath + "").ToString() + @"\";
        //    var prevPath = Path.GetDirectoryName(@"" + fileinfo.PrevFilePath + "").ToString() + @"\";
        //    string dirName = new DirectoryInfo(directory).Name;
        //    if (!Directory.Exists(directory))
        //    {
        //        Directory.CreateDirectory(directory);
        //    }

        //    string newName = DateTime.Now.ToString("ddMMMyyhhmmsstt");
        //    string exttension = System.IO.Path.GetExtension(fileinfo.FileName);
        //    string fileName = dirName + "_" + newName + exttension;

        //    string NewFile = directory + fileName;
        //    string prevFile = prevPath + fileinfo.FileName;

        //    try
        //    {
        //        if (prevFile != null && NewFile != null)
        //        {
        //            System.IO.File.Move(prevFile, NewFile);
        //            result = fileName;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //        result = "";
        //    }

        //    return Json(new
        //    {
        //        result
        //    });
        //}     

        //[HttpPost, BasicAuthorization]
        //public IHttpActionResult DeleteDocumentList(object[] data)
        //{
        //    string result = string.Empty;
        //    vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
        //    try
        //    {
        //        //if (ModelState.IsValid)
        //        //{
        //        result = _objDocumentService.DeleteDocumentList(objcmnParam);
        //        //}
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //        result = "0";
        //    }
        //    return Json(new
        //    {
        //        result
        //    });
        //}

    }
}
