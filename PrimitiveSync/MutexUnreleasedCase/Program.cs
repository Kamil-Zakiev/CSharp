using System;
using System.Threading;

namespace MutexUnreleasedCase
{
    internal class Program
    {
        private static void Thread1(Action enter, Action leave)
        {
            enter();
            Console.WriteLine("Thread 1 entered in sync block");
            Thread.Sleep(1000);
            Console.WriteLine("Thread 1 is completed without leavinig from sync block");
            
            // leave();
        }

        private static void Thread2(Action enter, Action leave)
        {
            try
            {
                Console.WriteLine("Thread 2 is trying to enter and waits for unlocking...");
                enter();
            }
            catch (Exception e)
            {
                Console.WriteLine("Thread 2 got exception " + e.GetType().Name + " when waited a kernel object");
            }

            Thread.Sleep(100);
            Console.WriteLine("Thread 2 is about to leave from sync block");
            leave();
        }

        private static void Main(string[] args)
        {
            var mutex = new Mutex();
            Action enter2 = () => mutex.WaitOne();
            Action leave2 = () => mutex.ReleaseMutex();
            new Thread(() => Thread1(enter2, leave2)).Start();
            new Thread(() => Thread2(enter2, leave2)).Start();
            // correct behaviour: throwed AbandonedMutexException

            Thread.Sleep(2000);
            Console.WriteLine();

            var autoResetEvent = new AutoResetEvent(true);
            Action enter1 = () => autoResetEvent.WaitOne();
            Action leave1 = () => autoResetEvent.Set();
            new Thread(() => Thread1(enter1, leave1)).Start();
            new Thread(() => Thread2(enter1, leave1)).Start();
            // incorrect behaviour: blocked forever

            Thread.Sleep(2000);
        }
    }
}