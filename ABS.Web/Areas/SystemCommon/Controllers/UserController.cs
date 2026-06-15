using CTGroup.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTGroup.Web.Areas.SystemCommon.Controllers
{
    public class UserController : Controller
    {
        // GET: SystemCommon/User
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }

        [SessionTimeout]
        public ActionResult Group()
        {
            return View();
        }

        [SessionTimeout]
        public ActionResult Type()
        {
            return View();
        }

        [SessionTimeout]
        public ActionResult Supplier()
        {
            return View();
        }

        [SessionTimeout]
        public ActionResult Buyer()
        {
            return View();
        }

        [SessionTimeout]
        public ActionResult BuyerReference()
        {
            return View();
        }
        [SessionTimeout]
        public ActionResult SupplierContactPerson()
        {
            return View();
        }
        [SessionTimeout]
        public ActionResult PartyContactPerson()
        {
            return View();
        }
    }
}