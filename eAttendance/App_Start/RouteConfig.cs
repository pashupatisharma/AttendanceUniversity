using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace eAttendance
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

//            //            routes.MapRoute(
//            //  name: "ZKADMS",
//            //  url: "iclock/cdata",
//            //  defaults: new { controller = "IClock", action = "CData" }
//            //);

//            routes.MapRoute(
//    name: "HikvisionPush",
//    url: "hikvision/push",
//    defaults: new { controller = "Attendance", action = "HikvisionPush" }
//);


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );
        }
    }
}
