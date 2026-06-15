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
    public interface iSOWiseDistTargetDocumentUploadMgt
    {
        List<vmDistributorTargetSOWise> DuplicateDataUploadChecking(DateTime startDate, DateTime endDate);
       // bool UploadBulkData(List<vmDistributorTargetSOWise> objvmPIMasterWithOutPaging, DateTime startDate, DateTime endDate, string LoggedUser);
        IEnumerable<vmDistributorTargetSOWiseMaster> GetDistTargetDocUploadMaster(vmCmnParameters objcmnParam, out int recordsTotal);
        string DeleteDistributorTargetRecord(vmCmnParameters objcmnParam, Int64 DocumentId);
        string UploadDocumentsSaveSOWiseDistTarget(List<vmDistributorTargetSOWise> itemDetails, DateTime startDate, DateTime endDate, string LoggedUser);

        
    }
}
