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
    [RoutePrefix("Sales/api/IncentiveFormulaSetup")]
    public class IncentiveFormulaSetupController : ApiController
    {
        private iIncentiveFormulaSetupMgt objPIService = null;

        public IncentiveFormulaSetupController()
        {
            objPIService = new IncentiveFormulaSetupMgt();
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

        // [HttpPost, BasicAuthorization]
        //public IEnumerable<vmIncentiveFormulaSetupDetail> GetIncentiveFormulaSetupDetail(object activePI)
        //{
        //    IEnumerable<vmIncentiveFormulaSetupDetail> objPIItemDetails = null;
        //    try
        //    {
        //        Int64 Id = (Int64)activePI;
        //        objPIItemDetails = objPIService.GetIncentiveFormulaSetupDetail(Id);
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
        public IHttpActionResult GetBrandPopUp(object[] data)
        {
            IEnumerable<vmBrandSKU> objBrandList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objBrandList = objPIService.GetBrandPopUp(objcmnParam, out recordsTotal);
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

        //[HttpPost, BasicAuthorization]
        [HttpPost]
        public HttpResponseMessage SaveUpdateIncentiveFormulaSetup(object[] data)
        {
            vmIncentiveFormulaSetupMaster itemMaster = JsonConvert.DeserializeObject<vmIncentiveFormulaSetupMaster>(data[0].ToString());
            string result = "";
            try
            {
                if (ModelState.IsValid && itemMaster != null)
                {
                    result = objPIService.SaveUpdateIncentiveFormulaSetup(itemMaster);
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
        public HttpResponseMessage SaveUpdateIncentiveRateSetup(object[] data)
        {
            vmIncentiveRateDistRatio incentiveRate = JsonConvert.DeserializeObject<vmIncentiveRateDistRatio>(data[0].ToString());
            List<vmIncentiveRateDistRatio> listDistRatio = JsonConvert.DeserializeObject<List<vmIncentiveRateDistRatio>>(data[1].ToString());
            string result = "";
            try
            {
                if (ModelState.IsValid && incentiveRate != null)
                {
                    result = objPIService.SaveUpdateIncentiveRateSetup(incentiveRate, listDistRatio);
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
        public HttpResponseMessage SaveUpdateIncentiveAchievementRatio(object[] data)
        {
            vmIncentiveAchievementRatio incentiveAchievementRatio = JsonConvert.DeserializeObject<vmIncentiveAchievementRatio>(data[0].ToString());
            List<vmIncentiveAchievementRatio> listIncentiveAchievementRatio = JsonConvert.DeserializeObject<List<vmIncentiveAchievementRatio>>(data[1].ToString());
            string result = "";
            try
            {
                if (ModelState.IsValid && incentiveAchievementRatio != null)
                {
                    result = objPIService.SaveUpdateIncentiveAchievementRatio(incentiveAchievementRatio, listIncentiveAchievementRatio);
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
        //public HttpResponseMessage DeleteUpdateIncentiveFormulaSetupMasterDetails(object[] data)
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

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetIncentiveFormula(object[] data)
        {
            IEnumerable<vmIncentiveFormulaSetupMaster> objIncenFormula = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objIncenFormula = objPIService.GetIncentiveFormula(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objIncenFormula
            });
        }
    }
}
