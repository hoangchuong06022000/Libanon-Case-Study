using CaseStudy.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace CaseStudy
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.DependencyResolver = new NinjectResolver();

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
