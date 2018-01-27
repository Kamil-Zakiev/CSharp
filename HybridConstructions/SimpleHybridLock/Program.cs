using System;
using System.Collections.Concurrent;
using System.Threading;

namespace SimpleHybridLock
{
    internal static class Logger
    {
        private static ConcurrentDictionary<int, ConsoleColor> colorDict =
            new ConcurrentDictionary<int, ConsoleColor>();

        private static ConcurrentQueue<ConsoleColor> stack = new ConcurrentQueue<ConsoleColor>();

        static Logger()
        {
            stack.Enqueue(ConsoleColor.Blue);
            stack.Enqueue(ConsoleColor.Green);
            stack.Enqueue(ConsoleColor.DarkYellow);
            stack.Enqueue(ConsoleColor.DarkBlue);
            stack.Enqueue(ConsoleColor.DarkMagenta);
            stack.Enqueue(ConsoleColor.Red);
            stack.Enqueue(ConsoleColor.DarkCyan);

            Console.ForegroundColor = ConsoleColor.White;
        }


        private static readonly AutoResetEvent AutoResetEvent = new AutoResetEvent(true);

        public static void Log(int threadId, string msg)
        {
            AutoResetEvent.WaitOne();
            ConsoleColor consoleColor;
            if (!colorDict.ContainsKey(threadId) && stack.TryDequeue(out consoleColor))
            {
                stack.Enqueue(consoleColor);
                colorDict[threadId] = consoleColor;
            }

            Console.BackgroundColor = colorDict[threadId];
            Console.WriteLine($"Thread #{threadId}: {msg}");

            AutoResetEvent.Set();
        }
    }

    internal class Program
    {
        private static readonly SimpleHybridLock SimpleHybridLock = new SimpleHybridLock();

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

            for (int i = 0; i < 5; i++)
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