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

namespace CTGroup.Web.Areas.Sales.api
{
    [RoutePrefix("Sales/api/IncentiveCalculation")]
    public class IncentiveCalculationController : ApiController
    {
        private iIncentiveCalculationMgt objIncenCalService = null;

        public IncentiveCalculationController()
        {
            objIncenCalService = new IncentiveCalculationMgt();
        }

        [Route("GetDivision/{pageNumber:int}/{pageSize:int}/{IsPaging:int}"), HttpGet, ResponseType(typeof(vmDistributor)), BasicAuthorization]
        public List<vmDistributor> GetDivision(int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<vmDistributor> objBrandList = null;
            try
            {
                objBrandList = objIncenCalService.GetDivision(pageNumber, pageSize, IsPaging);
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
                objBrandList = objIncenCalService.GetDistributor(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objBrandList;
        }
       
        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetIncentiveCalculationMaster(object[] data)
        {
            IEnumerable<vmIncentiveCalculationMaster> objVmPIMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objVmPIMaster = objIncenCalService.GetIncentiveCalculationMaster(objcmnParam, out recordsTotal);
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
        public IHttpActionResult CalculatePrimarySale(object[] data)
        {
            IEnumerable<vmIncentiveCalculationMaster> objVmPIMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objVmPIMaster = objIncenCalService.CalculatePrimarySale(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objVmPIMaster
            });
        }


        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetTargetPrimarySecondarySale(object[] data)
        {
            IEnumerable<vmIncentiveCalculationMaster> objVmPIMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objVmPIMaster = objIncenCalService.GetTargetPrimarySecondarySale(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objVmPIMaster
            });
        }



        [HttpPost, BasicAuthorization]
        public IHttpActionResult IncentiveCalculation(object[] data)
        {
            IEnumerable<vmIncentiveCalculationMaster> objVmPIMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objVmPIMaster = objIncenCalService.IncentiveCalculation(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objVmPIMaster
            });
        }
    }
}
