using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using TransientHolding.Components;
using TransientHolding.Services;

namespace TransientHolding
{
    internal class Program
    {
        private static void Test1Inner(WindsorContainer container)
        {
            Console.WriteLine("==================================================");
            for (var i = 0; i < 10; i++)
            {
                var service1 = container.Resolve<IService1>();
                var hasTrack = container.Kernel.ReleasePolicy.HasTrack(service1);
                Console.WriteLine($"It's {hasTrack} that Windsor is tracking service1 with GuidId = {service1.GuidId}");
                Console.WriteLine();
                //специально не вызываем "container.Release(service1);"
            }
            Console.WriteLine("==================================================");
        }

        private static void Test1(WindsorContainer container)
        {
            Test1Inner(container);
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
        ///     Windsor не отслеживает Transient-компоненты, не реализующие IDisposable.
        ///     Такой вывод можно сделать по тому, что уничтожение объектов происходит во время сборки мусора, а не во время
        ///     уничтожения контейнера.
        ///     Также в контейнере есть функция проверки отслеживания компонента.
        /// </summary>
        private static void Main(string[] args)
        {
            var container = new WindsorContainer();

            // регистрируем сервис с Lifestyle = Transient
            container.Register(Component.For<IService1>().ImplementedBy<Component1>().LifestyleTransient());

            Test1(container);

            DisposeContainer(container);
        }
    }
}