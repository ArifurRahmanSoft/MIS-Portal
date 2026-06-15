using CTGroup.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTGroup.Web.Areas.MISREPORT.Controllers
{
    public class BrandWiseSalesMonthYearController : Controller
    {
        // GET: MISREPORT/BrandWiseSalesMonthYear
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
    }
}