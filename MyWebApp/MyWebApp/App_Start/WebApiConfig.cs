using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Mvc;
using MyWebApp.Filters;

namespace MyWebApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new {id = RouteParameter.Optional}
            );
            
            GlobalFilters.Filters.Add(new MyActionFilterAttribute());
        }
    }
}