using CTGroup.Models;
using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel;
using CTGroup.Service.SystemCommon.Factories;
using CTGroup.Service.SystemCommon.Interfaces;
using CTGroup.Web.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace CTGroup.Web.SystemCommon.api
{
    [RoutePrefix("SystemCommon/api/Menu")]
    public class MenuController : ApiController
    {
        private iCmnMenuMgt objMenuService = null;
        public MenuController()
        {
            this.objMenuService = new CmnMenuMgt();
        }

        // GET: GetCustomers/0/10/0
        [Route("GetMenues/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(T_CMNMENU))]
        [HttpGet]
        public IEnumerable<vmCmnMenu> GetMenues(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<vmCmnMenu> objListMenues = null;
            try
            {
                objListMenues = objMenuService.GetMenues(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListMenues;
        }
        
        [HttpPost]
        public IHttpActionResult getbypage(object[] data)
        {
            DataTable resdata = new DataTable();
            vmCmnParameters cparam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                resdata = objMenuService.GetMenueByPage(cparam);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                resdata
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult getbyid(object[] data)
        {
            DataTable resdata = new DataTable();
            vmCmnParameters cparam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                resdata = objMenuService.GetMenuByID(cparam);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                resdata
            });
        }

        //// GET: GetCustomerByID/1
        [Route("GetMenuByID/{id:int}")]
        [ResponseType(typeof(T_CMNMENU))]
        [HttpGet]
        public vmCmnMenu GetMenuByID(int? id)
        {
            vmCmnMenu objMenu = null;
            try
            {
                objMenu = objMenuService.GetMenuByID(id);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objMenu;
        }

        //// POST SaveCustomer
        [ResponseType(typeof(T_CMNMENU))]
        [HttpPost]
        public HttpResponseMessage SaveMenu(T_CMNMENU model)
        {
            int result = 0;
            try
            {
                //By Default Company Status Will be Active(1)
                model.STATUSID = 1;
                result = objMenuService.SaveMenu(model);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        //// PUT UpdateCustomer/1
        [ResponseType(typeof(T_CMNMENU))]
        [HttpPut]
        public HttpResponseMessage UpdateMenu(T_CMNMENU model)
        {
            int result = 0;
            try
            {
                result = objMenuService.UpdateMenu(model);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        //// DELETE DeleteCustomer/1
        [Route("DeleteMenu/{id:int}")]
        [HttpDelete]
        public HttpResponseMessage DeleteMenu(int? id)
        {
            int result = 0;
            try
            {
                result = objMenuService.DeleteMenu(id);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
