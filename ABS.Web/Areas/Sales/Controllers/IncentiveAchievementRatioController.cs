using CTGroup.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTGroup.Web.Areas.Sales.Controllers
{
    public class IncentiveAchievementRatioController : Controller
    {

        // GET: /Sales/PI/
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }

	}
}