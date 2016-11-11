using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace pltkw3ApplicationInitialization.Controllers
{
    public class InitializationController : Controller
    {
        // GET: Initialization
        public ActionResult Index()
        {
            string msg = $"Initialization: WEBSITE_INSTANCE_ID:{Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID")}";

            //Trace.TraceWarning(msg + ", begin");
            MvcApplication.TC.TrackEvent(msg + ", begin");
            Thread.Sleep(30 * 1000);
            MvcApplication.IsInitialized = true;
            //Trace.TraceWarning(msg + ", end");
            MvcApplication.TC.TrackEvent(msg + ", end");
            return View();
        }
    }
}