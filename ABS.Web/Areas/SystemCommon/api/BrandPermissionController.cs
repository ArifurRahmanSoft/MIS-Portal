using ABS.Service.SystemCommon.Factories;
using CTGroup.Models;
using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel;
using CTGroup.Service.SystemCommon.Factories;
using CTGroup.Service.SystemCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace CTGroup.Web.Areas.SystemCommon.api
{
    [RoutePrefix("SystemCommon/api/BrandPermission")]
    public class BrandPermissionController : ApiController
    {
        private iCmnBrandPermissionMgt objService = null;

        public BrandPermissionController()
        {
            this.objService = new CmnBrandPermissionMgt();
        }

        [Route("GetBrandPermissionByParam/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{pModuleID:int}/{pUserID:int}")]
        [ResponseType(typeof(vmCmnBrandPermission))]
        [HttpGet]
        public IEnumerable<vmCmnBrandPermission> GetBrandPermissionByParam(string loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? pModuleID, string pUserID)
        {
            List<vmCmnBrandPermission> finalList = new List<vmCmnBrandPermission>();

            try
            {
                if (pUserID == "")
                {
                    finalList = objService.GetBrandPermissionByParams(loggedUser, pageNumber, pageSize, IsPaging, pModuleID, "0").ToList();
                }
                else finalList = objService.GetBrandPermissionByParamsUser(loggedUser, pageNumber, pageSize, IsPaging, pModuleID, pUserID).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            var session = HttpContext.Current.Session;

            return finalList.OrderBy(x => x.NATIONALTEAMOID).ThenBy(x => x.BRANDPERMISSIONID);
        }


        #region Save
        [ResponseType(typeof(vmCmnBrandPermission))]
        [HttpPost]
        public HttpResponseMessage SaveBrandPermission(List<vmCmnBrandPermission> Listmodel)
        {
            string result = "0";
            try
            {
                result = objService.SaveBrandPermission(Listmodel);
            }
            catch (Exception e)
            {
                e.ToString();
                result = "";
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        #endregion Save
    }
}
