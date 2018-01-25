using System;
using System.Threading;

namespace MutexThreadAttachedProperty
{
    internal class Program
    {
        private static void Thread1(Action enter, Action leave)
        {
            enter();
            Console.WriteLine("Thread 1 entered in sync block");
            Thread.Sleep(1000);
            Console.WriteLine("Thread 1 is about to leave from sync block");
            leave();
        }

        private static void Thread2(Action enter, Action leave)
        {
            try
                {
                // так делать - неправильно, 
                // но написано здесь для отображения функциональности мьютекса
                leave();
            }
            catch (Exception e)
            {
                Console.WriteLine("Thread2 cought exception " + e.GetType().Name);
            }

            enter();
            Console.WriteLine("Thread 2 entered in sync block");
            Thread.Sleep(100);
            Console.WriteLine("Thread 2 is about to leave from sync block");
            leave();
        }

        private static void Main(string[] args)
        {
            var autoResetEvent = new AutoResetEvent(true);
            Action enter1 = () => autoResetEvent.WaitOne();
            Action leave1 = () => autoResetEvent.Set();
            new Thread(() => Thread1(enter1, leave1)).Start();
            new Thread(() => Thread2(enter1, leave1)).Start();
            // incorrect behaviour

            Thread.Sleep(2000);
            Console.WriteLine();

            var mutex = new Mutex();
            Action enter2 = () => mutex.WaitOne();
            Action leave2 = () => mutex.ReleaseMutex();
            new Thread(() => Thread1(enter2, leave2)).Start();
            new Thread(() => Thread2(enter2, leave2)).Start();
            // correct behaviour: throwed Exception

            Thread.Sleep(2000);
        }
    }
}