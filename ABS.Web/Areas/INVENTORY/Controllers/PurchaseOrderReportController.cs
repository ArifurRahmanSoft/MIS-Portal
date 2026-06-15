using CTGroup.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTGroup.Web.Areas.INVENTORY.Controllers
{
    public class PurchaseOrderReportController : Controller
    {
        // GET: INVENTORY/BrandWiseSalesMonthYear
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
    }
}