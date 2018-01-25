using System;
using System.Threading;

namespace CustomRecursiveProperty
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
            var mutex = new RecursiveAutoResetEvent();
            Action enter2 = () => mutex.Enter();
            Action leave2 = () => mutex.Leave();
            var thread = new Thread(() => Method1(enter2, leave2));
            thread.Start();
            thread.Join();
            Console.WriteLine("ex1");
            // works fine
        }
    }
}