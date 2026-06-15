using CTGroup.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTGroup.Web.Areas.EkhonTv.Controllers
{
    public class MediaLedgerController : Controller
    {
        // GET: EkhonTv/MediaLedger
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
    }
}