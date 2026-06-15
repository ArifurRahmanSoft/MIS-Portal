using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.Service.SystemCommon.Interfaces
{
    public interface iDataMappingMgt
    {
        DataTable GetByPage(vmDataMapping cparam);
        string SaveUpdate(vmDataMapping urole);
        string SaveUpdateLineProduct(List<vmDataMapping> dataList);
        string Delete(vmDataMapping cparam);
    }
}
