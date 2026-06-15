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
    [RoutePrefix("Sales/api/DailyClosing")]
    public class DailyClosingController : ApiController
    {
        private iDailyClosingMgt objDailyClosing = null;

        public DailyClosingController()
        {
            this.objDailyClosing = new DailyClosingMgt();
        }

        #region GroupDDL


        #endregion

        #region Create
        // POST SaveRentCollector
        [ResponseType(typeof(vmDailyClosing))]
        [HttpPost]
        public async Task<HttpResponseMessage> SaveDailyClosing(object[] data)
        {
            string result = "";
            try
            {
                vmDailyClosing model = JsonConvert.DeserializeObject<vmDailyClosing>(data[0].ToString());
                if (ModelState.IsValid && model != null)
                {
                    result = objDailyClosing.SaveDailyClosing(model);
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


        // GET: GetDailyClosing
        [HttpPost]
        public IHttpActionResult GetDailyClosing(object[] data)
        {
            List<vmDailyClosing> listUsers = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                listUsers = objDailyClosing.GetDailyClosing(objcmnParam, out recordsTotal).ToList();
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
