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
using CTGroup.OracleModel.ViewModel.Sales;

namespace CTGroup.Web.Areas.Sales.api
{
    [RoutePrefix("Sales/api/ProductRate")]
    public class ProductRateController : ApiController
    {
        private iProductRateMgt objPIService = null;

        public ProductRateController()
        {
            objPIService = new ProductRateMgt();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetIncentiveFormulaSetupMaster(object[] data)
        {
            IEnumerable<vmIncentiveFormulaSetupMaster> objVmPIMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objVmPIMaster = objPIService.GetIncentiveFormulaSetupMaster(objcmnParam, out recordsTotal);
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
            try
            {
                objBrandList = objPIService.GetBrand(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objBrandList
            });
        }


        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetSingleDistributor(object[] data)
        {
            IEnumerable<vmDistributor> objDistributor = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objDistributor = objPIService.GetSingleDistributor(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objDistributor
            });
        }

        [HttpPost]
        public HttpResponseMessage SaveUpdateProductRate(object[] data)
        {
            vmProductRate productRate = JsonConvert.DeserializeObject<vmProductRate>(data[0].ToString());
            List<vmProductRate> productRateDetails = JsonConvert.DeserializeObject<List<vmProductRate>>(data[1].ToString());
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[2].ToString());
            string result = "";
            try
            {
                if (ModelState.IsValid && productRate != null)
                {
                    result = objPIService.SaveUpdateProductRate(productRate, productRateDetails, objcmnParam);
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
    }
}
