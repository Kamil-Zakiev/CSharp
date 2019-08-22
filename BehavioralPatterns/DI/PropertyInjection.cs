using System;

namespace DI.PropertyInjection
{
    public interface IDependency
    {

    }

    internal class LocalDefault : IDependency
    {

    }

    public class SomeClass
    {
        private IDependency _dependency;
        public IDependency Dependency
        {
            get
            {
                return _dependency ?? new LocalDefault();
            }
            set
            {
                if(_dependency != null)
                {
                    throw new InvalidOperationException("Dependency is already created");
                }

                _dependency = value ?? throw new ArgumentNullException();
            }
        }
    }
}
