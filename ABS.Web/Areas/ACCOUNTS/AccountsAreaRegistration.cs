using System.Web.Http;
using System.Web.Mvc;

namespace CTGroup.Web.Areas.ACCOUNTS
{
    public class AccountsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ACCOUNTS";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapHttpRoute(
                      name: "AccountsApiAction",
                      routeTemplate: "ACCOUNTS/api/{controller}/{action}",
                      defaults: new { id = RouteParameter.Optional }
                  );

            context.Routes.MapHttpRoute(
                   name: "AccountsApi",
                   routeTemplate: "ACCOUNTS/api/{controller}",
                   defaults: new { id = RouteParameter.Optional }
               );

            context.MapRoute(
                "Accounts_default",
                "ACCOUNTS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}