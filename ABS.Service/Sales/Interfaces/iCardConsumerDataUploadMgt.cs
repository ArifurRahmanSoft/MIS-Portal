using CTGroup.Models.ViewModel.Sales;
using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel.ViewModel.Sales;
using System;
using System.Collections.Generic;
using System.Data;

namespace CTGroup.Service.Sales.Interfaces
{
    public interface iCardConsumerDataUploadMgt
    {
        IEnumerable<vmCardConsumerDataUpload> CardConsumerReportData(vmCmnParameters objcmnParam);
        //IEnumerable<vmCardConsumerMaster> GetDistTargetDocUploadMasterBackup(vmCmnParameters objcmnParam, out int recordsTotal);
        DataTable GetDistTargetDocUploadMaster(vmCmnParameters objcmnParam, out int recordsTotal);
        int DeleteDistributorTargetRecord(vmCmnParameters objcmnParam, Int64 DocumentId);
        string UploadDocumentsSave(List<vmCardConsumerDataUpload> itemDetails);

    }
}
