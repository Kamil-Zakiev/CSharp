using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI.Anti_patterns.ControlFreak
{
    interface IDependency
    {
    }

    class ConcreteImplementation : IDependency
    {
    }

    class SomeClass
    {
        private readonly IDependency _dependency;

        public SomeClass()
        {
            _dependency = new ConcreteImplementation();
        }
    }
}
