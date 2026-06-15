using System.Web.Http;
using System.Web.Mvc;

namespace CTGroup.Web.Areas.COSTING
{
    public class CostingAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "COSTING";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapHttpRoute(
                      name: "CostingApiAction",
                      routeTemplate: "COSTING/api/{controller}/{action}",
                      defaults: new { id = RouteParameter.Optional }
                  );

            context.Routes.MapHttpRoute(
                   name: "CostingApi",
                   routeTemplate: "COSTING/api/{controller}",
                   defaults: new { id = RouteParameter.Optional }
               );

            context.MapRoute(
                "Costing_default",
                "COSTING/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}