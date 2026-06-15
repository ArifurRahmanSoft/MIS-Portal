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
    [RoutePrefix("Sales/api/AutoRiceCollection")]
    public class AutoRiceCollectionController : ApiController
    {
        private iAutoRiceCollectionMgt objARCollService = null;

        public AutoRiceCollectionController()
        {
            this.objARCollService = new AutoRiceCollectionMgt();
        }

        #region GroupDDL


        #endregion

        #region Create
        // POST SaveAutoRiceCollection
        [ResponseType(typeof(vmAutoRiceCollection))]
        [HttpPost]
        public async Task<HttpResponseMessage> SaveAutoRiceCollection(object[] data)
        {
            string result = "";
            try
            {
                vmAutoRiceCollection model = JsonConvert.DeserializeObject<vmAutoRiceCollection>(data[0].ToString());
                if (ModelState.IsValid && model != null)
                {
                    result = objARCollService.SaveAutoRiceCollection(model);
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
        public IHttpActionResult GetAutoRiceCollection(object[] data)
        {
            List<vmAutoRiceCollection> listUsers = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                listUsers = objARCollService.GetAutoRiceCollection(objcmnParam, out recordsTotal).ToList();
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
