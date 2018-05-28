using Castle.DynamicProxy;
using Tests;

namespace ProxyTypes
{
    public class FreezableInterceptor: IInterceptor, IFreezable
    {
        public void Intercept(IInvocation invocation)
        {
            if (IsFrozen && invocation.Method.IsSpecialName && invocation.Method.Name.StartsWith("set_"))
            {
                throw new ObjectFrozenException();
            }

            invocation.Proceed();
        }

        public bool IsFrozen { get; private set; }

        public void Freeze()
        {
            IsFrozen = true;
        }
    }
}