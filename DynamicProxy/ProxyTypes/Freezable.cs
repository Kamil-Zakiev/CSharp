using System.Collections.Generic;
using Castle.DynamicProxy;

namespace ProxyTypes
{
    public static class Freezable
    {
        private static readonly IDictionary<object, IFreezable> Freezables = new Dictionary<object, IFreezable>();
        
        private static readonly ProxyGenerator Generator = new ProxyGenerator();
        
        public static T MakeFreezable<T>() where T: class
        {
            var freezableInterceptor = new FreezableInterceptor();
            var proxy = Generator.CreateClassProxy<T>(freezableInterceptor);
            Freezables.Add(proxy, freezableInterceptor);
            return proxy;
        }

        public static bool IsFreezable(object freezable)
        {
            return freezable != null && Freezables.ContainsKey(freezable);
        }

        public static void Freeze(object freezabl)
        {
            if (!IsFreezable(freezabl))
            {
                throw new NotFreezableObjectException();
            }
            
            Freezables[freezabl].Freeze();
        }

        public static bool IsFrozen(object freezable)
        {
            return IsFreezable(freezable) && Freezables[freezable].IsFrozen;
        }
    }
}