# Scenario

## App Service configuration

![App Service configuration - turn ON Always On and turn OFF ARR](2016-11-11_13-06-57.png?raw=true)

Turn ON **Always On** and turn OFF **ARR Affinity**.

## WebTest - check if instance is initialized

![WebTest - check if instance is initialized](2016-11-11_11-25-11.png?raw=true)
## LoadTest
![During LoadTest](2016-11-11_11-51-44.png?raw=true)

Of course - no error during scale up. 

## Sample from Application Inisght Log Analytics

![Sample from Application Inisght Log Analytics](2016-11-11_12-14-39.png?raw=true)
Each requests, initialization events etc. are sended to [Application insight](https://azure.microsoft.com/en-us/documentation/articles/app-insights-overview/).

By the way - GREAT tool for doing analysis based on AI, special language etc , see & try:  [application insights analytics ](https://analytics.applicationinsights.io/demo). Documentation is [here](azure.microsoft.com/en-us/documentation/articles/app-insights-analytics-reference).

## Scale out - analysis
![What happend during scale out (+ 1 instance)](2016-11-11_13-57-41.png?raw=true)

This is screenshoot showing scenario when App Service scale out (+1 instance).

1. New Instance - Application_Start
2. Localhost is calling default web page (in this case /Home/Index)
3. Application Initialization calling defined web pages (to do actual initialization)
4. After initialization is completed, new instance will be registered in load balancer and will be ready to receive normal traffic.

## Important code fragments

### web.config

Configuration for application initialization:

```xml
<configuration>
  <system.webServer>
    <applicationInitialization doAppInitAfterRestart="true" skipManagedModules="false">
      <add initializationPage="/Initialization" hostName="pltkw3applicationinitialization.azurewebsites.net" />
    </applicationInitialization>
  </system.webServer>
</configuration>
```

### InitializationController.cs

Our "initialization":

```c#
public class InitializationController : Controller
{
  public ActionResult Index() {
    string msg = $"Initialization: WEBSITE_INSTANCE_ID:{Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID")}";
    MvcApplication.TC.TrackEvent(msg + ", begin");

    //Our initialization will take more than 30 second 
    Thread.Sleep(30 * 1000);
    MvcApplication.IsInitialized = true;

    MvcApplication.TC.TrackEvent(msg + ", end");
    return View(); //And of course corresponding view etc.
  }
}
```

## Remarks - first instance

Unfortunately, in case of FIRST instance, we need to use another mechanism, like URL Rewrite to redirect traffic to static (HTML!) page to show splash screen during initialization. We need to add something like this to web.config:

```xml
<configuration>
  <system.webServer>
	...
    <rewrite>
      <rules>
        <rule name="All Other Requests" stopProcessing="true">
          <match url=".*" />
          <conditions>
            <add input="{APP_WARMING_UP}" pattern="1" />
            <add input="{WARMUP_REQUEST}" pattern="1" negate="true" />
          </conditions>
          <action type="Rewrite" url="SplashScreen.html" />
        </rule>
      </rules>
    </rewrite>
  </system.webServer>
</configuration>
```

See URL Rewrite on IIS documentation [here](https://www.iis.net/downloads/microsoft/url-rewrite). 

**APP_WARMING_UP** (server variable)  is equal 1 during application initialization. **WARMUP_REQUEST** allow us to detect warming request from <applicationInitialization />.

### Remarks - before run

Please, update *InstrumentationKey* in pltkw3ApplicationInitialization\ApplicationInsights.config