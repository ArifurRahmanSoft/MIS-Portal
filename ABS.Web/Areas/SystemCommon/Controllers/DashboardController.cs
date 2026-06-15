using CTGroup.Models;
using CTGroup.Service.SystemCommon.Factories;
using CTGroup.Service.SystemCommon.Interfaces;
using CTGroup.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;
using System.Web.Mvc;

namespace CTGroup.Web.Areas.SystemCommon.Controllers
{
    public class DashboardController : Controller
    {
        // GET: SystemCommon/Dashboard
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
        #region ModifyCompanySession  
        public JsonResult ModifyCompanySession(int id)
        {
            try
            {
                Session["CompanyID"] = id;
                Session["CompanyName"] = "City Group";
                Session["CompanyShortName"] = "CTG";
                return Json(new { ComapnyID = Session["CompanyID"].ToString(), ComapnyName = Session["CompanyName"].ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion ModifyCompanySession  
    }
}