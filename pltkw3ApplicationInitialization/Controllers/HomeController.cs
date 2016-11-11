using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace pltkw3ApplicationInitialization.Controllers {
    public class HomeController : Controller {
        public string Index() {
            string msg = $"WEBSITE_INSTANCE_ID:{Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID")}, ClientIP: {getIPAddress()}, APP_WARMING_UP:{Request.ServerVariables.Get("APP_WARMING_UP")}, IsInitialized:{MvcApplication.IsInitialized}\r\n";
            //Trace.TraceWarning(msg);
            //if (!MvcApplication.IsInitialized) return new HttpStatusCodeResult(500);
            MvcApplication.TC.TrackEvent(msg);
            return msg;
        }

        private string getIPAddress() {
            var context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(ipAddress)) {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0) {
                    return addresses[0];
                }
            }
            return context.Request.ServerVariables["REMOTE_ADDR"];
        }
        public string State() {
            return $"{MvcApplication.IsInitialized}\r\n";
        }
        public ActionResult About() {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact() {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}