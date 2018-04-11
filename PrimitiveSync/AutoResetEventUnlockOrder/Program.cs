using System;
using System.Threading;
using System.Threading.Tasks;

namespace AutoResetEventUnlockOrder
{
    internal static class Program
    {
        private static readonly AutoResetEvent AutoResetEvent = new AutoResetEvent(false);

        private static long _firstThreadId;
        private static long _firstUnlockedThreadId;

        private static int _setMark1;
        private static int _setMark2;

        private static void SetFirstThreadId()
        {
            if (Interlocked.Exchange(ref _setMark1, 1) == 0) _firstThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        private static void SetFirstUnlockedThreadId()
        {
            if (Interlocked.Exchange(ref _setMark2, 1) == 0)
                _firstUnlockedThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        private static void ThreadWork()
        {
            SetFirstThreadId();

            AutoResetEvent.WaitOne();

            SetFirstUnlockedThreadId();

            AutoResetEvent.Set();
        }

        public static void Main(string[] args)
        {
            var queueCase = 0;
            var notQueueCase = 0;
            for (var i = 0; i < 1000; i++)
            {
                if (Test())
                {
                    queueCase++;
                }
                else
                {
                    notQueueCase++;
                }
            }

            Console.WriteLine("queueCase = " + queueCase);
            Console.WriteLine("notQueueCase = " + notQueueCase);
            
            // queueCase = 960
            // notQueueCase = 40
        }

        private static bool Test()
        {
            AutoResetEvent.Reset();

            var tasksCount = Environment.ProcessorCount;
            var tasks = new Task[tasksCount];
            for (var i = 0; i < tasksCount; i++) tasks[i] = Task.Factory.StartNew(ThreadWork);

            AutoResetEvent.Set();

            var isQueue = _firstThreadId == _firstUnlockedThreadId;
            return isQueue;
            // Console.WriteLine($"AutoResetEvent supports queue of threads execution is {isQueue} statement");
        }
    }
}