using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyMailWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "MessageContent",
                url: "{controller}/{action}/{index}/{id}",
                defaults: new { controller = "Mail", action = "MessageContent", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "MailList",
                url: "{controller}/{action}/{index}/{page}/{count}",
                defaults: new { controller = "Mail", action = "MailList", id = UrlParameter.Optional }
            );

        }
    }
}
