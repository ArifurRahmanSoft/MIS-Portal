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
    [RoutePrefix("Sales/api/TentativeSale")]
    public class TentativeSaleController : ApiController
    {
        private iTentativeSaleMgt objBuyerService = null;
        private iSystemCommonDDL objddlService = null;

        public TentativeSaleController()
        {
            this.objBuyerService = new TentativeSaleMgt();
            this.objddlService = new SystemCommonDDL();
        }

        #region GroupDDL


        #endregion

        #region Create
        // POST SaveTentativeSale
        [ResponseType(typeof(vmTentativeSale))]
        [HttpPost]
        public async Task<HttpResponseMessage> SaveTentativeSale(object[] data)
        {
            string result = "";
            try
            {
                vmTentativeSale model = JsonConvert.DeserializeObject<vmTentativeSale>(data[0].ToString());
                if (ModelState.IsValid && model != null)
                {
                    result = objBuyerService.SaveTentativeSale(model);
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


        // GET: GetTentativeSale
        [HttpPost]
        public IHttpActionResult GetTentativeSale(object[] data)
        {
            List<vmTentativeSale> listUsers = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                listUsers = objBuyerService.GetTentativeSale(objcmnParam, out recordsTotal).ToList();
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

    }
}
