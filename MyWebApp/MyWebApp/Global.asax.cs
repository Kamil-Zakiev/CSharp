using System;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;

namespace MyWebApp
{
    public class Global : HttpApplication
    {
        private readonly Guid _appGuid;

        protected Global()
        {
            _appGuid = Guid.NewGuid();
            Logger.Log(_appGuid);
        }
        
        
        protected void Application_BeginRequest()
        {
            Logger.Log(_appGuid);
        }
        
        protected void Application_AuthenticateRequest()
        {
            Logger.Log(_appGuid);
        }
        
        


        ~Global()
        {
            Logger.Log(_appGuid);
        }
        
        protected void Application_Start()
        {
            Logger.Log(_appGuid);
            //AreaRegistration.RegisterAllAreas();
            //GlobalConfiguration.Configure(WebApiConfig.Register);
            
            // removing routes registrations enables custom http-handler execution at any route
            // hence mvc route system should explicitly ignore concrete path
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        
        
        
        protected void Application_End()
        {
            Logger.Log(_appGuid);
        }
    }
}