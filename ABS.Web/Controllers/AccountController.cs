using CTGroup.Models;
using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.Service.SystemCommon.Factories;
using CTGroup.Service.SystemCommon.Interfaces;
using CTGroup.Utility;
using CTGroup.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CTGroup.Web.Controllers
{
    public class AccountController : Controller
    {
        private iCmnUserMgt objUserService = null;

        public AccountController()
        {
            this.objUserService = new CmnUserMgt();
        }

        // GET: Account
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                return View();
            }
        }

        // GET: Account/Login
        public ActionResult Login()
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                return View();
            }
        }

        // GET: Account/Profile
        public ActionResult MyProfile()
        {
            return View();
        }

        // GET: Account/Login
        [HttpPost]
        [AllowAnonymous]
        public JsonResult Login(vmLoginUser model)
        {
            int result = 0;
            if (ModelState.IsValid)
            {
                try
                {
                    List<vmAuthenticatedUser> objAuthMember = null;
                    objAuthMember = objUserService.Get_CmnUserAuthentication(model);
                    if (objAuthMember != null && objAuthMember.Count > 0)
                    {
                        result = Convert.ToInt32(objAuthMember[0].ReturnValue);
                        if (result > 0)
                        {
                            Session["UserID"] = objAuthMember[0].UserID;
                            Session["CompanyID"] = objAuthMember[0].CompanyID;
                            Session["CompanyName"] = objAuthMember[0].CompanyName;
                            Session["UserFirstName"] = objAuthMember[0].UserFirstName;
                            Session["UserFullName"] = objAuthMember[0].UserFullName;
                            Session["ClientIP"] = HostService.GetIP();
                            Session["CompanyShortName"] = objAuthMember[0].CompanyShortName;
                        }

                        if (objAuthMember[0].IsFirstLogin == 1)
                        {
                            return Json(new { status = result, TargetUrl = objAuthMember[0].TargetPath });
                        }
                        else
                        {
                            return Json(new { status = result, TargetUrl = "/Account/MyProfile" });
                        }
                    }
                }
                catch (Exception e)
                {
                    return Json(new { status = false, errors = e.ToString() });


                    var frame = new StackTrace(true).GetFrame(0);
                    var filename = frame.GetFileName();
                    var line = frame.GetFileLineNumber();

                    Utils u = new Utils();
                    u.LogWrite(e, filename, line);
                }
            }
            return Json(new
            {
                status = result,
                errors = ModelState.Keys.SelectMany(i => ModelState[i].Errors).Select(m => m.ErrorMessage).ToArray()
            });
        }


        // GET: Account/LogOut
        [HttpPost]
        public JsonResult LogOut()
        {
            int result = 0;
            FormsAuthentication.SignOut();
            Session.Abandon();
            result = 1;
            return Json(new { status = result });
        }


    }
}