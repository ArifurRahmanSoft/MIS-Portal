using CTGroup.Models;
using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.Service.SystemCommon.Interfaces
{
    public interface iCmnMenuMgt
    {
        IEnumerable<vmCmnMenu> GetMenues(int? pageNumber, int? pageSize, int? IsPaging);
        DataTable GetMenueByPage(vmCmnParameters cparam);
        DataTable GetMenuByID(vmCmnParameters cparam);
        vmCmnMenu GetMenuByID(int? id);
        int SaveMenu(T_CMNMENU model);
        int UpdateMenu(T_CMNMENU model);
        int DeleteMenu(int? MenuID);
        List<T_CMNMODULE> GetModuleOnDemand();

    }
}
