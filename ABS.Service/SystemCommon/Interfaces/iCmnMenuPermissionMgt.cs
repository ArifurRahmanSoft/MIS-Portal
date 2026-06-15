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
    public interface iCmnMenuPermissionMgt
    {
        string SaveMenuPermission(List<vmCmnMenuPermission> listModel);
        IEnumerable<vmCmnMenuPermission> GetMenuPermissionByParams(string loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? pModuleID, string pUserID);
        IEnumerable<vmCmnMenuPermission> GetMenuPermissionByParamsUser(string loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? pModuleID, string pUserID);
    }
}
