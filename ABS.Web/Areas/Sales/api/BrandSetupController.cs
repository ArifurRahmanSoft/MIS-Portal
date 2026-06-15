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
//using System.Data;


namespace CTGroup.Web.Areas.Sales.api
{
    [RoutePrefix("Sales/api/BrandSetup")]
    public class BrandSetupController : ApiController
    {
        private iProductSetupMgt objPIService = null;
       

        public BrandSetupController()
        {
            objPIService = new ProductSetupMgt();
           
        }




        //---------------------------------------------Start------------------------------------------------
        //---------------------------------------------Start------------------------------------------------


        /*
                [HttpGet, BasicAuthorization]
                public IHttpActionResult GetProductDetail(string parameter, string parameter1)
                {
                    vmCmnParameters objcmnParam = new vmCmnParameters
                    {
                        parameter = parameter,
                        parameter1 = parameter1
                    };

                    //vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(parameter[0].ToString());

                    if (parameter == null)
                    {
                        return BadRequest("Invalid request data.");
                    }

                    try
                    {
                        int recordsTotal;
                        //var productDetailList = objPIService.GetProductDetail(objcmnParam, out recordsTotal);
                        var productDetailList = objPIService.GetProductDetail(objcmnParam);

                        return Ok(new
                        {
                            productDetailList
                        });
                    }
                    catch (Exception ex)
                    {
                        return InternalServerError(ex);
                    }
                }

*/



        /*   [HttpGet, BasicAuthorization]
           public DataTable GetProductType()
           {

               DataTable objJVList = null;
               try
               {
                   objJVList = objPIService.GetProductType();
               }
               catch (Exception e)
               {
                   e.ToString();
               }
               return objJVList;

           }*/







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






        //---------------------------------------------End------------------------------------------------
        //---------------------------------------------End------------------------------------------------





    }
}
