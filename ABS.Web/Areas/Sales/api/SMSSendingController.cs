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
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ABS.Web.Areas.Sales.api
{
    [RoutePrefix("Sales/api/SMSSending")]
    public class SMSSendingController : ApiController
    {
        private iSMSSendingMgt objSMSSending = null;

        public SMSSendingController()
        {
            this.objSMSSending = new SMSSendingMgt();
        }

        // POST SaveRentCollector
        [ResponseType(typeof(vmSMSSending))]
        [HttpPost]
        public async Task<HttpResponseMessage> SendSingleSMS(object[] data)
        {
            
            vmSMSSending model = JsonConvert.DeserializeObject<vmSMSSending>(data[0].ToString());

            string bngmsg = ConvertBngToUnicode(model.MESSAGETEXT);

            String sid = "CityGroupBangla";
            String user = "citygroup";
            String pass = "City@4321";
            string strResult = "";
            String URI = "http://sms.sslwireless.com/pushapi/dynamic/server.php";

            string messageResponse = string.Empty;


            String myParameters = "user=" + user + "&pass=" + pass + "&sms[0][0]=" + "88" + model.MOBILENUMBER + "&sms[0][1]="
                + System.Web.HttpUtility.UrlEncode(bngmsg) + "&sms[0][2]=" + "1234567890" + "&sid=" + sid;

            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                messageResponse = wc.UploadString(URI, myParameters);
            }


            string result = "";
            try
            {
                if (ModelState.IsValid && model != null)
                {
                    result = objSMSSending.SendSingleSMS(model, messageResponse);
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

            if (messageResponse.Contains("SUCCESSFULL"))
            {
                result = "1";
            }
            else
            {
                result = "0";
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        // GET: GetSentMessageDetail
        [HttpPost]
        public IHttpActionResult GetSentMessageDetail(object[] data)
        {
            List<vmSMSSending> listUsers = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                listUsers = objSMSSending.GetSentMessageDetail(objcmnParam, out recordsTotal).ToList();
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


        private string ConvertBngToUnicode(string bngText)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in bngText)
            {
                sb.AppendFormat("{1:x4}", c, (int)c);
            }
            string uniCode = sb.ToString().ToUpper();
            return uniCode;

        }

    }
}
