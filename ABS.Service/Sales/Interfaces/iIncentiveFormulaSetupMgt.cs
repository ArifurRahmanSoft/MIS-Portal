using CTGroup.Models.ViewModel.Sales;
using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel.ViewModel.Sales;
using CTGroup.OracleModel.ViewModel.SystemCommon;
using System.Collections.Generic;
namespace CTGroup.Service.Sales.Interfaces
{
    public interface iIncentiveFormulaSetupMgt
    {
        List<vmDistributor> GetDivision(int? pageNumber, int? pageSize, int? IsPaging);
        List<vmDistributor> GetDistributor(int? pageNumber, int? pageSize, int? IsPaging);
        string SaveUpdateIncentiveFormulaSetup(vmIncentiveFormulaSetupMaster itemMaster);

        string SaveUpdateIncentiveRateSetup(vmIncentiveRateDistRatio incentiveRate, List<vmIncentiveRateDistRatio> listDistRatio);
        string SaveUpdateIncentiveAchievementRatio(vmIncentiveAchievementRatio incentiveAchievementRatio, List<vmIncentiveAchievementRatio> listIncentiveAchievementRatio);

        
        IEnumerable<vmIncentiveFormulaSetupMaster> GetIncentiveFormulaSetupMaster(vmCmnParameters objcmnParam, out int recordsTotal);
        IEnumerable<vmBrandSKU> GetBrand(vmCmnParameters objcmnParam);
        IEnumerable<vmIncentiveFormulaSetupMaster> GetIncentiveFormula(vmCmnParameters objcmnParam);
        IEnumerable<vmBrandSKU> GetBrandPopUp(vmCmnParameters objcmnParam, out int recordsTotal);
    }
}
