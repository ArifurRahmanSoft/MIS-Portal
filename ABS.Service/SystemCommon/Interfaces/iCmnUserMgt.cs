using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGroup.Service.SystemCommon.Interfaces
{
    public interface iCmnUserMgt
    {
        //vmAuthenticatedUser Get_CmnUserAuthentication(vmLoginUser model);
        List<vmAuthenticatedUser> Get_CmnUserAuthentication(vmLoginUser model);

        // IEnumerable<vmUserGroup> GetUserGroup(vmCmnParameters objcmnParam, out int recordsTotal);
        // IEnumerable<vmUserType> GetUserType(vmCmnParameters objcmnParam, out int recordsTotal);
        IEnumerable<vmEmployee> GetUser(vmCmnParameters objcmnParam, out long recordsTotal);
        string getCurrentPassword(int companyID, string loggedUser);
        int ChangePassword(vmLoginUser model);

        // string getCurrentPassword(int companyID, int loggedUser);

        //int ChangePassword(Models.T_CMNUSERAUTHENTICATION model);


        //int CheckLoginID(T_CMNUSERAUTHENTICATION Atuuser);
    }
}
