using System;
using System.Web;

namespace MyWebApp
{
    public class ModuleExample : IHttpModule
    {
        private readonly Guid _appGuid;
        
        public ModuleExample()
        {
            _appGuid = Guid.NewGuid();
            Logger.Log(_appGuid);
        }
        public void Init(HttpApplication app)
        {
            Logger.Log(_appGuid);
            app.BeginRequest += BeginRequest;
        }
        public void Dispose()
        {
            Logger.Log(_appGuid);
        }
        
        public void BeginRequest(object source, EventArgs e)
        {
            Logger.Log(_appGuid);
            var app = (HttpApplication)source;
            var context = app.Context;

            if (context.CurrentNotification == RequestNotification.LogRequest)
            {
                if (!context.IsPostNotification)
                {
                    // Put code here that is invoked when the LogRequest event is raised.
                }
                else
                {
                    // PostLogRequest 
                    // Put code here that runs after the LogRequest event completes.
                }
            }

        }
    }
}