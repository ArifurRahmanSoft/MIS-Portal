using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel.ViewModel.Sales;
using System.Collections.Generic;

namespace CTGroup.Service.Sales.Interfaces
{
    public interface iDailyClosingMgt
    {
        string SaveDailyClosing(vmDailyClosing model);
        IEnumerable<vmDailyClosing> GetDailyClosing(vmCmnParameters objcmnParam, out int recordsTotal);
        // int DeleteUser(int? id, int? CompanyID, int? LoggedUser);       
    }
}
