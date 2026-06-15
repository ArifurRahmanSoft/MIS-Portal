using CTGroup.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTGroup.Web.Areas.SPECIALREPORT.Controllers
{
    public class BrandWiseSalesStatementController : Controller
    {
        // GET: Sales/BrandWiseSalesStatement

        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
    }
}