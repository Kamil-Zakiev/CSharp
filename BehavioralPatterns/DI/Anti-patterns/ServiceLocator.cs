using System;

namespace DI.Anti_patterns.ServiceLocator
{
    interface IDependency
    {
    }

    class Locator
    {
        public static T Resolve<T>() => throw new NotImplementedException();
    }

    class SomeClass
    {
        void Method()
        {
            var service = Locator.Resolve<IDependency>();

            // do smth with service
        }
    }
}
