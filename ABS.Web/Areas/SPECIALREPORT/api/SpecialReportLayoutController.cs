using CTGroup.Models;
using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.Service.MenuMgt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using CTGroup.Utility;
using System.Threading.Tasks;
using CTGroup.Web.Attributes;
using Newtonsoft.Json;

namespace CTGroup.Web.Areas.SPECIALREPORT.api
{
    [RoutePrefix("SPECIALREPORT/api/SpecialReportLayout")]
    public class SpecialReportLayoutController : ApiController
    {
        private iMenuMgt objService = null;

        public SpecialReportLayoutController()
        {
            this.objService = new MenuMgt();
        }

        [Route("GetBreadCrums/{menuID:int}")]
        [HttpGet]
        public List<vmBreadCrums> GetBreadCrums(int? menuID)
        {
            var type = objService.GetBreadCrums(menuID);
            return type;
        }

        //[Route("GetSideMenu/{companyID:int}/{loggedUser:int}/{ModuleID:int}")]
        //[HttpGet]
        //public object GetSideMenu(int? companyID, int? loggedUser, int? ModuleID)
        //{
        //    objService = new MenuMgt();
        //    var type = objService.GetSideMenu(companyID, loggedUser, ModuleID);


        //    return type;
        //}    

        [Route("GetSideMenu/{companyID:int}/{loggedUser}/{ModuleID:int}")]
        [HttpGet]
        public object GetSideMenu(int? companyID, string loggedUser, int? ModuleID)
        {
            objService = new MenuMgt();
            var type = objService.GetSideMenu(companyID, loggedUser, ModuleID);


            return type;
        }

        [Route("GetTopMenu/{companyID:int}/{loggedUser:int}/{ModuleID:int}")]
        [HttpGet]
        public List<vmCmnModule> GetTopMenu(int? companyID, int? loggedUser, int? ModuleID)
        {
            var type = objService.GetTopMenu(companyID, loggedUser, ModuleID);
            return type;
        }

        [HttpPost]
        public object GetMenuPermission(object[] data)
        {
            vmApplicationTokenModel menu = JsonConvert.DeserializeObject<vmApplicationTokenModel>(data[0].ToString());
            string menuPath = menu.MenuPath;
            objService = new MenuMgt();
            var menuist = objService.GetMenuPermission(menu);
            return menuist;
        }


    }
}