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
        ///     Регистрируя для Singleton-сервиса зависимость от Scoped-сервиса, мы тем самым продлеваем жизнь
        ///     Scoped-компонента до Singleton. Здесь нет утечки памяти,
        ///     но есть ошибка в архитектуре [IDisposable здесь ни на что не влияет].
        ///     К тому же есть еще одна ошибка: при завершении скоупа соответствующий компонент может освободить сист.ресурсы
        ///     посредством Dispose, и тогда Singleton не сможет(!) пользоваться "кешированной" версией Scoped-компонента.
        ///     Заметим, что при уничтожении контейнера вызывается Dispose по всему графу зависимостей компонента
        /// </summary>
        private static void Main(string[] args)
        {
            var container = new WindsorContainer();

            // регистрируем сервис с Lifestyle = Singleton
            // заметим, container.Kernel.ReleasePolicy.HasTrack(service) == false
            container.Register(Component.For<IService1>().ImplementedBy<Component1>().LifeStyle.Singleton);

            // регистрируем сервис с Lifestyle = Scoped
            container.Register(Component.For<IService2>().ImplementedBy<Component2>().LifeStyle.Scoped());

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
                    var singletonService = container.Resolve<IService1>();
                    Console.WriteLine($"Dependent scoped service2 has guidId = {singletonService.Service2.GuidId}");

                    var scopedService = container.Resolve<IService2>();
                    Console.WriteLine(
                        $"Resolved by container directly scoped service has guidId = {scopedService.GuidId}");
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