using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SOMIOD
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "SomiodApi",
                routeTemplate: "api/somiod/{application}/{module}/{resource}",
                defaults: new { controller = "somiod", application = RouteParameter.Optional, module = RouteParameter.Optional, resource = RouteParameter.Optional }
            );

            //Disable json
            //var formatters = GlobalConfiguration.Configuration.Formatters;
            //formatters.Remove(formatters.JsonFormatter);
        }
    }
}
