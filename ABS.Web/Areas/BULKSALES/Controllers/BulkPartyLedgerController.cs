using CTGroup.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTGroup.Web.Areas.BULKSALES.Controllers
{
    public class BulkPartyLedgerController : Controller
    {
        // GET: MISREPORT/Dashboard
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
    }
}