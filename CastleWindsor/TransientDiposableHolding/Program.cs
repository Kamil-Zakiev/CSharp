using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using TransientDiposableHolding.Components;
using TransientDiposableHolding.Services;

namespace TransientDiposableHolding
{
    internal class Program
    {
        private static void Test1Inner<T>(WindsorContainer container) where T: class, IHasGuidId
        {
            Console.WriteLine("==================================================");
            for (var i = 0; i < 3; i++)
            {
                var service = container.Resolve<T>();
                var hasTrack = container.Kernel.ReleasePolicy.HasTrack(service);
                Console.WriteLine($"It's {hasTrack} that Windsor is tracking {typeof(T).Name} with GuidId = {service.GuidId}");
                Console.WriteLine();
                //специально не вызываем "container.Release(service1);"
            }
            Console.WriteLine("==================================================");
        }

        private static void Test1<T>(WindsorContainer container) where T : class, IHasGuidId
        {
            Test1Inner<T>(container);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private static void DisposeContainer(WindsorContainer container)
        {
            Console.WriteLine("\nContainer is ready to be disposed...");
            container.Dispose();
            Console.WriteLine("Container was disposed.\n");
        }

        /// <summary>
        ///     Утечка памяти при отсутствии вызова Release для Transient-компонента, поддерживающего паттерн IDisposable.
        ///     Windsor отслеживает Transient-компоненты, реализующие IDisposable (неважно кто из двух - компонент или сервис).
        ///     Такой вывод можно сделать потому, что уничтожение объектов происходит не во время сборки мусора,
        ///     а после уничтожения контейнера. Также в контейнере есть функция проверки отслеживания компонента.
        ///     Заметим, что во время уничтожения контейнера происходит вызов Dispose у Transient-компонентов.
        /// </summary>
        private static void Main(string[] args)
        {
            var container = new WindsorContainer();

            // регистрируем сервис с Lifestyle = Transient
            container.Register(Component.For<IService1>().ImplementedBy<Component1>().LifestyleTransient());
            container.Register(Component.For<IService2>().ImplementedBy<Component2>().LifestyleTransient());

            Test1<IService1>(container);
            Test1<IService2>(container);

            DisposeContainer(container);
        }
    }
}