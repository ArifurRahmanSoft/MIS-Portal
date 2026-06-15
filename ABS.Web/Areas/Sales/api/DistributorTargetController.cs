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
    [RoutePrefix("Sales/api/DistributorTarget")]
    public class DistributorTargetController : ApiController
    {
        private iDistributorTargetMgt objPIService = null;

        public DistributorTargetController()
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

        // [HttpPost, BasicAuthorization]
        //public IEnumerable<vmDistributorTargetDetail> GetDistributorTargetDetail(object activePI)
        //{
        //    IEnumerable<vmDistributorTargetDetail> objPIItemDetails = null;
        //    try
        //    {
        //        Int64 Id = (Int64)activePI;
        //        objPIItemDetails = objPIService.GetDistributorTargetDetail(Id);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }

        //    return objPIItemDetails;
        //}

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

        [HttpPost, BasicAuthorization]
        public HttpResponseMessage SaveUpdateDistributorTarget(object[] data)
        {
            vmDistributorTargetMaster itemMaster = JsonConvert.DeserializeObject<vmDistributorTargetMaster>(data[0].ToString());
            List<vmDistributorTargetDetail> itemDetails = JsonConvert.DeserializeObject<List<vmDistributorTargetDetail>>(data[1].ToString());
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[2].ToString());
            //SalDistributorTargetMaster obj = new SalDistributorTargetMaster();
            string result = "";
            try
            {
                if (ModelState.IsValid && itemMaster != null && itemDetails.Count > 0)
                {
                    result = objPIService.SaveUpdateDistributorTarget(itemMaster, itemDetails, objcmnParam);
                }
                else
                {
                    result = "";
                }
            }
            catch (Exception e)
            {
                e.ToString();
                result = "";
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        //[HttpPost, BasicAuthorization]
        //public HttpResponseMessage DeleteUpdateDistributorTargetMasterDetails(object[] data)
        //{
        //    string result = string.Empty;
        //    vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
        //    try
        //    {
        //        result = objPIService.DeleteMasterDetail(objcmnParam);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //        result = "";
        //    }
        //    return Request.CreateResponse(HttpStatusCode.OK, result);
        //}
    }
}
