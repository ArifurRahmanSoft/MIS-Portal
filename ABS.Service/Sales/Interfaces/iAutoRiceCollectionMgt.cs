using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel.ViewModel.Sales;
using System.Collections.Generic;

namespace CTGroup.Service.Sales.Interfaces
{
    public interface iAutoRiceCollectionMgt
    {
        string SaveAutoRiceCollection(vmAutoRiceCollection model);
        IEnumerable<vmAutoRiceCollection> GetAutoRiceCollection(vmCmnParameters objcmnParam, out int recordsTotal);
        // int DeleteUser(int? id, int? CompanyID, int? LoggedUser);       
    }
}
