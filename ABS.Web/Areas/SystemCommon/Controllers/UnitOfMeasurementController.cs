using CTGroup.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTGroup.Web.Areas.SystemCommon.Controllers
{
    public class UnitOfMeasurementController : Controller
    {
        //
        // GET: /SystemCommon/SizeColorUnit/
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
	}
}