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

namespace CTGroup.Web.Areas.SPECIALREPORT.api
{
    [RoutePrefix("SPECIALREPORT/api/ProductDetailsPerLCBased")]
    public class ProductDetailsPerLCBasedController : ApiController
    {
        private ProductDetailsPerLCBasedMgt objMVService = null;

        public ProductDetailsPerLCBasedController()
        {
            objMVService = new ProductDetailsPerLCBasedMgt();
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
        public HttpResponseMessage SaveOpeningStock(object[] data)
        {
            string result = "";
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());

                vmOpeningStock openStock = JsonConvert.DeserializeObject<vmOpeningStock>(data[1].ToString());

                if (ModelState.IsValid)
                {
                    result = objMVService.SaveOpeningStock(objcmnParam, openStock);
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



   


        [HttpPost]
        public IHttpActionResult getOpenStockList(object[] data)
        {
            long recordsTotal = 0;
            List<vmOpeningStock> listOpenStock = null;
           vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
           try
           {
             listOpenStock = objMVService.getOpenStockList(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                listOpenStock
            });
        }



        public HttpResponseMessage SaveLCReceive(object[] data)
        {
            string result = "";
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());

                vmSrLcReceive lcReceive = JsonConvert.DeserializeObject<vmSrLcReceive>(data[1].ToString());

                if (ModelState.IsValid)
                {
                    result = objMVService.SaveLCReceive(objcmnParam, lcReceive);
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
