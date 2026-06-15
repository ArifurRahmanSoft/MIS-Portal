using CTGroup.Service.SystemCommon.Factories;
using CTGroup.Service.SystemCommon.Interfaces;
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

namespace CTGroup.Web.Areas.SystemCommon.api
{
    public class DataMappingController : ApiController
    {
        private iDataMappingMgt _manager = null;
        public DataMappingController()
        {
            _manager = new DataMappingMgt();
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
            vmDataMapping MappingData = JsonConvert.DeserializeObject<vmDataMapping>(data[0].ToString());            
            try
            {
                resdata = _manager.GetByPage(MappingData);
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
            vmDataMapping MappingData = JsonConvert.DeserializeObject<vmDataMapping>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                resdata = _manager.SaveUpdate(MappingData);
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
        public IHttpActionResult saveupdatelineproduct(object[] data)
        {
            object resdata = null;
            List<vmDataMapping> MappingDataList = JsonConvert.DeserializeObject<List<vmDataMapping>>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                resdata = _manager.SaveUpdateLineProduct(MappingDataList);
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
        public IHttpActionResult delete(object[] data)
        {
            object resdata = null;
            vmDataMapping MappingData = JsonConvert.DeserializeObject<vmDataMapping>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                resdata = _manager.Delete(MappingData);
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