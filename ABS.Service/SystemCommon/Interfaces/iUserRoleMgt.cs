using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.Service.SystemCommon.Interfaces
{
    public interface iUserRoleMgt
    {
        IEnumerable<vmUserRole> GetByPage(vmCmnParameters cparam);
        string SaveUpdate(vmUserRole urole);
        string Delete(vmCmnParameters cparam);
    }
}
