using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI.Anti_patterns.ConstrainedConstruction
{
    interface IService
    {
    }

    class ConcreteService : IService
    {
        public ConcreteService()
        {
            // initialize
        }
    }

    class SomeClass
    {
        void SomeMethod(Type serviceImplementationType)
        {
            var service = (IService)Activator.CreateInstance(serviceImplementationType);
            // do something with service
        }
    }
}
