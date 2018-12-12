namespace Proxy
{
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
