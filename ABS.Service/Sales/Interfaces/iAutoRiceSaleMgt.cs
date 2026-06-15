using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel.ViewModel.Sales;
using System.Collections.Generic;

namespace CTGroup.Service.Sales.Interfaces
{
    public interface iAutoRiceSaleMgt
    {
        string SaveRentCollector(vmAutoRiceSale model);
        IEnumerable<vmAutoRiceSale> GetRentCollector(vmCmnParameters objcmnParam, out int recordsTotal);
        // int DeleteUser(int? id, int? CompanyID, int? LoggedUser);       
    }
}
