using CTGroup.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTGroup.Web.Areas.Sales.Controllers
{
    public class ProductRateController : Controller
    {
        // GET: Sales/ProductRate
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
    }
}