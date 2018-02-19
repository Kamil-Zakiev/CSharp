using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace SingletonFromManyThreads
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

        /// <summary> Создание синглтона через контейнер безопасно по отношению к потокам </summary>
        public static void Main(string[] args)
        {
            var container = new WindsorContainer();

            container.Register(Component.For<I1>().ImplementedBy<C1>());

            var processorCount = Environment.ProcessorCount;
            var tasks = new Task<I1>[processorCount];
            for (var i = 0; i < processorCount; i++)
            {
                tasks[i] = Task.Run(() => Method(container));
            }

            ManualResetEventSlim.Set();
            Task.WaitAll(tasks);

            var results = tasks.Select(t => t.Result).ToArray();
            foreach (var result in results)
            {
                Console.WriteLine(result.Guid);
            }
        }
    }
}