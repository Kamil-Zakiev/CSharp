using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncBlocking
{
    internal class Program
    {
        private static readonly int _tasksCount = Environment.ProcessorCount * 16;

        private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(0);

        [MethodImpl(MethodImplOptions.NoOptimization)]
        private static async Task AsyncSingletonGetter()
        {
            await SemaphoreSlim.WaitAsync();
            Thread.Sleep(1000);
        }

        [MethodImpl(MethodImplOptions.NoOptimization)]
        private static async Task BlockingSingletonGetter()
        {
            SemaphoreSlim.Wait();
            Thread.Sleep(1000);
        }

        private static void Main()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            Console.WriteLine("Async:");
            var tasks = new Task[_tasksCount];
            for (var i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(AsyncSingletonGetter);
            }

            SemaphoreSlim.Release(_tasksCount);
            Task.WaitAll(tasks);
            Console.WriteLine(stopWatch.ElapsedMilliseconds + "ms");

            stopWatch.Restart();
            Console.WriteLine("\nBlocking:");
            tasks = new Task[_tasksCount];
            for (var i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(BlockingSingletonGetter);
            }

            SemaphoreSlim.Release(_tasksCount);
            Task.WaitAll(tasks);
            Console.WriteLine(stopWatch.ElapsedMilliseconds + "ms");
        }
    }
}