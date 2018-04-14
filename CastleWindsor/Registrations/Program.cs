using System;
using Castle.MicroKernel;
using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Registrations
{
    #region interfaces, classes

    internal interface IInterface1
    {
    }

    internal interface IInterface2
    {
    }

    internal class MyClass : IInterface1, IInterface2
    {
    }

    internal class MyClass2 : IInterface1, IInterface2
    {
    }

    internal interface I1
    {
        I2 I2 { get; set; }
        I3 I3 { get; set; }
    }

    internal class C1 : I1
    {
        public I2 I2 { get; set; }
        public I3 I3 { get; set; }
    }

    internal interface I2
    {
    }

    internal interface I3
    {
    }

    internal class C2 : I2, I3
    {
    }

    #endregion

    internal class Program
    {
        public static void Main(string[] args)
        {
            Example7();
        }

        /// <summary> Обычное использование </summary>
        private static void Example1()
        {
            var container = new WindsorContainer();

            var reg = Component.For<IInterface1>().ImplementedBy<MyClass>().LifestyleSingleton();

            container.Register(reg);

            var component = container.Resolve<IInterface1>();
            Console.WriteLine(component.GetType());
        }

        /// <summary> Исключение при отсутствии соответствующей регистрации </summary>
        private static void Example2()
        {
            var container = new WindsorContainer();

            var reg = Component.For<IInterface1>().ImplementedBy<MyClass>().LifestyleSingleton();

            container.Register(reg);

            try
            {
                var component2 = container.Resolve<IInterface2>();
                Console.WriteLine(component2.GetType());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary> Ошибка при повторном использовании класса при регистрации </summary>
        private static void Example3()
        {
            var container = new WindsorContainer();

            var reg1 = Component.For<IInterface1>().ImplementedBy<MyClass>().LifestyleSingleton();
            var reg2 = Component.For<IInterface2>().ImplementedBy<MyClass>().LifestyleSingleton();

            container.Register(reg1);

            try
            {
                container.Register(reg2);
            }
            catch (ComponentRegistrationException e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        ///     Регистрация по нескольким интерфейсам, резолв скоупт-компонентов возвращает один и тот же объект по разным
        ///     интерфейсам
        /// </summary>
        private static void Example4()
        {
            var container = new WindsorContainer();

            var reg = Component.For<IInterface1>().Forward<IInterface2>().ImplementedBy<MyClass>().LifestyleScoped();
            container.Register(reg);

            container.BeginScope();
            var component1 = container.Resolve<IInterface1>();
            Console.WriteLine(component1.GetType());

            var component2 = container.Resolve<IInterface2>();
            Console.WriteLine(component2.GetType());

            Console.WriteLine(nameof(component1) + " == " + nameof(component2) + " is " +
                              ReferenceEquals(component1, component2));
        }

        /// <summary>
        ///     IsDefault определяет, какой экземпляр какого типа будет создаваться при Resolve, по умолчанию первая
        ///     регистрация считается дефолтной
        /// </summary>
        private static void Example5()
        {
            var container = new WindsorContainer();

            var reg1 = Component.For<IInterface1>().ImplementedBy<MyClass>();
            var reg2 = Component.For<IInterface1>().ImplementedBy<MyClass2>().IsDefault();
            container.Register(reg1);
            container.Register(reg2);

            var component1 = container.Resolve<IInterface1>();
            Console.WriteLine(component1.GetType());
        }

        /// <summary>
        ///     IsFallback делает регистрацию сервисов резервной, т.е. эта регистрация сработает, если нет иных регистраций
        ///     по тому же интерфейсу
        /// </summary>
        private static void Example6()
        {
            var container = new WindsorContainer();

            var reg1 = Component.For<IInterface1>().Forward<IInterface2>().ImplementedBy<MyClass>().IsFallback();
            var reg2 = Component.For<IInterface1>().ImplementedBy<MyClass2>();
            container.Register(reg1);
            container.Register(reg2);

            var component1 = container.Resolve<IInterface1>();
            Console.WriteLine(component1.GetType());

            var component2 = container.Resolve<IInterface2>();
            Console.WriteLine(component2.GetType());
        }

        /// <summary>
        ///     Регистрация по нескольким интерфейсам, резолв скоупт-компонентов возвращает один и тот же объект по разным
        ///     интерфейсам
        /// </summary>
        private static void Example7()
        {
            var container = new WindsorContainer();

            var reg1 = Component.For<I1>().ImplementedBy<C1>().LifestyleTransient();
            var reg2 = Component.For<I2>().Forward<I3>().ImplementedBy<C2>().LifestyleScoped();
            container.Register(reg1);
            container.Register(reg2);

            var scope1 = container.BeginScope();
            var transientComponent1 = container.Resolve<I1>();
            var transientComponent2 = container.Resolve<I1>();
            scope1.Dispose();

            Console.WriteLine(ReferenceEquals(transientComponent1, transientComponent2));
            Console.WriteLine(ReferenceEquals(transientComponent1.I2, transientComponent2.I3));
        }
    }
}