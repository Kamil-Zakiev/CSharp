using System;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel;

namespace PerWebByThreadsWebApp
{
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel _kernel;
        
        public WindsorControllerFactory(IKernel kernel)
        {
            _kernel = kernel;
        }
        
        public override void ReleaseController(IController controller)
        {
            _kernel.ReleaseComponent(controller);
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                throw new ArgumentNullException();
            }

            return (IController)_kernel.Resolve(controllerType);
        }
    }
}