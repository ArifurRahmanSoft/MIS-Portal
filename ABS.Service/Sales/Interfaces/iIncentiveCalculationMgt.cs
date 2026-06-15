using CTGroup.Models.ViewModel.Sales;
using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel.ViewModel.Sales;
using CTGroup.OracleModel.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
namespace CTGroup.Service.Sales.Interfaces
{
    public interface iIncentiveCalculationMgt
    {
        List<vmDistributor> GetDivision(int? pageNumber, int? pageSize, int? IsPaging);
        List<vmDistributor> GetDistributor(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<vmIncentiveCalculationMaster> GetIncentiveCalculationMaster(vmCmnParameters objcmnParam, out int recordsTotal);
        IEnumerable<vmIncentiveCalculationMaster> CalculatePrimarySale(vmCmnParameters objcmnParam);
        IEnumerable<vmIncentiveCalculationMaster> GetTargetPrimarySecondarySale(vmCmnParameters objcmnParam);
        IEnumerable<vmIncentiveCalculationMaster> IncentiveCalculation(vmCmnParameters objcmnParam);
    }
}
