using System.Web.Http;
using System.Web.Mvc;

namespace CTGroup.Web.Areas.SHOWROOM
{
    public class ShowRoomAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SHOWROOM";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapHttpRoute(
                      name: "BulkSalesApiAction",
                      routeTemplate: "SHOWROOM/api/{controller}/{action}",
                      defaults: new { id = RouteParameter.Optional }
                  );

            context.Routes.MapHttpRoute(
                   name: "ShowRoomApi",
                   routeTemplate: "SHOWROOM/api/{controller}",
                   defaults: new { id = RouteParameter.Optional }
               );

            context.MapRoute(
                "ShowRoom_default",
                "SHOWROOM/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}