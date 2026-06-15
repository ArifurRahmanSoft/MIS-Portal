using System.Web.Http;
using System.Web.Mvc;

namespace CTGroup.Web.Areas.INVENTORY
{
    public class InventoryAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "INVENTORY";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapHttpRoute(
                      name: "InventoryApiAction",
                      routeTemplate: "INVENTORY/api/{controller}/{action}",
                      defaults: new { id = RouteParameter.Optional }
                  );

            context.Routes.MapHttpRoute(
                   name: "InventoryApi",
                   routeTemplate: "INVENTORY/api/{controller}",
                   defaults: new { id = RouteParameter.Optional }
               );

            context.MapRoute(
                "Inventory_default",
                "INVENTORY/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}