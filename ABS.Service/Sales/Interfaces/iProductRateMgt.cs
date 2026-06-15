using CTGroup.Models.ViewModel.Sales;
using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel.ViewModel.Sales;
using CTGroup.OracleModel.ViewModel.SystemCommon;
using System.Collections.Generic;
namespace CTGroup.Service.Sales.Interfaces
{
    public interface iProductRateMgt
    {
        string SaveUpdateProductRate(vmProductRate itemMaster, List<vmProductRate> itemDetails, vmCmnParameters objcmnParam);
        IEnumerable<vmIncentiveFormulaSetupMaster> GetIncentiveFormulaSetupMaster(vmCmnParameters objcmnParam, out int recordsTotal);
        IEnumerable<vmBrandSKU> GetBrand(vmCmnParameters objcmnParam);
        IEnumerable<vmDistributor> GetSingleDistributor(vmCmnParameters objcmnParam);

    }
}
