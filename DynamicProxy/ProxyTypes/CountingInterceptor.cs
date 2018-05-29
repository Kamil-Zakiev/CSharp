using Castle.DynamicProxy;

namespace ProxyTypes
{
    public class CountingInterceptor: IInterceptor, IHasCount
    {
        public int Count { get; private set; }
        
        public void Intercept(IInvocation invocation)
        {
            Count++;
            invocation.Proceed();
            
        }
    }
}