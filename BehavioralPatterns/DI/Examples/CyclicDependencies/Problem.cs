namespace DI.Examples.CyclicDependencies.Problem
{
    interface IA
    {
    }

    interface IB
    {
    }

    class A : IA
    {
        public A(IB b)
        {
        }
    }

    class B : IB
    {
        public B(IA a)
        {
        }
    }

    class CompositionRoot
    {
        void Compose()
        {
            // how to create instances of A and B?
            /*
            var serviceA = new A(???);
            var serviceB = new B(???);
            */
        }
    }
}
