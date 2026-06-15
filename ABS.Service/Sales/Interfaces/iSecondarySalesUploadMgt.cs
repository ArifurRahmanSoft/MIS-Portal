using CTGroup.Models.ViewModel.Sales;
using CTGroup.OracleModel;
using CTGroup.OracleModel.ViewModel.Sales;
using CTGroup.OracleModel.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CTGroup.Service.Sales.Interfaces
{
    public interface iSecondarySalesUploadMgt
    {
        bool UploadBulkData(List<vmDistributorTarget> objvmPIMasterWithOutPaging, DateTime startDate, DateTime endDate);

        //List<vmDocuments> GeDocumentTypeList(vmCmnParameters objcmnParam);
        //List<vmDocuments> GetParentDocList(vmCmnParameters objcmnParam);
        //vmDocuments GetDocumentTypeBaseData(vmCmnParameters objcmnParam);      
        //string SaveUpdateDocumentList(vmDocuments Master, List<vmDocuments> ParentDocumentList, vmCmnParameters objcmnParam);
        ////string UpdateDocumentList(vmDocuments Master, List<vmDocuments> ParentDocumentList, vmCmnParameters objcmnParam);
        //List<vmDocuments> DocumentMasterList(vmCmnParameters objcmnParam, out int recordsTotal);        
        //IEnumerable<vmDocuments> GetDocumentListByID(vmCmnParameters objcmnParam);
        //string DeleteDocumentList(vmCmnParameters objcmnParam);
        //int DuplicateCheckDocName(vmCmnParameters objcmnParam);

    }
}
