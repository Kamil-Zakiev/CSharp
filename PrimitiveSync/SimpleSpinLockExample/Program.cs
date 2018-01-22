using System;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleSpinLockExample
{
    internal class Program
    {
        private static SimpleSpinLock mSimpleSpinLock;
        
        private static bool _flag = true;

        private static int _x;

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
                _x++;
                Thread.Sleep(1);
            });
        }

        private static void Main()
        {
            const int count = 6;
            var tasks = new Task[count];
            for (var i = 0; i < count; i++)
            {
                tasks[i] = new Task(Method);
                tasks[i].Start();
            }
            
            _flag = false;

            Task.WaitAll(tasks);

            Console.WriteLine("Expected Value:\t" + count);
            Console.WriteLine("Fact Value:\t" + _x);
            Console.WriteLine("\nIdle Count: \t" + mSimpleSpinLock.idleCount);
        }
    }
}