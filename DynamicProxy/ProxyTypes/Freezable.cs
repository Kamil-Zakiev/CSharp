using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;

namespace ProxyTypes
{
    public static class Freezable
    {
        private static readonly ProxyGenerator Generator;

        static Freezable()
        {
            Generator = new ProxyGenerator();
        }

        public static T MakeFreezable<T>() where T: class
        {
            var hook = new FreezableProxyGenerationHook();
            var proxyGenOpthions = new ProxyGenerationOptions(hook); 
            
            var freezableInterceptor = new FreezableInterceptor();
            var proxy = Generator.CreateClassProxy<T>(proxyGenOpthions, new CountingInterceptor(), freezableInterceptor);
            return proxy;
        }

        public static bool IsFreezable(object freezable)
        {
            return GetFreezableInterface(freezable) != null;
        }

        private static IFreezable GetFreezableInterface(object freezable)
        {
            if (freezable == null)
            {
                return null;
            }

            var hack = freezable as IProxyTargetAccessor;
            var freezableInterceptor = hack?.GetInterceptors().FirstOrDefault(interceptor => interceptor is IFreezable);
            return freezableInterceptor as IFreezable;
        }

        public static void Freeze(object freezabl)
        {
            var freezableInterface = GetFreezableInterface(freezabl); 
            if (freezableInterface == null)
            {
                throw new NotFreezableObjectException();
            }
            
            freezableInterface.Freeze();
        }

        public static bool IsFrozen(object freezable)
        {
            var freezableInterface = GetFreezableInterface(freezable);
            return freezableInterface != null && freezableInterface.IsFrozen;
        }

        public static int GetCountOfInterceptorInvocations<TInterceptor>(object countableProxy) where TInterceptor: class, IHasCount 
        {
            var hack = countableProxy as IProxyTargetAccessor;
            var countInterceptor =
                hack?.GetInterceptors().FirstOrDefault(i => i is TInterceptor) as TInterceptor;

            if (countInterceptor == null)
            {
                throw new InvalidOperationException();
            }

            return countInterceptor.Count;
        }
    }
}