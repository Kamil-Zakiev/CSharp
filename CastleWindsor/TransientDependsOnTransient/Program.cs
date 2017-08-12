using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using SingletonDependsOnTransient.Components;
using SingletonDependsOnTransient.Services;

namespace SingletonDependsOnTransient
{
    internal class Program
    {
        /// <summary>
        ///     Утечка памяти.
        ///     Если у Transient-сервиса №1 есть зависимость от другого Transient-сервиса, поддерживающего IDisposable,
        ///     то произойдет Windsor будет отслеживать №1, поэтому произойдет
        ///     утечка памяти (при отсутствии явного освобождения Release для сервиса-родителя).
        /// </summary>
        private static void Main(string[] args)
        {
            var container = new WindsorContainer();

            // регистрируем сервисы с Lifestyle = Transient
            container.Register(Component.For<IService1>().ImplementedBy<Component1>().LifeStyle.Transient);
            container.Register(Component.For<IService2>().ImplementedBy<Component2>().LifeStyle.Transient);

            Test1(container);

            DisposeContainer(container);
        }

        private static void Test1(WindsorContainer container)
        {
            Test1Inner(container);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private static void Test1Inner(WindsorContainer container)
        {
            Console.WriteLine("==================================================");
            for (var i = 0; i < 3; i++)
            {
                var service1 = container.Resolve<IService1>();
                var hasTrack = container.Kernel.ReleasePolicy.HasTrack(service1);
                Console.WriteLine($"It's {hasTrack} that Windsor is tracking service1 with GuidId = {service1.GuidId}");
                Console.WriteLine();
                // container.Release(service1);
                // специально не вызываем "container.Release(service1);"
            }
            Console.WriteLine("==================================================");
        }

        private static void DisposeContainer(WindsorContainer container)
        {
            Console.WriteLine("\nContainer is ready to be disposed...");
            container.Dispose();
            Console.WriteLine("Container was disposed.\n");
        }
    }
}