using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CTGroup.Utility.Attributes;

namespace CTGroup.Web.Areas.MISREPORT.Controllers
{
    public class SalesRealizationController : Controller
    {
        // GET: MISREPORT/SalesRealization
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
    }
}