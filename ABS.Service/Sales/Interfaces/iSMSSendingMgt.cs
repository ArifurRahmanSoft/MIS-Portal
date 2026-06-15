using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel.ViewModel.Sales;
using System.Collections.Generic;

namespace CTGroup.Service.Sales.Interfaces
{
    public interface iSMSSendingMgt
    {
        string SendSingleSMS(vmSMSSending model, string messageResponse);
        IEnumerable<vmSMSSending> GetSentMessageDetail(vmCmnParameters objcmnParam, out int recordsTotal);
    }
}
