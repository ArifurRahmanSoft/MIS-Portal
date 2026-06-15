using CTGroup.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTGroup.Web.Areas.SECONDARYSALESMISREPORT.Controllers
{
    public class NonOperationalRetailerController : Controller
    {
        // GET: SECONDARYSALESMISREPORT/NonOperationalRetailer
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
    }
}