using System.Web.Http;
using System.Web.Mvc;

namespace CTGroup.Web.Areas.EkhonTv
{
    public class EkhonTvAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "EkhonTv";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapHttpRoute(
                      name: "EkhonTvApiAction",
                      routeTemplate: "EkhonTv/api/{controller}/{action}",
                      defaults: new { id = RouteParameter.Optional }
                  );

            context.Routes.MapHttpRoute(
                   name: "EkhonTvApi",
                   routeTemplate: "EkhonTv/api/{controller}",
                   defaults: new { id = RouteParameter.Optional }
               );

            context.MapRoute(
                "EkhonTv_default",
                "EkhonTv/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}