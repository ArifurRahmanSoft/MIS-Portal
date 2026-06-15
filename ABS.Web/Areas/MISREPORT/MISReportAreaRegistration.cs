using System.Web.Http;
using System.Web.Mvc;

namespace CTGroup.Web.Areas.MISREPORT
{
    public class MISReportAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "MISREPORT";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapHttpRoute(
                      name: "MISReportApiAction",
                      routeTemplate: "MISREPORT/api/{controller}/{action}",
                      defaults: new { id = RouteParameter.Optional }
                  );

            context.Routes.MapHttpRoute(
                   name: "MISReportApi",
                   routeTemplate: "MISREPORT/api/{controller}",
                   defaults: new { id = RouteParameter.Optional }
               );

            context.MapRoute(
                "MISReport_default",
                "MISREPORT/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}