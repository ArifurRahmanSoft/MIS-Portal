using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using CTGroup.Web.Controllers;
using System.Web.Optimization;
using System.Configuration;

namespace CTGroup.Web
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //JSON Return
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
        }


        //void Session_Start(object sender, EventArgs e)
        //{
        //    if (Session.IsNewSession)
        //        Session.Timeout = 120;
        //}

        void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            if (exception != null)
            {
                //Log
                if (HttpContext.Current.Server != null)
                {
                    //HttpContext.Current.Server.Transfer("/siteerror.aspx");

                    //return RedirectToAction("login", "account");
                    //Response.Redirect()
                }
            }
        }
    }
}