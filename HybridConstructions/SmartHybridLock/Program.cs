using System;
using System.Threading;

namespace SmartHybridLock
{
    internal class Program
    {
        private static readonly SmartHybridLock SimpleHybridLock = new SmartHybridLock();

        private static readonly ManualResetEvent ManualResetEvent = new ManualResetEvent(false);

        private static void Thread1()
        {
            ManualResetEvent.WaitOne();

            SimpleHybridLock.Enter();
            // some work
            SimpleHybridLock.Leave();
        }

        /// <summary>Конкуренция потоков </summary>
        private static void Test1()
        {
            Console.WriteLine("Threads contest situation:");
            ManualResetEvent.Reset();

            new Thread(Thread1).Start();
            new Thread(Thread1).Start();
            new Thread(Thread1).Start();
            new Thread(Thread1).Start();
            new Thread(Thread1).Start();

            ManualResetEvent.Set();

            Thread.Sleep(5000);
        }

        /// <summary> Отсутствие конкуренции </summary>
        private static void Test2()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("\nThreads contest absence:");
            ManualResetEvent.Set();

            for (var i = 0; i < 5; i++)
            {
                new Thread(Thread1).Start();

                // даем потоку фору
                Thread.Sleep(10);
            }

            Thread.Sleep(5000);
        }

        private static void Main(string[] args)
        {
            Test1();
            Test2();
        }
    }
}