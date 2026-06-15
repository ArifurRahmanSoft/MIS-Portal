using CTGroup.Service.GlobalMgt.Factories;
using CTGroup.Service.GlobalMgt.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CTGroup.Web.Areas.SystemCommon.api
{
    [RoutePrefix("SystemCommon/api/Dashboard")]
    public class DashboardController : ApiController
    {
        iGlobalMgt objGlobalTotal = null;

        public DashboardController()
        {
            objGlobalTotal = new GlobalMgt();
        }
    }
}
