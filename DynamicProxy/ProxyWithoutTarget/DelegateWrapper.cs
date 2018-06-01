using System;
using System.Reflection;
using Castle.DynamicProxy;

namespace ProxyWithoutTarget
{
    public class DelegateWrapper
    {
        private static ProxyGenerator ProxyGenerator = new ProxyGenerator();
        
        public static ICalcInterface GetCalcInterfaceFromDelegate(Delegate del)
        {
            var proxy = ProxyGenerator.CreateInterfaceProxyWithoutTarget<ICalcInterface>(new CalcInterceptor(del));
            return proxy;
        }
    }

    public class CalcInterceptor : IInterceptor
    {
        private readonly Delegate _implementaion;
        
        public CalcInterceptor(Delegate del)
        {
            _implementaion = del;
        }
        
        public void Intercept(IInvocation invocation)
        {
            invocation.ReturnValue = _implementaion.DynamicInvoke(invocation.Arguments);
        }
    }
}