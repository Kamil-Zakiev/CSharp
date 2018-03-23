using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace PassingParameters
{
    internal interface IService
    {
    }

    public class SomeArguments
    {
        public int SomeParameter { get; set; }
    }

    public sealed class MyFirstService : IService
    {
        private readonly int _id;

        public MyFirstService(SomeArguments someArguments)
        {
            _id = GetHashCode();
            Console.Write(".ctor received some parameter: ");
            Console.WriteLine(someArguments.SomeParameter);
        }

        ~MyFirstService()
        {
            Console.WriteLine("Finilizing of component with id = " + _id);
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            var container = new WindsorContainer();
            Example1(container);

            Console.WriteLine("Is going to collect garbage");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Console.WriteLine("Garbage was collected. Needed finilizers executed");

            container.Dispose();
        }

        /// <summary>
        ///     Передача параметров в конструктор через анонимный объект.
        ///     Минус - имя свойства в анонимном объекте должно совпадать с именем аргумента конструктора компонента.
        /// </summary>
        public static void Example1(WindsorContainer container)
        {
            container.Register(Component.For<IService>().ImplementedBy<MyFirstService>().LifestyleTransient());

            var comp1 = container.Resolve<IService>(new
            {
                someArguments = new SomeArguments
                {
                    SomeParameter = 1
                }
            });
            var comp2 = container.Resolve<IService>(new
            {
                someArguments = new SomeArguments
                {
                    SomeParameter = 2
                }
            });
            var comp3 = container.Resolve<IService>(new
            {
                someArguments = new SomeArguments
                {
                    SomeParameter = 3
                }
            });
            var comp4 = container.Resolve<IService>(new
            {
                someArguments = new SomeArguments
                {
                    SomeParameter = 4
                }
            });
        }
        
        /// <summary>
        ///     Передача параметров в конструктор через анонимный объект.
        /// Синглтон только один раз и резолвит, что в принципе не удивительно
        /// </summary>
        public static void Example2(WindsorContainer container)
        {
            container.Register(Component.For<IService>().ImplementedBy<MyFirstService>());

            var comp1 = container.Resolve<IService>(new
            {
                someArguments = new SomeArguments
                {
                    SomeParameter = 1
                }
            });
            var comp2 = container.Resolve<IService>(new
            {
                someArguments = new SomeArguments
                {
                    SomeParameter = 2
                }
            });
            var comp3 = container.Resolve<IService>(new
            {
                someArguments = new SomeArguments
                {
                    SomeParameter = 3
                }
            });
            var comp4 = container.Resolve<IService>(new
            {
                someArguments = new SomeArguments
                {
                    SomeParameter = 4
                }
            });
        }
    }
}