using System;
using System.Web;

namespace MyWebApp
{
    public class MyHttHandler : IHttpHandler
    {
        private readonly Guid _handlerGuidId = Guid.NewGuid();
        public MyHttHandler()
        {
            Logger.Log(_handlerGuidId);
        }
        public void ProcessRequest(HttpContext context)
        {
            Logger.Log(_handlerGuidId);
            var request = context.Request;
            var response = context.Response;

            var info = context.Request.UserHostAddress + "<br/>" + context.Request.UserAgent;
            
            // This handler is called whenever a file ending 
            // in .sample is requested. A file with that extension
            // does not need to exist.
            response.Write("<html>");
            response.Write("<body>");
            response.Write(info);
            response.Write("</body>");
            response.Write("</html>");
        }

        public bool IsReusable
        {
            get
            {
                Logger.Log(_handlerGuidId);
                return false;
            }
        }
    }
}