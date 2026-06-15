using System.Web.Http;
using System.Web.Mvc;

namespace CTGroup.Web.Areas.BULKSALES
{
    public class BulkSalesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "BULKSALES";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapHttpRoute(
                      name: "BulkSalesApiAction",
                      routeTemplate: "BULKSALES/api/{controller}/{action}",
                      defaults: new { id = RouteParameter.Optional }
                  );

            context.Routes.MapHttpRoute(
                   name: "BulkSalesApi",
                   routeTemplate: "BULKSALES/api/{controller}",
                   defaults: new { id = RouteParameter.Optional }
               );

            context.MapRoute(
                "BulkSales_default",
                "BULKSALES/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}