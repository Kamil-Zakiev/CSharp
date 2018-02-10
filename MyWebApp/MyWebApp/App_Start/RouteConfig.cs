using System.Web.Mvc;
using System.Web.Routing;

namespace MyWebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
           
            // http://localhost:5000/mySection/About.test
            // in order to enable custom http-handler we must ignore path for UrlRoutingModule
            routes.IgnoreRoute("mySection/{path}.test");
            
            //second way - add a route and put in a custom route handler that returns a custom http handler
            
            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new {controller = "Home", action = "Index", id = UrlParameter.Optional}
            );
        }
    }
}