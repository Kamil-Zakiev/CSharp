using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Castle.Core;
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
    
    
    internal interface I2
    {
        I1 I1 { get; set; }
    }

    class C2 : I2
    {
        public I1 I1 { get; set; }
    }
    
    
    internal interface I3
    {
     
    }
    
    internal interface I4 :IDisposable
    {
     
    }

    class C3 : I3
    {
        [DoNotWire]
        public I4 I4 { get; set; }
    }
    
    class C4 : I4
    {
        public static bool disposed = false;

        public C4()
        {
            Console.WriteLine("Ctor");
        }
        
        public void Dispose()
        {
            disposed = true;
            Console.WriteLine("dispose");
        }
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
            ManagedExternallyTest2();
        }
        
        /// <summary>
        /// managedExternally не влияет на разрешение зависимостей
        /// </summary>
        private static void ManagedExternallyTest()
        {
            var container = new WindsorContainer();
            
            container.Register(Component.For<I1>().ImplementedBy<C1>());
            container.Register(Component.For<I2>().ImplementedBy<C2>()
                .UsingFactoryMethod((k, m, c) => new C2()
                {
                    I1 = k.Resolve<I1>()
                }, true));


            var comp = container.Resolve<I2>();
            Console.WriteLine("comp.I1 == null is " + (comp.I1 == null));
        }
        
        /// <summary>
        /// managedExternally = true указывает контейенру, что не нужно трекать созданный компонент
        /// например, программист сам будет управлять отслеживанием вызовов dispose
        /// </summary>
        private static void ManagedExternallyTest2()
        {
            var container = new WindsorContainer();
            
            container.Register(Component.For<I4>().ImplementedBy<C4>().LifestyleTransient());
            container.Register(Component.For<I3>().ImplementedBy<C3>()
                .UsingFactoryMethod((k, m, c) => new C3()
                {
                    I4 = k.Resolve<I4>()
                }, true).LifestyleTransient());
            

            var comp = container.Resolve<I3>();

            Console.WriteLine(container.Kernel.ReleasePolicy.HasTrack(comp));
            
            
            container.Dispose();
            
            Console.WriteLine(C4.disposed);
        }
        

        /// <summary> Фабричный метод уже проводит синрхронизацию потоков </summary>
        private static void FactoryMethodCallingCount()
        {
            var container = new WindsorContainer();

            var k = 0;
            container.Register(Component.For<I1>().ImplementedBy<C1>().UsingFactoryMethod((kernel) =>
            {
                Interlocked.Increment(ref k);
                ManualResetEventSlim.Wait();
                return new C1();
            }, true));

            var processorCount = Environment.ProcessorCount;
            var tasks = new Task<I1>[processorCount];
            for (var i = 0; i < processorCount; i++)
            {
                tasks[i] = Task.Run(() => container.Resolve<I1>());
            }

            ManualResetEventSlim.Set();
            Task.WaitAll(tasks);

            Console.WriteLine(k);
        }

        /// <summary> Создание синглтона через контейнер безопасно по отношению к потокам </summary>
        private static void OneInstanceCheck()
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