using CTGroup.Models.ViewModel.Sales;
using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel;
using CTGroup.OracleModel.ViewModel.Sales;
using CTGroup.OracleModel.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CTGroup.Service.Sales.Interfaces
{
    public interface iPrimarySalesTargetUploadMgt
    {
        List<vmDistributorTarget> DuplicateDataUploadChecking(DateTime startDate, DateTime endDate, string fileName);
        bool UploadBulkData(List<vmDistributorTarget> bulkData, DateTime startDate, DateTime endDate, string LoggedUser, string fileName);
        IEnumerable<vmDistributorTargetMaster> GetDistTargetDocUploadMaster(vmCmnParameters objcmnParam, out int recordsTotal);
        int DeleteDistributorTargetRecord(vmCmnParameters objcmnParam, Int64 DocumentId);
    }
}
