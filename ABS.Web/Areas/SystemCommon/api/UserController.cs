using CTGroup.Models;
using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.OracleModel.ViewModel.SystemCommon;
using CTGroup.Service.SystemCommon.Factories;
using CTGroup.Service.SystemCommon.Interfaces;
using CTGroup.Utility;
using CTGroup.Utility.Attributes;
using CTGroup.Utility.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace CTGroup.Web.Areas.SystemCommon.api
{
    [RoutePrefix("SystemCommon/api/User")]
    public class UserController : ApiController
    {
        private iCmnUserMgt objUserService = null;
        private iSystemCommonDDL objddlService = null;


        public UserController()
        {
            this.objUserService = new CmnUserMgt();
            this.objddlService = new SystemCommonDDL();
        }

        [HttpPost]
        public IHttpActionResult GetUser(object[] data)
        {
            long recordsTotal = 0;
            IEnumerable<vmEmployee> listUsers = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                listUsers = objUserService.GetUser(objcmnParam, out recordsTotal).ToList();
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






        [HttpPost]
        public void DeleteAvatar(vmUser _vUser)
        {
            string fileName = _vUser.ImageUrl.ToString();

            try
            {
                var directory = @"D:/Upload/Avatar/";
                string filePath = directory + fileName;

                if (fileName != null)
                {
                    System.IO.File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        #region Finger
        #endregion

        #region Signature
        [HttpPost]
        public HttpResponseMessage UploadSignature()
        {
            int iUploadedCnt = 0;
            string sPath = string.Empty; string result = string.Empty;
            try
            {
                var directory = @"D:/Upload/Signature/";
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
                {
                    System.Web.HttpPostedFile hpf = hfc[iCnt];
                    if (hpf.ContentLength > 0)
                    {
                        if (!File.Exists(sPath + Path.GetFileName(hpf.FileName)))
                        {
                            string newName = DateTime.Now.ToString("ddMMMyyhhmmsstt");
                            string exttension = System.IO.Path.GetExtension(hpf.FileName);
                            int fileSerial = iCnt + 1;
                            string fileName = "Sig_" + newName + fileSerial + exttension;
                            string filePath = directory + fileName;
                            hpf.SaveAs(filePath);
                            result = fileName;
                            iUploadedCnt = iUploadedCnt + 1;
                            hpf.InputStream.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        public void DeleteSignature(vmUser _vUser)
        {
            string fileName = _vUser.SignatUrl.ToString();

            try
            {
                var directory = @"D:/Upload/Signature/";
                string filePath = directory + fileName;

                if (fileName != null)
                {
                    System.IO.File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        [Route("GetCurrentUserPassword/{companyID:int}/{loggedUser:decimal}")]
        [HttpGet]
        public string GetCurrentUserPassword(int companyID, string loggedUser)
        {
            string currentPassword = "";
            try
            {
                currentPassword = objUserService.getCurrentPassword(companyID, loggedUser);
            }
            catch
            {
            }
            return currentPassword;
        }
        [HttpPost]
        public HttpResponseMessage ChangePassword(vmLoginUser model)
        {
            int result = 0;
            try
            {
                result = objUserService.ChangePassword(model);
            }
            catch
            {
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        #endregion







    } 
    
}