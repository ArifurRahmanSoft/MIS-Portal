using CTGroup.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTGroup.Web.Areas.SECONDARYSALESMISREPORT.Controllers
{
    public class ActivityReportController : Controller
    {
        // GET: SECONDARYSALESMISREPORT/ActivityReport
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
    }
}