using CTGroup.Models;
using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.Service.SystemCommon.Interfaces
{
    public interface iCmnModulePermissionMgt
    {
        int SaveModulePermission(T_CMNMODULEPERMISSION model);
        //List<vmModulePermission> GetAllModulePermission(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging);
        int DeletePermission(int? ModuleID);
    }
}
