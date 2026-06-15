using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel.ViewModel.Sales;
using CTGroup.Service.Sales.Factories;
using CTGroup.Service.Sales.Interfaces;
using CTGroup.Service.SystemCommon.Factories;
using CTGroup.Service.SystemCommon.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ABS.Web.Areas.Sales.api
{
    [RoutePrefix("Sales/api/AutoRiceSale")]
    public class AutoRiceSaleController : ApiController
    {
        private iAutoRiceSaleMgt objBuyerService = null;
        private iSystemCommonDDL objddlService = null;

        public AutoRiceSaleController()
        {
            this.objBuyerService = new AutoRiceSaleMgt();
            this.objddlService = new SystemCommonDDL();
        }

        #region GroupDDL


        #endregion

        #region Create
        // POST SaveRentCollector
        [ResponseType(typeof(vmAutoRiceSale))]
        [HttpPost]
        public async Task<HttpResponseMessage> SaveRentCollector(object[] data)
        {
            string result = "";
            try
            {
                vmAutoRiceSale model = JsonConvert.DeserializeObject<vmAutoRiceSale>(data[0].ToString());
                if (ModelState.IsValid && model != null)
                {
                    result = objBuyerService.SaveRentCollector(model);
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


        #endregion

        #region Read


        // GET: GetRentCollector
        [HttpPost]
        public IHttpActionResult GetRentCollector(object[] data)
        {
            List<vmAutoRiceSale> listUsers = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                listUsers = objBuyerService.GetRentCollector(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                listUsers
            });
        }
        #endregion



        #region Delete
        // DELETE DeleteUserType/1
        //[Route("DeleteUser/{id:int}/{companyID:int}/{loggedUser:int}")]
        //[HttpDelete]
        //public HttpResponseMessage DeleteUser(int? id, int? companyID, int? loggedUser)
        //{
        //    int result = 0;
        //    try
        //    {
        //        if (id != null)
        //            result = objBuyerService.DeleteUser(id, companyID, loggedUser);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //        result = -0;
        //    }

        //    return Request.CreateResponse(HttpStatusCode.OK, result);
        //}

        #endregion


    }
}
