using CTGroup.Models.ViewModel.Sales;
using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel.ViewModel.Sales;
using System;
using System.Collections.Generic;


namespace CTGroup.Service.Sales.Interfaces
{
    public interface iAutoRiceSalesUpload
    {
        List<vmDistributorTarget> DuplicateDataUploadChecking(DateTime startDate, DateTime endDate);
        object UploadBulkDatas(List<vmAutoRiceSalesUpload> objvmAutoRiceSalesUpload, DateTime startDate, DateTime endDate, string LoggedUser);
        IEnumerable<vmDistributorTargetMaster> GetDistTargetDocUploadMaster(vmCmnParameters objcmnParam, out int recordsTotal);
        IEnumerable<vmCmnDocument> GetCmnDocument(vmCmnParameters objcmnParam, out int recordsTotal);
        int DeleteDistributorTargetRecord(vmCmnParameters objcmnParam, Int64 DocumentId);
    }
}
