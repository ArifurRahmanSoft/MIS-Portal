using CTGroup.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTGroup.Web.Areas.ACCOUNTS.Controllers
{
    public class GeneralLedgerSubHeadWiseController : Controller
    {
        // GET: ACCOUNTS/GeneralLedgerSubHeadWise
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
    }
}