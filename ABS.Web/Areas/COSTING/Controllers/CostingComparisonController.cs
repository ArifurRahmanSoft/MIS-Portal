using CTGroup.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTGroup.Web.Areas.COSTING.Controllers
{
    public class CostingComparisonController : Controller
    {
        // GET: SECONDARYSALESMISREPORT/ActivityReport
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
    }
}