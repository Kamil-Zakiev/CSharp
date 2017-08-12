using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using SingletonDependsOnTransient.Components;
using SingletonDependsOnTransient.Services;

namespace SingletonDependsOnTransient
{
    internal class Program
    {
        private static void Test1(WindsorContainer container)
        {
            Console.WriteLine("==================================================");

            for (var i = 0; i < 10; i++)
            {
                var singletonService = container.Resolve<IService1>();
               // var hasTrack = container.Kernel.ReleasePolicy.HasTrack(singletonService);
               // Console.WriteLine($"hastrack singleton = {hasTrack}");
                Console.WriteLine($"Dependent transient service2 has guidId = {singletonService.Service2.GuidId}");
            }

            Console.WriteLine("==================================================");
        }

        private static void DisposeContainer(WindsorContainer container)
        {
            Console.WriteLine("\nContainer is ready to be disposed...");
            container.Dispose();
            Console.WriteLine("Container was disposed.\n");
        }

        /// <summary>
        ///     Регистрируя для Singleton-сервиса зависимость от Transient-сервисы, мы тем самым продлеваем жизнь
        ///     Transient-компонента до Singleton. Здесь нет утечки памяти,
        ///     но есть ошибка в архитектуре [IDisposable здесь ни на что не влияет].
        ///     Заметим, что при уничтожении контейнера вызывается Dispose по всему графу зависимостей компонента
        /// </summary>
        private static void Main(string[] args)
        {
            var container = new WindsorContainer();

            // регистрируем сервис с Lifestyle = Singleton
            // заметим, container.Kernel.ReleasePolicy.HasTrack(service) == false
            container.Register(Component.For<IService1>().ImplementedBy<Component1>().LifeStyle.Singleton);

            // регистрируем сервис с Lifestyle = Transient
            container.Register(Component.For<IService2>().ImplementedBy<Component2>().LifeStyle.Transient);

            Test1(container);

            DisposeContainer(container);
        }
    }
}