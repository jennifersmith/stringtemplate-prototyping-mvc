using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using stringtemplate_prototyping.Framework;

namespace stringtemplate_prototyping
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("content/{*pathinfo}");
            routes.IgnoreRoute("Content/{*pathinfo}");

            routes.MapRoute(
                "Default", // Route name
                "{*viewpath}", // URL with parameters
                new { controller = "Main", action = "Index", viewPath="Main/Index" } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new StringTemplateViewEngine(new StringTemplateGroupProvider(Server.MapPath("~/Views"))));
            RegisterRoutes(RouteTable.Routes);
        }
    }
}