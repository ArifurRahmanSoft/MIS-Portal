using CTGroup.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABS.Web.Utility
{
    public class UtilityController : Controller
    {
        // GET: Utility
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
    }
}