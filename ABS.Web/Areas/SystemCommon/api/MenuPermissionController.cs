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
    [RoutePrefix("SystemCommon/api/MenuPermission")]
    public class MenuPermissionController : ApiController
    {
        private iCmnMenuPermissionMgt objService = null;

        public MenuPermissionController()
        {
            this.objService = new CmnMenuPermissionMgt();
        }

        //[Route("GetAllModulePermission/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        //[HttpGet]
        //public List<vmModulePermission> GetAllModulePermission(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        //{
        //    List<vmModulePermission> objlist = new List<vmModulePermission>();
        //    try
        //    {
        //        objlist = objService.GetAllModulePermission(companyID, loggedUser, pageNumber, pageSize, IsPaging).ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return objlist;
        //}
          
        //[Route("DeletePermission/{id:int}")]
        //[HttpDelete]
        //public HttpResponseMessage DeletePermission(int id)
        //{
        //    int result = 0;
        //    try
        //    {
        //        result = objService.DeletePermission(id);
        //        //result = objItemSizeService.DeleteItemSize(id);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //        result = -0;
        //    }

        //    return Request.CreateResponse(HttpStatusCode.OK, result);
        //}
       
        [Route("GetMenuPermissionByParam/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{pModuleID:int}/{pUserID:int}")]
        [ResponseType(typeof(vmCmnMenuPermission))]
        [HttpGet]
        public IEnumerable<vmCmnMenuPermission> GetMenuPermissionByParam(string loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? pModuleID, string pUserID)
        {
            List<vmCmnMenuPermission> finalList = new List<vmCmnMenuPermission>();

            try
            {
                if (pUserID == "")
                {
                    finalList = objService.GetMenuPermissionByParams(loggedUser, pageNumber, pageSize, IsPaging, pModuleID, "0").ToList();
                }
                else finalList = objService.GetMenuPermissionByParamsUser(loggedUser, pageNumber, pageSize, IsPaging, pModuleID, pUserID).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            var session = HttpContext.Current.Session;

            return finalList.OrderBy(x => x.ModuleID).ThenByDescending(x => x.Sequence).ThenBy(x => x.MenuID).ThenBy(x => x.MenuPermissionID);
        }


        #region Save
        [ResponseType(typeof(vmCmnMenuPermission))]
        [HttpPost]
        public HttpResponseMessage SaveMenuPermission(List<vmCmnMenuPermission> Listmodel)
        {
            string result = "0";
            try
            {
                result = objService.SaveMenuPermission(Listmodel);
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
