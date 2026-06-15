using CTGroup.Data.BaseInterfaces;
using CTGroup.OracleModel;
using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.Service.AllServiceClasses;
using CTGroup.Service.SystemCommon.Interfaces;
using CTGroup.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using CTGroup.Service.Sales.Interfaces;
using CTGroup.Models.ViewModel.Sales;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using CTGroup.OracleModel.ViewModel.Sales;

namespace CTGroup.Service.Sales.Factories
{
    public class SecondarySalesUploadMgt : iSecondarySalesUploadMgt
    {
        public bool UploadBulkData(List<vmDistributorTarget> bulkData, DateTime startDate, DateTime endDate)
        {
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);

            bool returnValue = false;
            try
            {
                string query = @"insert into T_DIST_SECON_SALE_TEMP ( CUST_ID, CUST_NAME, TASO_C, SSO_C, TCR_C, TMO_C, TSG_C, TMR, JMW_C, TLD_C, NVO_C, BCT,  TA_C, MD_C, SZ_C, TWA_C) 
                                                values (:CUST_ID, :CUST_NAME, :TASO_C, :SSO_C, :TCR_C, :TMO_C, :TSG_C, :TMR, :JMW_C, :TLD_C, :NVO_C, :BCT, :TA_C, :MD_C, :SZ_C, :TWA_C )";
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
                    command.Parameters.Add(":TASO_C", OracleDbType.Decimal, bulkData.Select(c => c.TASO_C).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":SSO_C", OracleDbType.Decimal, bulkData.Select(c => c.SSO_C).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":TCR_C", OracleDbType.Decimal, bulkData.Select(c => c.TCR_C).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":TMO_C", OracleDbType.Decimal, bulkData.Select(c => c.TMO_C).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":TSG_C", OracleDbType.Decimal, bulkData.Select(c => c.TSG_C).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":TMR", OracleDbType.Decimal, bulkData.Select(c => c.TMR).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":JMW_C", OracleDbType.Decimal, bulkData.Select(c => c.JMW_C).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":TLD_C", OracleDbType.Decimal, bulkData.Select(c => c.TLD_C).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":NVO_C", OracleDbType.Decimal, bulkData.Select(c => c.NVO_C).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":BCT", OracleDbType.Decimal, bulkData.Select(c => c.BCT).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":TA_C", OracleDbType.Decimal, bulkData.Select(c => c.TA_C).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":MD_C", OracleDbType.Decimal, bulkData.Select(c => c.MD_C).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":SZ_C", OracleDbType.Decimal, bulkData.Select(c => c.SZ_C).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":TWA_C", OracleDbType.Decimal, bulkData.Select(c => c.TWA_C).ToArray(), ParameterDirection.Input);
                    int result = command.ExecuteNonQuery();
                    if (result == bulkData.Count)
                        returnValue = true;
                }
            }
            catch (OracleException ex)
            {
                //Log error thrown
            }
            finally
            {
                con.Close();
            }

            if (returnValue)
            {
                bool isTrue = ProcessData(startDate, endDate);
            }

            return returnValue;
        }

        public bool ProcessData(DateTime startDate, DateTime endDate)
        {
            OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString);
            OracleCommand objCmd = new OracleCommand();
            try
            {
                using (objCmd.Connection = con)
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.Add("START_DATE", OracleDbType.Date).Value = startDate;
                    objCmd.Parameters.Add("END_DATE", OracleDbType.Date).Value = endDate;
                    objCmd.Parameters.Add("CREATED_BY", OracleDbType.Int32).Value = 1;
                    objCmd.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                    objCmd.Parameters.Add("CREATED_IP", OracleDbType.Varchar2).Value = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(1).ToString();

                    objCmd.CommandText = "INCENTIVE.Set_ProcessDistSecondarySale";

                    objCmd.Connection.Open();
                    objCmd.ExecuteNonQuery();
                }
            }
            catch (OracleException exception)
            {
            }
            finally
            {
                objCmd.Connection.Close();
            }
            return true;
        }



        //public List<vmDocuments> GeDocumentTypeList(vmCmnParameters objcmnParam)
        //{
        //    List<vmDocuments> styles = null;
        //    try
        //    {
        //        using (_ctxCmn = new ERP_Entities())
        //        {
        //            styles = (from IMM in _ctxCmn.CmnDocumentTypes
        //                      where IMM.IsDeleted == false
        //                      select new vmDocuments
        //                      {
        //                          DocumentTypeID = IMM.DocumentTypeID,
        //                          DocumentTypeName = IMM.DocumentTypeName
        //                      }).ToList();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return styles;
        //}

        //public List<vmDocuments> GetParentDocList(vmCmnParameters objcmnParam)
        //{
        //    GenericFactory_vmCmnDocument_GF = new vmDocuments_GF();
        //    List<vmDocuments> PDocL = null;
        //    string spQuery = string.Empty;
        //    try
        //    {
        //        using (_ctxCmn = new ERP_Entities())
        //        {
        //            Hashtable ht = new Hashtable();
        //            ht.Add("CompanyID", objcmnParam.loggedCompany);
        //            ht.Add("DocumentParentID", objcmnParam.id);

        //            spQuery = "[Get_ParentDocumentList]";
        //            PDocL = GenericFactory_vmCmnDocument_GF.ExecuteQuery(spQuery, ht).ToList();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return PDocL;
        //}

        ////public List<vmDocuments> GetParentDocList(vmCmnParameters objcmnParam)
        ////{
        ////    List<vmDocuments> PDocL = null;
        ////    try
        ////    {
        ////        using (_ctxCmn = new ERP_Entities())
        ////        {
        ////            PDocL = (from CDPS in _ctxCmn.CmnDocumentParents
        ////                     join CD in _ctxCmn.CmnDocuments on CDPS.DocumentID equals CD.DocumentID
        ////                     join CDT in _ctxCmn.CmnDocumentTypes on CD.DocumnetTypeID equals CDT.DocumentTypeID
        ////                     join CDP in _ctxCmn.CmnDocumentPaths on CD.DocumentPahtID equals CDP.DocumentPathID
        ////                     where CDPS.DocumentParentID == objcmnParam.id && CDP.IsDeleted == false
        ////                     select new vmDocuments
        ////                     {
        ////                         FileId = CDPS.DocumentID,
        ////                         FileName = CD.DocumentName,
        ////                         DocName = CD.DocName,
        ////                         DocumentTypeName = CDT.DocumentTypeName,
        ////                         DocumentPahtID = CD.DocumentPahtID,
        ////                         DocumentTypeID = CD.DocumnetTypeID,
        ////                         FileSize = CD.DocSize,
        ////                         FileType = CD.DocType,
        ////                         FilePath = CDP.PhysicalPath,
        ////                         Remarks = CD.Remarks,
        ////                         VirtualPath = CDP.VirtualPath + CDT.DocumentTypeName + "/" + CD.DocumentName,
        ////                         ViewPath = CDP.VirtualPath + CDT.DocumentTypeName + "/" + CD.DocumentName,
        ////                         //id = CDPS.DocumentParentID,
        ////                         ModelState = "Updated"
        ////                     }).ToList();
        ////        }
        ////    }
        ////    catch (Exception e)
        ////    {
        ////        e.ToString();
        ////    }
        ////    return PDocL;
        ////}

        //public vmDocuments GetDocumentTypeBaseData(vmCmnParameters objcmnParam)
        //{
        //    _ctxCmn = new ERP_Entities();
        //    vmDocuments _objDocData = null;
        //    try
        //    {
        //        _objDocData = (from DT in _ctxCmn.CmnDocumentTypes
        //                       join DP in _ctxCmn.CmnDocumentPaths on DT.DocumentTypeID equals DP.DocumentTypeID
        //                       where DT.DocumentTypeID == objcmnParam.id
        //                       select new vmDocuments
        //                       {
        //                           DocumentPahtID = DP.DocumentPathID,
        //                           DocumentTypeName = DT.DocumentTypeName,
        //                           FilePath = DP.PhysicalPath,
        //                           VirtualPath = DP.VirtualPath
        //                       }
        //                     ).FirstOrDefault();
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return _objDocData;
        //}

        //public List<vmDocuments> DocumentMasterList(vmCmnParameters objcmnParam, out int recordsTotal)
        //{
        //    GenericFactory_vmCmnDocument_GF = new vmDocuments_GF();
        //    List<vmDocuments> MasterData = null;
        //    string spQuery = string.Empty;
        //    recordsTotal = 0;
        //    try
        //    {
        //        using (_ctxCmn = new ERP_Entities())
        //        {
        //            Hashtable ht = new Hashtable();
        //            ht.Add("CompanyID", objcmnParam.loggedCompany);
        //            ht.Add("LoggedUser", objcmnParam.loggeduser);
        //            ht.Add("PageNo", objcmnParam.pageNumber);
        //            ht.Add("RowCountPerPage", objcmnParam.pageSize);
        //            ht.Add("IsPaging", objcmnParam.IsPaging);

        //            spQuery = "[Get_DocumentMasterList]";
        //            MasterData = GenericFactory_vmCmnDocument_GF.ExecuteQuery(spQuery, ht).ToList();
        //            recordsTotal = _ctxCmn.CmnDocuments.Where(x => x.CompanyID == objcmnParam.loggedCompany && x.IsDeleted == false).Count();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return MasterData;
        //}
        ////public vmMrrBillMasterDetail GetPurchaseBillMasterByID(vmCmnParameters objcmnParam)
        ////{
        ////    GenericFactory_vmMrrBillMasterDetail_GF = new vmMrrBillMasterDetail_GF();
        ////    vmMrrBillMasterDetail MasterByID = null;
        ////    string spQuery = string.Empty;
        ////    try
        ////    {
        ////        Hashtable ht = new Hashtable();
        ////        ht.Add("MRRBillID", objcmnParam.id);
        ////        spQuery = "[Get_PurchaseBillMasterByID]";
        ////        MasterByID = GenericFactory_vmMrrBillMasterDetail_GF.ExecuteQuerySingle(spQuery, ht);
        ////    }
        ////    catch (Exception e)
        ////    {
        ////        e.ToString();
        ////    }
        ////    return MasterByID;
        ////}

        //public IEnumerable<vmDocuments> GetDocumentListByID(vmCmnParameters objcmnParam)
        //{
        //    GenericFactory_vmCmnDocument_GF = new vmDocuments_GF();
        //    IEnumerable<vmDocuments> DocByID = null;
        //    string spQuery = string.Empty;
        //    try
        //    {
        //        Hashtable ht = new Hashtable();
        //        ht.Add("DocumentID", objcmnParam.id);
        //        spQuery = "[Get_DocumentListByID]";
        //        DocByID = GenericFactory_vmCmnDocument_GF.ExecuteQuery(spQuery, ht);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return DocByID;
        //}

        //public int DuplicateCheckDocName(vmCmnParameters objcmnParam)
        //{
        //    int result = 0;
        //    using (_ctxCmn = new ERP_Entities())
        //    {
        //        try
        //        {
        //            if (objcmnParam.id > 0)
        //            {
        //                result = _ctxCmn.CmnDocuments.Where(x => x.DocName == objcmnParam.ParamName && x.DocumentID == objcmnParam.id && x.IsDeleted == false).FirstOrDefault() != null ? 0 : 1;

        //                if (result == 1)
        //                {
        //                    result = _ctxCmn.CmnDocuments.Where(x => x.DocName == objcmnParam.ParamName && x.IsDeleted == false).FirstOrDefault() == null ? 0 : 1;
        //                }
        //            }
        //            else
        //            {
        //                result = _ctxCmn.CmnDocuments.Where(x => x.DocName == objcmnParam.ParamName && x.IsDeleted == false).FirstOrDefault() == null ? 0 : 1;
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            e.ToString();
        //        }
        //        return result;
        //    }
        //}

        ////public IEnumerable<vmDocuments> GetDocumentListByID(vmCmnParameters objcmnParam)
        ////{
        ////    _ctxCmn = new ERP_Entities();
        ////    List<vmDocuments> DocByID = null;
        ////    try
        ////    {
        ////        DocByID = (from CD in _ctxCmn.CmnDocuments.ToList()
        ////                   join CDP in _ctxCmn.CmnDocumentPaths.ToList() on CD.DocumentPahtID equals CDP.DocumentPathID
        ////                   where CD.TransactionID == objcmnParam.id && CD.IsDeleted == false && CD.TransactionTypeID == objcmnParam.tTypeId
        ////                   select new vmDocuments
        ////                   {
        ////                       DocumentPahtID = (int)CD.DocumentPahtID,
        ////                       FileId = (int)CD.DocumentID,
        ////                       FileName = CD.DocumentName,
        ////                       FilePath = CDP.PhysicalPath + CD.DocumentName,
        ////                       FileSize = CD.DocSize, //int.Parse(System.IO.File.OpenRead(CDP.PhysicalPath + CD.DocumentName).Length.ToString()),//
        ////                       FileType = CD.DocType, //System.IO.File.OpenRead(CDP.PhysicalPath + CD.DocumentName).GetType().ToString(),
        ////                       ModelState = "Updated"
        ////                   }
        ////                   ).ToList();

        ////    }
        ////    catch (Exception e)
        ////    {
        ////        e.ToString();
        ////    }
        ////    return DocByID;
        ////}
        //public string SaveUpdateDocumentList(vmDocuments Master, List<vmDocuments> ParentDocumentList, vmCmnParameters objcmnParam)
        //{
        //    string Rsult = string.Empty;
        //    Rsult = SaveDocumentList(Master, ParentDocumentList, objcmnParam);
        //    return Rsult;
        //}

        //private string SaveDocumentList(vmDocuments Master, List<vmDocuments> ParentDocumentList, vmCmnParameters objcmnParam) //List<vmDocuments> DelDocList, 
        //{
        //    string result = string.Empty;
        //    using (var transaction = new TransactionScope())
        //    {
        //        //*********************************************Start Initialize Variable*****************************************             
        //        long FirstDigitDocument = 0, OtherDigitsDocument = 0, nextDocumentId = 0;
        //        //***************************************End Initialize Variable*************************************************
        //        //**************************Start Initialize Generic Repository Based on table***********************************
        //        _ctxCmn = new ERP_Entities();
        //        GenericFactory_CmnDocument_EF = new CmnDocument_EF();
        //        //****************************End Initialize Generic Repository Based on table***********************************

        //        //**********************************Start Create Related Table Instance to Save**********************************                
        //        var objDoc = new CmnDocument();
        //        var CmnDocParent = new List<CmnDocumentParent>();
        //        //************************************End Create Related Table Instance to Save**********************************
        //        //**************************************************Start Main Operation************************************************                
        //        try
        //        {

        //            nextDocumentId = Convert.ToInt64(GenericFactory_CmnDocument_EF.getMaxID("CmnDocument"));
        //            //FirstDigitDocument = Convert.ToInt64(nextDocumentId.ToString().Substring(0, 1));
        //            //OtherDigitsDocument = Convert.ToInt64(nextDocumentId.ToString().Substring(1, nextDocumentId.ToString().Length - 1));
        //            objDoc.DocumentID = nextDocumentId;//Convert.ToInt64(FirstDigitDocument + "" + OtherDigitsDocument);
        //            objDoc.DocumentPahtID = Master.DocumentPahtID;
        //            objDoc.TransactionTypeID = objcmnParam.tTypeId;
        //            objDoc.DocName = Master.DocName;
        //            objDoc.DocumentName = Master.FileName;
        //            objDoc.DocSize = Master.FileSize;
        //            objDoc.DocType = Master.FileType;
        //            objDoc.Remarks = Master.Remarks;
        //            objDoc.DocumnetTypeID = Master.DocumentTypeID;

        //            objDoc.CompanyID = objcmnParam.loggedCompany;
        //            objDoc.CreateBy = objcmnParam.loggeduser;
        //            objDoc.CreateOn = DateTime.Now;
        //            objDoc.IsDeleted = false;
        //            objDoc.CreatePc = HostService.GetIP();
        //            //***************************************************End Save Operation************************************************
                    

        //            //***************************************************Start Update************************************************
        //            _ctxCmn.CmnDocuments.Add(objDoc);
        //            //GenericFactory_CmnDocument_EF.updateMaxID("CmnDocument", Convert.ToInt64(FirstDigitDocument + "" + (OtherDigitsDocument - 1)));
        //            GenericFactory_CmnDocument_EF.updateMaxID("CmnDocument", Convert.ToInt64(nextDocumentId));

        //            if (CmnDocParent != null && CmnDocParent.Count != 0)
        //            {
        //                _ctxCmn.CmnDocumentParents.AddRange(CmnDocParent.ToList());
        //            }
        //            //***************************************************End Update************************************************

        //            _ctxCmn.SaveChanges();
        //            transaction.Complete();
        //            result = "1";
        //        }
        //        catch (Exception e)
        //        {
        //            e.ToString();
        //            result = "0";
        //        }
        //    }
        //    return result;
        //    //**************************************************End Main Operation************************************************
        //}

        //private string UpdateDocumentList(vmDocuments Master, List<vmDocuments> ParentDocumentList, vmCmnParameters objcmnParam) //List<vmDocuments> DelDocList, 
        //{
        //    string result = string.Empty;
        //    using (var transaction = new TransactionScope())
        //    {
        //        //**************************Start Initialize Generic Repository Based on table***********************************
        //        _ctxCmn = new ERP_Entities();
        //        var CmnDocParent = new List<CmnDocumentParent>();
        //        var UCmnDocParent = new List<CmnDocumentParent>();
        //        //**************************************************Start Main Operation************************************************
        //        if (Master.FileId > 0)
        //        {
        //            try
        //            {
        //                //***********************************Start Get Data From Related Table to Update*********************************                            
        //                var objDocument = _ctxCmn.CmnDocuments.Where(x => x.DocumentID == Master.FileId).FirstOrDefault();

        //                objDocument.DocumentPahtID = Master.DocumentPahtID;
        //                objDocument.TransactionTypeID = objcmnParam.tTypeId;
        //                objDocument.DocName = Master.DocName;
        //                objDocument.DocumentName = Master.FileName;
        //                objDocument.DocSize = Master.FileSize;
        //                objDocument.DocType = Master.FileType;
        //                objDocument.Remarks = Master.Remarks;
        //                objDocument.DocumnetTypeID = Master.DocumentTypeID;

        //                objDocument.UpdateBy = objcmnParam.loggeduser;
        //                objDocument.UpdateOn = DateTime.Now;
        //                objDocument.UpdatePc = HostService.GetIP();
        //                //***************************************************End Update Operation******************************************** 

        //                #region Update in Document Parent
        //                if (ParentDocumentList.Count > 0 && ParentDocumentList[0].ModelState != "NotFound")
        //                {
        //                    foreach (vmDocuments Pdoc in ParentDocumentList.Where(x => x.ModelState == "Inserted"))
        //                    {
        //                        CmnDocumentParent objDocParent = new CmnDocumentParent();

        //                        objDocParent.DocumentParentID = (long)Master.FileId;
        //                        objDocParent.DocumentID = Pdoc.FileId;

        //                        objDocParent.CompanyID = objcmnParam.loggedCompany;
        //                        objDocParent.CreateBy = objcmnParam.loggeduser;
        //                        objDocParent.CreateOn = DateTime.Now;
        //                        objDocParent.IsDeleted = false;
        //                        objDocParent.CreatePc = HostService.GetIP();

        //                        CmnDocParent.Add(objDocParent);
        //                    }

        //                    foreach (vmDocuments Updoc in ParentDocumentList.Where(x => x.ModelState == "Deleted"))
        //                    {
        //                        var UpDocParent = _ctxCmn.CmnDocumentParents.Where(x => x.DocumentID == Updoc.id).FirstOrDefault();

        //                        UpDocParent.CompanyID = objcmnParam.loggedCompany;
        //                        UpDocParent.CreateBy = objcmnParam.loggeduser;
        //                        UpDocParent.CreateOn = DateTime.Now;
        //                        UpDocParent.IsDeleted = true;
        //                        UpDocParent.CreatePc = HostService.GetIP();

        //                        UCmnDocParent.Add(UpDocParent);
        //                    }
        //                }
        //                #endregion Update in Document Parent

        //                if (CmnDocParent != null && CmnDocParent.Count != 0)
        //                {
        //                    _ctxCmn.CmnDocumentParents.AddRange(CmnDocParent.ToList());
        //                }

        //                _ctxCmn.SaveChanges();
        //                transaction.Complete();
        //                result = "1";
        //            }
        //            catch (Exception e)
        //            {
        //                e.ToString();
        //                result = "0";
        //            }
        //        }
        //        else
        //        {
        //            result = "0";
        //        }
        //    }
        //    return result;
        //    //**************************************************End Main Operation************************************************
        //}

        //public string DeleteDocumentList(vmCmnParameters objcmnParam)
        //{
        //    string result = "";
        //    using (var transaction = new TransactionScope())
        //    {
        //        _ctxCmn = new ERP_Entities();
        //        var DocItem = new List<CmnDocument>();

        //        //For Delete Document
        //        var DocAll = _ctxCmn.CmnDocuments.Where(x => x.DocumentID == objcmnParam.id && x.CompanyID == objcmnParam.loggedCompany);
        //        //-------------------END----------------------
        //        try
        //        {
        //            foreach (CmnDocument CD in DocAll.Where(d => d.DocumentID == objcmnParam.id))
        //            {
        //                CD.CompanyID = objcmnParam.loggedCompany;
        //                CD.DeleteBy = objcmnParam.loggeduser;
        //                CD.DeleteOn = DateTime.Now;
        //                CD.DeletePc = HostService.GetIP();
        //                CD.IsDeleted = true;

        //                DocItem.Add(CD);
        //            }

        //            _ctxCmn.SaveChanges();
        //            transaction.Complete();
        //            result = "1";
        //        }
        //        catch (Exception e)
        //        {
        //            result = "";
        //            e.ToString();
        //        }
        //    }
        //    return result;
        //}

    }
}

