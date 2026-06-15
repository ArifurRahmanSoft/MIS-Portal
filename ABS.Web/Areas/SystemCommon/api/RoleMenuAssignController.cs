using CTGroup.Service.SystemCommon.Factories;
using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel.ViewModel.SystemCommon;
using CTGroup.Web.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CTGroup.Service.SystemCommon.Interfaces;

namespace CTGroup.Web.Areas.SystemCommon.api
{
    public class RoleMenuAssignController : ApiController
    {
        private iRoleMenuMgt _manager = null;
        public RoleMenuAssignController()
        {
            _manager = new RoleMenuMgt();
        }
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult getbypage(object[] data)
        {
            object resdata = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                resdata = _manager.GetByPage(objcmnParam);
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
        public IHttpActionResult saveupdate(object[] data)
        {
            object resdata = null;
            //vmUserRole urole = JsonConvert.DeserializeObject<vmUserRole>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                resdata = _manager.SaveUpdate(data[0].ToString());
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
        public IHttpActionResult saverole(object[] data)
        {
            object resdata = null;
            dynamic role = JsonConvert.DeserializeObject<dynamic>(data[0].ToString());
            string roleName = role.roleName.ToString();
            string loggedUserId = role.loggedUserId.ToString();
            int recordsTotal = 0;
            try
            {
                resdata = _manager.SaveRole(roleName, loggedUserId);
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
    }
}