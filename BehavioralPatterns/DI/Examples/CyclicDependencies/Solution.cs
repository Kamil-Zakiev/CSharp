namespace DI.Examples.CyclicDependencies.Solution
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
        public B()
        {
        }

        public IA A { get; set; }
    }

    class CompositionRoot
    {
        void Compose()
        {
            var b = new B();
            var a = new A(b);
            b.A = a;

            // do something
        }
    }
}
