using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace ScopedFromManyThreads
{
    internal interface I1
    {
        Guid Guid { get; }
    }

    internal class C1 : I1
    {
        public Guid Guid { get; } = Guid.NewGuid();
    }

    internal class Program
    {
        private static readonly ManualResetEventSlim ManualResetEventSlim = new ManualResetEventSlim(false);

        public static I1 Method(WindsorContainer container)
        {
            ManualResetEventSlim.Wait();
            return container.Resolve<I1>();
        }

        public static void Main(string[] args)
        {
            Example1();
        }

        /// <summary>
        ///     При открытии скоупа после создания потоков падает ошибка, т.к. данные через контекст потока не передались дочерним
        ///     потокам
        /// </summary>
        private static void Example2()
        {
            var container = new WindsorContainer();

            container.Register(Component.For<I1>().ImplementedBy<C1>().LifestyleScoped());

            var processorCount = Environment.ProcessorCount;
            var tasks = new Task<I1>[processorCount];
            for (var i = 0; i < processorCount; i++) tasks[i] = Task.Run(() => Method(container));

            var scope = container.BeginScope();
            //  scope.Dispose();
            ManualResetEventSlim.Set();
            Task.WaitAll(tasks);

            var results = tasks.Select(t => t.Result).ToArray();
            foreach (var result in results) Console.WriteLine(result.Guid);
        }

        /// <summary>
        ///     Данные о скоупе передаются в дочерние потоки через контекст потока
        ///     Резолвится один и тот же экземпляр
        /// </summary>
        private static void Example1()
        {
            var container = new WindsorContainer();

            container.Register(Component.For<I1>().ImplementedBy<C1>().LifestyleScoped());

            var scope = container.BeginScope();
            var processorCount = Environment.ProcessorCount;
            var tasks = new Task<I1>[processorCount];
            for (var i = 0; i < processorCount; i++) tasks[i] = Task.Run(() => Method(container));

            //  scope.Dispose();
            ManualResetEventSlim.Set();
            Task.WaitAll(tasks);

            var results = tasks.Select(t => t.Result).ToArray();
            foreach (var result in results) Console.WriteLine(result.Guid);
        }
    }
}