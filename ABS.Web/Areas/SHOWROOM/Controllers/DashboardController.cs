using CTGroup.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTGroup.Web.Areas.SHOWROOM.Controllers
{
    public class DashboardController : Controller
    {
        // GET: SECONDARYSALESMISREPORT/Dashboard
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
    }
}