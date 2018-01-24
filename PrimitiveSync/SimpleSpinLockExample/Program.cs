using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleSpinLockExample
{
    internal class Program
    {
        private static SimpleSpinLock mSimpleSpinLock;
        
        private static bool _flag = true;

        private static double _x = 1.001;

        private static void Locked(Action action)
        {
            mSimpleSpinLock.Enter();
            action();
            mSimpleSpinLock.Leave();
        }

        private static void Method()
        {
            // цикл, чтобы позже запустить потоки практически одновременно
            while (_flag)
            {
            }

            Locked(() =>
            {
                // Тут могут быть громоздкие вычисления
                // Доступ к ресурсу в каждый момент времени имеет только один поток...
                _x *= 1.001;
                //Thread.Sleep(1);
            });
        }

        private static void Main()
        {
            const int count = 600;
            var tasks = new Task[count];
            for (var i = 0; i < count; i++)
            {
                tasks[i] = new Task(Method);
                tasks[i].Start();
            }
            
            _flag = false;
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            
            Task.WaitAll(tasks);

            Console.WriteLine("Elapsed ticks: " + stopWatch.ElapsedTicks); // 1k-7k

            var expected = Math.Pow(1.001, count + 1);
            Console.WriteLine("Equals:\t\t" + (Math.Abs(expected - _x) < 0.000000000001));
            Console.WriteLine("Expected Value:\t" + expected);
            Console.WriteLine("Fact Value:\t" + _x);
        }
    }
}