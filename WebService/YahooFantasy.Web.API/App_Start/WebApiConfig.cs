using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web;
using Microsoft.Practices.Unity;
using FantasySports.Web.API.Unity;
using FantasySports.Web.API.Services;

namespace FantasySports.Web.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var container = new UnityContainer();
            container.RegisterType<IWebClient, WebApiController>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);
            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
        }
    }
}