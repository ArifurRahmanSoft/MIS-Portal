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
    [RoutePrefix("Sales/api/AutoRiceSalesByProduct")]
    public class AutoRiceSalesByProductController : ApiController
    {
        private iAutoRiceSalesByProductMgt objARCollService = null;

        public AutoRiceSalesByProductController()
        {
            this.objARCollService = new AutoRiceSalesByProductMgt();
        }

        #region GroupDDL


        #endregion

        #region Create
        // POST SaveAutoRiceSaleByProduct
        [ResponseType(typeof(vmAutoRiceSaleByProduct))]
        [HttpPost]
        public async Task<HttpResponseMessage> SaveAutoRiceSaleByProduct(object[] data)
        {
            string result = "";
            try
            {
                vmAutoRiceSaleByProduct model = JsonConvert.DeserializeObject<vmAutoRiceSaleByProduct>(data[0].ToString());
                if (ModelState.IsValid && model != null)
                {
                    result = objARCollService.SaveAutoRiceSaleByProduct(model);
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


        // GET: GetAutoRiceCollection
        [HttpPost]
        public IHttpActionResult GetAutoRiceSalesByProduct(object[] data)
        {
            List<vmAutoRiceSaleByProduct> listUsers = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                listUsers = objARCollService.GetAutoRiceSalesByProduct(objcmnParam, out recordsTotal).ToList();
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
        //            result = objARCollService.DeleteUser(id, companyID, loggedUser);
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
