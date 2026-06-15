using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTGroup.Web.Areas.SystemCommon.Controllers
{    
    public class MenuController : Controller
    {
        // GET: /SystemCommon/Menu/
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
	}
}