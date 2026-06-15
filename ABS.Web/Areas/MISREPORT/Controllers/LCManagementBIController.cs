using CTGroup.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTGroup.Web.Areas.MISREPORT.Controllers
{
    public class LCManagementBIController : Controller
    {
        // GET: MISREPORT/LCManagementBI
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
    }
}