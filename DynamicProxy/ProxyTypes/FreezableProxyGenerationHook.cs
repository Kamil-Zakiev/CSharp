using System;
using System.Reflection;
using Castle.DynamicProxy;

namespace ProxyTypes
{
    public class FreezableProxyGenerationHook : IProxyGenerationHook
    {
        public void MethodsInspected()
        {
            
            
        }

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
            
        }

        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            return methodInfo.Name.StartsWith("set_");
        }
    }
}