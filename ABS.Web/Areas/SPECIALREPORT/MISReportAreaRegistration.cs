using System.Web.Http;
using System.Web.Mvc;

namespace CTGroup.Web.Areas.SPECIALREPORT
{
    public class MISReportAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SPECIALREPORT";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapHttpRoute(
                      name: "SpecialReportApiAction",
                      routeTemplate: "SPECIALREPORT/api/{controller}/{action}",
                      defaults: new { id = RouteParameter.Optional }
                  );

            context.Routes.MapHttpRoute(
                   name: "SpecialReportApi",
                   routeTemplate: "SPECIALREPORT/api/{controller}",
                   defaults: new { id = RouteParameter.Optional }
               );

            context.MapRoute(
                "SpecialReport_default",
                "SPECIALREPORT/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}