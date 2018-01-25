using System;
using System.Threading;

namespace MutexRecursiveProperty
{
    internal class Program
    {
        private static void Method1(Action enter, Action leave)
        {
            enter();
            Method2(enter, leave);
            leave();
        }

        private static void Method2(Action enter, Action leave)
        {
            enter();
            // some work
            leave();
        }

        private static void Main(string[] args)
        {
            var mutex = new Mutex();
            Action enter2 = () => mutex.WaitOne();
            Action leave2 = () => mutex.ReleaseMutex();
            var thread = new Thread(() => Method1(enter2, leave2));
            thread.Start();
            thread.Join();
            Console.WriteLine("ex1");
            // works fine

            var autoResetEvent = new AutoResetEvent(true);
            Action enter1 = () => autoResetEvent.WaitOne();
            Action leave1 = () => autoResetEvent.Set();
            thread = new Thread(() => Method1(enter1, leave1));
            thread.Start();
            thread.Join();
            Console.WriteLine("ex2");
            // deadlock
        }
    }
}