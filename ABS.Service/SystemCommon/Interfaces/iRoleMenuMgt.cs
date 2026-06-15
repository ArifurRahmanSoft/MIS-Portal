using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.Service.SystemCommon.Interfaces
{
    public interface iRoleMenuMgt
    {
        IEnumerable<vmRoleMenu> GetByPage(vmCmnParameters cparam);
        string SaveUpdate(string JsonData);
        string SaveRole(string roleName, string loggedUserId);
    }
}
