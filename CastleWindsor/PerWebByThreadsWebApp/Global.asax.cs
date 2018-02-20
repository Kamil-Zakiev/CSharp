using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace PerWebByThreadsWebApp
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            var container = new WindsorContainer();
            container.Register(Component.For<IWindsorContainer>().Instance(container));

            container.Register(Classes.FromThisAssembly().Pick().If(t => typeof(IController).IsAssignableFrom(t))
                .Configure(c => c.Named(c.Implementation.Name)).LifestylePerWebRequest());
            
            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container.Kernel));
            
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}