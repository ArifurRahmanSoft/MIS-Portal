using CTGroup.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CTGroup.Service.Sales.Factories;

namespace CTGroup.Web.Areas.Sales.Controllers
{
    public class SOWiseDocumentUploadController : Controller
    {
        // GET: Sales/DocumentUpload
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
    }
}