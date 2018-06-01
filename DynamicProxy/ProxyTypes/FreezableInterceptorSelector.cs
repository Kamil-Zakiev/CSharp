using System;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;

namespace ProxyTypes
{
    public class FreezableInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            if (method.IsSpecialName && method.Name.StartsWith("set_"))
            {
                return interceptors;
            }

            return interceptors.Where(i => !(i is FreezableInterceptor)).ToArray();
        }
    }
}