using CTGroup.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTGroup.Web.Areas.Sales.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Sales/Dashboard
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
    }
}