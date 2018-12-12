using System;

namespace Proxy
{
    public class ClientCode
    {
        public void Method1()
        {
            var proxy = new LazyProxy();
            Method2(proxy, () => false);
        }

        public void Method2(HeavyObject heavyObject, Func<bool> condition)
        {
            if (condition())
            {
                return;
            }

            heavyObject.Method1();
        }
    }
}
