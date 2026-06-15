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
using CTGroup.Service.Costing.Factories;

namespace CTGroup.Web.Areas.COSTING.api
{
    [RoutePrefix("COSTING/api/ProductConsting")]
    public class ProductConstingController : ApiController
    {
        private ProductCostingMgt objMVService = null;

        public ProductConstingController()
        {
            objMVService = new ProductCostingMgt();
        }

        public IEnumerable<vmProductCostingCmn> getAllCompanyList()
        {
            IEnumerable<vmProductCostingCmn> _list = null;

            try
            {
                _list = objMVService.getAllCompanyList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _list;
        }

        [HttpPost]
        public IHttpActionResult LoadAllProduct(object[] data)
        {
            List<vmProductCostingCmn> listProduct = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                listProduct = objMVService.LoadAllProduct(objcmnParam).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                listProduct
            });
        }

        [HttpPost]
        public HttpResponseMessage SaveProductCostingRate(object[] data)
        {
            string result = "";
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());

                vmSprodCostRate LSS = JsonConvert.DeserializeObject<vmSprodCostRate>(data[1].ToString());

                if (ModelState.IsValid)
                {
                    result = objMVService.SaveProductCostingRate(objcmnParam, LSS);
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
