using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SemaphoreExample
{

    internal class Program
    {
        private static readonly Semaphore Semaphore = new Semaphore(0, 600);

        private static readonly EventWaitHandle AutoResetEvent = new AutoResetEvent(true);

        private static int _x;

        private static void Method()
        {
            // в будущем значение семафора станет 600 и все потоки пула (их будет штук 4-12) запустятся
            Semaphore.WaitOne();

            AutoResetEvent.WaitOne();
            _x++;
            AutoResetEvent.Set();
        }

        public static void Main(string[] args)
        {
            const int count = 600;
            var tasks = new Task[count];
            for (var i = 0; i < count; i++)
            {
                tasks[i] = new Task(Method);
                tasks[i].Start();
            }

            Semaphore.Release(count);

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            Task.WaitAll(tasks);

            Console.WriteLine("Elapsed ticks: " + stopWatch.ElapsedTicks); // 7k - 12k

            Console.WriteLine("Expected Value:\t" + count);
            Console.WriteLine("Fact Value:\t" + _x);
        }
    }
}
