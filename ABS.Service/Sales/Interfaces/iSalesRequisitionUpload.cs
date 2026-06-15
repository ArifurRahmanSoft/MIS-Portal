using CTGroup.Models.ViewModel.Sales;
using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel.ViewModel.Sales;
using System;
using System.Collections.Generic;


namespace CTGroup.Service.Sales.Interfaces
{
    public interface iSalesRequisitionUpload
    {
        List<vmDistributorTarget> DuplicateDataUploadChecking(DateTime startDate, DateTime endDate);
        object UploadBulkDatas(List<vmSalesRequisitionUpload> objvmAutoRiceSalesUpload, DateTime startDate, DateTime endDate, string LoggedUser);
        IEnumerable<vmCmnDocument> GetCmnDocument(vmCmnParameters objcmnParam, out int recordsTotal);
    }
}
