using System;

namespace DI.ConstructorInjection
{
    public interface IDependency
    {
    }

    public class ConcreteImplementationOfDependency : IDependency
    {
    }

    public class SomeService
    {
        private readonly IDependency _dependency;

        public SomeService(IDependency dependency)
        {
            _dependency = dependency ?? throw new ArgumentNullException(nameof(dependency));
        }
    }
}
