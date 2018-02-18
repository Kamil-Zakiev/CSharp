using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace NonObviousRegistrations
{
    internal interface I1
    {
        I2 I2 { get; set; }
        I3 I3 { get; set; }
    }

    internal class C1 : I1
    {
        public C1(int a)
        {
            Console.WriteLine(a);
        }

        public C1()
        {
            Console.WriteLine("empty ctor");
        }

        public C1(I3 _i3, I2 _i2)
        {
            Console.WriteLine("I3 _i3, I2 _i2 ctor");
        }

        public C1(I2 _i2)
        {
            Console.WriteLine("I2 _i2 ctor");
        }

        public I2 I2 { get; set; }
        public I3 I3 { get; set; }
    }

    internal interface I2
    {
    }

    internal interface I3
    {
    }

    internal class C2 : I2
    {
        public C2(int abcd)
        {
            Console.WriteLine(abcd);
        }
    }

    internal class C3 : I3
    {
    }

    internal interface I1Factory
    {
        I1 Create(int a);
        void Release(I1 i1);
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            Example4();
        }

        /// <summary> Manual creating </summary>
        private static void Example4()
        {
            var container = new WindsorContainer();

            var reg = Component.For(typeof(I1)).ImplementedBy<C1>().UsingFactoryMethod(f => new C1(1));
            container.Register(reg);
            var component = container.Resolve<I1>();
        }

        /// <summary> Передача параметров при помощи фабрики </summary>
        private static void Example3()
        {
            var container = new WindsorContainer();

            var reg = Component.For(typeof(I1)).ImplementedBy<C1>();
            container.Register(reg);

            container.AddFacility<TypedFactoryFacility>();
            container.Register(Component.For<I1Factory>().AsFactory());

            var factory = container.Resolve<I1Factory>();
            var component = factory.Create(123);

            factory.Release(component);
        }

        /// <summary> Передача параметров в резолв компонента </summary>
        private static void Example2()
        {
            var container = new WindsorContainer();

            container.Register(Component.For(typeof(I2)).ImplementedBy<C2>());

            var comp = container.Resolve<I2>(new Dictionary<string, object> {{"abcd", 123}});
            Console.WriteLine(comp.GetType());
        }

        /// <summary>
        ///     При наличии нескольких конструкторов, резолв использует конструктор с наибольшим(!) числом разрешимых(!)
        ///     зависимостей
        /// </summary>
        private static void Example1()
        {
            var container = new WindsorContainer();

            var reg = Component.For(typeof(I1)).ImplementedBy<C1>();
            container.Register(reg);

            container.Register(Component.For(typeof(I3)).ImplementedBy<C3>());
            container.Register(Component.For(typeof(I2)).ImplementedBy<C2>());

            var comp = container.Resolve<I1>();
            Console.WriteLine(comp.GetType());
        }
    }
}