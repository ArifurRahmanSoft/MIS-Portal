using CTGroup.Models;
using CTGroup.Models.ViewModel.Sales;
using CTGroup.Models.ViewModel.SystemCommon;
using CTGroup.Service.SystemCommon.Factories;
using CTGroup.Service.SystemCommon.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
//using System.Web.Mvc;

namespace CTGroup.Web.SystemCommon.api
{

    [RoutePrefix("SystemCommon/api/PublicApi")]
    public class PublicApiController : ApiController  
    {
        private iPublicApiMgt objService = null;

       
    }
}
