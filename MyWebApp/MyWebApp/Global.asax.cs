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
            Log("Global ctor");
            Thread.Sleep(5000);
        }

        private void Log(string methodName)
        {
            Console.WriteLine($"AppGuid: {_appGuid}: {methodName} was called by Thread #{Thread.CurrentThread.ManagedThreadId}." +
                              $" Hashes: [HttpContext - {(object)Context?.GetHashCode() ?? "контекст пуст"}");
        }

        ~Global()
        {
            Log("Global finilize method");
        }
        
        protected void Application_Start()
        {
            Log("Application_Start");
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}