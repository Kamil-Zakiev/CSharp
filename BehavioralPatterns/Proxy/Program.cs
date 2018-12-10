using System;

namespace Proxy
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    public class HeavyObject
    {
        // some big arrays/strings etc.

        public virtual void Method1() { }
        public virtual void Method2() { }
        public virtual void Method3() { }
        public virtual void Method4() { }
    }

    public class LazyProxy : HeavyObject
    {
        private HeavyObject _heavyObject;

        private HeavyObject GetOrCreateHeavyObject()
        {
            return _heavyObject ?? (_heavyObject = new HeavyObject());
        }

        public override void Method1()
        {
            var service = GetOrCreateHeavyObject();
            service.Method1();
        }
    }
}
