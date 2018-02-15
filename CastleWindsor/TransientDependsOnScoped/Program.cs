using System;
using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using SingletonDependsOnScoped.Components;
using SingletonDependsOnScoped.Services;

namespace SingletonDependsOnScoped
{
    internal class Program
    {
        /// <summary>
        ///     Если Транзиентный компонент реализует IDisposable (или имеет зависимость от IDisposable компонента),
        ///     то контейнер будет удерживать ссылки на Scoped-зависимости (хоть у них и вызван Dispose)
        ///     В Б4 NH-сессия и ISessionProvider - имеет Lifestyle = SessionScoped,
        ///     поэтому если транзиентный сервис будет иметь зависимоть от сессии,
        ///     то будет утечка (ведь сессия держит кэш), если не вызвать явный Release
        /// </summary>
        private static void Main(string[] args)
        {
            var container = new WindsorContainer();

            container.Register(Component.For<IService1>().ImplementedBy<Component1>().LifeStyle.Transient);

            // регистрируем сервис с Lifestyle = Scoped
            container.Register(Component.For<IService2>().ImplementedBy<Component2>().LifeStyle.Scoped());

            // регистрируем сервис с Lifestyle = Transient
            container.Register(Component.For<IService3>().ImplementedBy<Component3>().LifeStyle.Transient);

            Test1(container);

            // явно вызовем сборщик мусора, чтобы выяснить, сохраняет ли контейнер ссылки
            GC.Collect();
            GC.WaitForPendingFinalizers();
            DisposeContainer(container);
        }

        private static void Test1(WindsorContainer container)
        {
            Console.WriteLine("==================================================");

            for (var i = 0; i < 3; i++)
                using (container.BeginScope())
                {
                    Console.WriteLine();
                    var transientService = container.Resolve<IService1>();
                    Console.WriteLine($"Dependent scoped service2 has guidId = {transientService.Service2.GuidId}");
                }

            Console.WriteLine("==================================================\n");
        }

        private static void DisposeContainer(WindsorContainer container)
        {
            Console.WriteLine("\nContainer is ready to be disposed...");
            container.Dispose();
            Console.WriteLine("Container was disposed.\n");
        }
    }
}