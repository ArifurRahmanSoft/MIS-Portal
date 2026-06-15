using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel.ViewModel.Sales;
using System.Collections.Generic;

namespace CTGroup.Service.Sales.Interfaces
{
    public interface iTentativeSaleMgt
    {
        string SaveTentativeSale(vmTentativeSale model);
        IEnumerable<vmTentativeSale> GetTentativeSale(vmCmnParameters objcmnParam, out int recordsTotal);    
    }
}
