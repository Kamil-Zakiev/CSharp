using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using PerWebByThreadsWebApp.Controllers;

namespace PerWebByThreadsWebApp
{
    public interface I1
    {
        Guid Guid { get; }
    }

    internal class C1 : I1
    {
        public Guid Guid { get; } = Guid.NewGuid();
    }
    
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            var container = new WindsorContainer();
            container.Register(Component.For<IWindsorContainer>().Instance(container));
            container.Register(Component.For<I1>().ImplementedBy<C1>().LifestylePerWebRequest());

            //container.Register(Classes.FromThisAssembly().Pick().If(t => typeof(IController).IsAssignableFrom(t)).Configure(c => c.Named(c.Implementation.Name)).LifestylePerWebRequest());

            container.Register(Component.For<HomeController>().ImplementedBy<HomeController>().LifestylePerWebRequest());
            
            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container.Kernel));
            
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}