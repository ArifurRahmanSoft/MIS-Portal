using CTGroup.Models;
using CTGroup.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.Service.MenuMgt
{
    public interface iMenuMgt
    {
        object GetSideMenu(int? companyID, int? loggedUser, int? ModuleID);
        object GetSideMenu(int? companyID, string loggedUser, int? ModuleID);
        List<vmBreadCrums> GetBreadCrums(int? companyID);
        object GetMenuPermission(vmApplicationTokenModel menu);
        List<vmCmnModule> GetTopMenu(int? companyID, int? loggedUser, int? ModuleID);
        
    }

}
