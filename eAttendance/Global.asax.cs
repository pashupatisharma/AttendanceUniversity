using System;
using System.Data.Entity;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using eAttendance.Controllers;
using eAttendance.Models;
using Newtonsoft.Json.Linq;

namespace eAttendance
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;

        }





     


    }


}
