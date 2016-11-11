using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace pltkw3ApplicationInitialization {
    public class MvcApplication : System.Web.HttpApplication {
        public static TelemetryClient TC = new TelemetryClient();
        public volatile static bool IsInitialized = false;
        protected void Application_Start() {
            TC.Context.Component.Version = "2.2";
            string msg = $"Application_Start: WEBSITE_INSTANCE_ID:{Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID")}";
            TC.TrackEvent(msg);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
