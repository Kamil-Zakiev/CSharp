using System;
using System.Collections.Concurrent;
using System.Threading;

namespace MultithreadUtils
{
    public static class Logger
    {
        private static readonly ConcurrentDictionary<int, ConsoleColor> colorDict =
            new ConcurrentDictionary<int, ConsoleColor>();

        private static readonly ConcurrentQueue<ConsoleColor> stack = new ConcurrentQueue<ConsoleColor>();

        private static readonly AutoResetEvent AutoResetEvent = new AutoResetEvent(true);

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
}