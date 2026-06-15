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
    [RoutePrefix("Sales/api/ProductSetup")]
    public class ProductSetupController : ApiController
    {
        private iProductSetupMgt objPIService = null;

         public ProductSetupController()
        {
           objPIService = new ProductSetupMgt();
            
        }



        [HttpPost]
        public HttpResponseMessage SaveUpdateProductDetails([FromBody]  object[] data)
        {
            string JsonData_Mstr = data[0].ToString();
            string userId =  data[1].ToString();
          
            string result = "";
            try
            {
                if (ModelState.IsValid && userId != null)
                {
                    result = objPIService.SaveUpdateProductDetails(JsonData_Mstr, userId);
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
        public HttpResponseMessage DeleteProduct([FromBody] object[] data)
         {
             string ProductId = data[0].ToString();
            string userId = data[1].ToString();

            string result = "";
            try
            {
                if (ModelState.IsValid && userId != null)
                {
                    result = objPIService.DeleteProduct(ProductId, userId);
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







        [HttpGet, BasicAuthorization]
    public IHttpActionResult GetProductList(string parameter, string parameter1)
    {
        object resdata = null;
        vmCmnParameters objcmnParam = new vmCmnParameters
        {
            parameter = parameter,
            parameter1 = parameter1
        };
        try
        {
            resdata = objPIService.GetProductList(objcmnParam);
        }
        catch (Exception e)
        {
            e.ToString();
            resdata = "";
        }
        return Json(new { resdata });
    }


    [HttpGet, BasicAuthorization]
    public IHttpActionResult GetProductDetail(string parameter)
    {
        object resdata = null;
        vmCmnParameters objcmnParam = new vmCmnParameters
        {
            parameter = parameter,

        };
        try
        {
            resdata = objPIService.GetProductDetail(parameter);
        }
        catch (Exception e)
        {
            e.ToString();
            resdata = "";
        }
        return Json(new { resdata });
    }


        //GET PRODUCT DETAIL BY ID
        [HttpGet, BasicAuthorization]
        public IHttpActionResult GetProductDetailById(string parameter)
        {
            object resdata = null;
            vmCmnParameters objcmnParam = new vmCmnParameters
            {
                parameter = parameter,

            };
            try
            {
                resdata = objPIService.GetProductDetailById(parameter);
            }
            catch (Exception e)
            {
                e.ToString();
                resdata = "";
            }
            return Json(new { resdata });
        }



        [HttpGet, BasicAuthorization]
        public IHttpActionResult GetProductListByPage()
        {
            object resdata = null;
            try
            {
                resdata = objPIService.GetProductListByPage();
            }
            catch (Exception e)
            {
                e.ToString();
                resdata = "";
            }
            return Json(new { resdata });
        }



        [HttpGet, BasicAuthorization]
    public IHttpActionResult GetProductType()
    {
        object resdata = null;
        try
        {
            resdata = objPIService.GetProductType();
        }
        catch (Exception e)
        {
            e.ToString();
            resdata = "";
        }
        return Json(new { resdata });
    }


    [HttpGet, BasicAuthorization]
    public IHttpActionResult GetSconType()
    {
        object resdata = null;
        try
        {
            resdata = objPIService.GetSconType();
        }
        catch (Exception e)
        {
            e.ToString();
            resdata = "";
        }
        return Json(new { resdata });
    }



    [HttpGet, BasicAuthorization]
    public IHttpActionResult GetSpcat()
    {
        object resdata = null;
        try
        {
            resdata = objPIService.GetSpcat();
        }
        catch (Exception e)
        {
            e.ToString();
            resdata = "";
        }
        return Json(new { resdata });
    }


             
              [HttpGet, BasicAuthorization]
    public IHttpActionResult GetProductSerial()
        {
            object resdata = null;
            try
            {
                resdata = objPIService.GetProductSerial();
            }
            catch (Exception e)
            {
                e.ToString();
                resdata = "";
            }
            return Json(new { resdata });
        }


        [HttpGet, BasicAuthorization]
        public IHttpActionResult GetProductSize()
        {
            object resdata = null;
            try
            {
                resdata = objPIService.GetProductSize();
            }
            catch (Exception e)
            {
                e.ToString();
                resdata = "";
            }
            return Json(new { resdata });
        }


        [HttpGet, BasicAuthorization]
        public IHttpActionResult GetProductGroup()
        {
            object resdata = null;
            try
            {
                resdata = objPIService.GetProductGroup();
            }
            catch (Exception e)
            {
                e.ToString();
                resdata = "";
            }
            return Json(new { resdata });
        }


        [HttpGet, BasicAuthorization]
        public IHttpActionResult GetDoGroup()
        {
            object resdata = null;
            try
            {
                resdata = objPIService.GetDoGroup();
            }
            catch (Exception e)
            {
                e.ToString();
                resdata = "";
            }
            return Json(new { resdata });
        }


        [HttpGet, BasicAuthorization]
        public IHttpActionResult GetSBrand()
        {
            object resdata = null;
            try
            {
                resdata = objPIService.GetSBrand();
            }
            catch (Exception e)
            {
                e.ToString();
                resdata = "";
            }
            return Json(new { resdata });
        }


        [HttpGet, BasicAuthorization]
        public IHttpActionResult GetSmunt()
        {
            object resdata = null;
            try
            {
                resdata = objPIService.GetSmunt();
            }
            catch (Exception e)
            {
                e.ToString();
                resdata = "";
            }
            return Json(new { resdata });
        }




        [HttpPost]
        public HttpResponseMessage SaveUpdateLitem([FromBody] object[] data)
         {
            string JsonData_Mstr = data[0].ToString();
            string userId = data[1].ToString();

            string result = "";
            try
            {
                if (ModelState.IsValid && userId != null)
                {
                    result = objPIService.SaveUpdateLitem(JsonData_Mstr, userId);
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



        [HttpGet, BasicAuthorization]
        public IHttpActionResult GetLitemDetail(string parameter)
        {
            object resdata = null;
            vmCmnParameters objcmnParam = new vmCmnParameters
            {
                parameter = parameter,

            };
            try
            {
                resdata = objPIService.GetLitemDetail(parameter);
            }
            catch (Exception e)
            {
                e.ToString();
                resdata = "";
            }
            return Json(new { resdata });
        }






    }
}
