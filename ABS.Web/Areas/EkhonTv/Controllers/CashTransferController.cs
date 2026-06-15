using CTGroup.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTGroup.Web.Areas.EkhonTv.Controllers
{
    public class CashTransferController : Controller
    {
        // GET: EkhonTv/CashTransfer
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
    }
}