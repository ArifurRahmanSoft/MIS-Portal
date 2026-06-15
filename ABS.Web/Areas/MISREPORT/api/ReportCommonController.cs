using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using CTGroup.Models;

using CTGroup.Service.Sales.Interfaces;
using CTGroup.Service.Sales.Factories;
using Newtonsoft.Json;
using CTGroup.Models.ViewModel.Sales;
using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.Web.Attributes;
using CTGroup.OracleModel.ViewModel.SystemCommon;

namespace CTGroup.Web.Areas.MISREPORT.api
{
    [RoutePrefix("MISREPORT/api/ReportCommon")]
    public class ReportCommonController : ApiController
    {
        private iDistributorTargetMgt objPIService = null;

        public ReportCommonController()
        {
            objPIService = new DistributorTargetMgt();
        }

        [Route("GetDivision/{pageNumber:int}/{pageSize:int}/{IsPaging:int}"), HttpGet, ResponseType(typeof(vmDistributor)), BasicAuthorization]
        public List<vmDistributor> GetDivision(int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<vmDistributor> objBrandList = null;
            try
            {
                objBrandList = objPIService.GetDivision(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objBrandList;
        }

        [Route("GetDistributor/{pageNumber:int}/{pageSize:int}/{IsPaging:int}"), HttpGet, ResponseType(typeof(vmDistributor)), BasicAuthorization]
        public List<vmDistributor> GetDistributor(int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<vmDistributor> objBrandList = null;
            try
            {
                objBrandList = objPIService.GetDistributor(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objBrandList;
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetDistributorTargetMaster(object[] data)
        {
            IEnumerable<vmDistributorTargetMaster> objVmPIMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objVmPIMaster = objPIService.GetDistributorTargetMaster(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objVmPIMaster
            });
        }
        
        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetBrand(object[] data)
        {
            IEnumerable<vmBrandSKU> objBrandList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objBrandList = objPIService.GetBrand(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objBrandList
            });
        }
    }
}
