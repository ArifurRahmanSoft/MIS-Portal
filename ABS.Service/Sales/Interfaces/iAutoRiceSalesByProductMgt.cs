using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel.ViewModel.Sales;
using System.Collections.Generic;

namespace CTGroup.Service.Sales.Interfaces
{
    public interface iAutoRiceSalesByProductMgt
    {
        string SaveAutoRiceSaleByProduct(vmAutoRiceSaleByProduct model);
        IEnumerable<vmAutoRiceSaleByProduct> GetAutoRiceSalesByProduct(vmCmnParameters objcmnParam, out int recordsTotal);
    }
}
