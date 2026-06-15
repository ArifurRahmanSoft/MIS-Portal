using System.Web.Http;
using System.Web.Mvc;

namespace CTGroup.Web.Areas.SECONDARYSALESMISREPORT
{
    public class SecondaryMISReportAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SECONDARYSALESMISREPORT";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapHttpRoute(
                      name: "SecondaryMISReportApiAction",
                      routeTemplate: "SECONDARYSALESMISREPORT/api/{controller}/{action}",
                      defaults: new { id = RouteParameter.Optional }
                  );

            context.Routes.MapHttpRoute(
                   name: "SecondaryMISReportApi",
                   routeTemplate: "SECONDARYSALESMISREPORT/api/{controller}",
                   defaults: new { id = RouteParameter.Optional }
               );

            context.MapRoute(
                "SecondaryMISReport_default",
                "SECONDARYSALESMISREPORT/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}