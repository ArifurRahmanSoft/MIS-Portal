using CTGroup.Models.ViewModel.Sales;
using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel.ViewModel.Sales;
using CTGroup.OracleModel.ViewModel.SystemCommon;
using System.Collections.Generic;
namespace CTGroup.Service.Sales.Interfaces
{
    public interface iDistributorTargetMgt
    {
        List<vmDistributor> GetDivision(int? pageNumber, int? pageSize, int? IsPaging);
        List<vmDistributor> GetDistributor(int? pageNumber, int? pageSize, int? IsPaging);
        string SaveUpdateDistributorTarget(vmDistributorTargetMaster itemMaster, List<vmDistributorTargetDetail> itemDetails, vmCmnParameters objcmnParam);
    
  
        IEnumerable<vmDistributorTargetMaster> GetDistributorTargetMaster(vmCmnParameters objcmnParam, out int recordsTotal);
        IEnumerable<vmBrandSKU> GetBrand(vmCmnParameters objcmnParam, out int recordsTotal);
        string GenOrdDlvCountCustom(vmCmnParameters cparam);
        string GetLastGenDate();
    }
}
