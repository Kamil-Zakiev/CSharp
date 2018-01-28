using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MultithreadUtils;

namespace ConditionVariablePattern
{
    internal class Program
    {
        private static object lockObj = new object();

        private static bool _condition = false;

        public static void Thread1()
        {
            Monitor.Enter(lockObj);
            Logger.Log(Thread.CurrentThread.ManagedThreadId, "locked lockObj");

            while (!_condition)
            {
                Logger.Log(Thread.CurrentThread.ManagedThreadId, "will wait for a condition");
                // снимаем блокировку и даем другому потоку её получить
                // текущий поток блокируется
                Monitor.Wait(lockObj);
                Logger.Log(Thread.CurrentThread.ManagedThreadId, "has waited and got a chance to chech condition one more time");
            }

            Logger.Log(Thread.CurrentThread.ManagedThreadId, "got a true condition and is executing some actions...");
            // обработка данных
            // ...

            Monitor.Exit(lockObj);
            Logger.Log(Thread.CurrentThread.ManagedThreadId, "unlocked lockObj");
        }

        public static void Thread2()
        {
            Monitor.Enter(lockObj);
            Logger.Log(Thread.CurrentThread.ManagedThreadId, "locked lockObj");

            // вычисление условия
            _condition = true;
            
            // Monitor.Pulse(lockObj); // будим один поток после отмены блокировки, остальные останутся заблокированными
             Monitor.PulseAll(lockObj); // будим все потоки после отмены блокировки (они будут выполняться НЕ одновременно)
            // если не вызывать никакую из конструкций, ожидающий потоки не разблокируются, что очевидно

            Logger.Log(Thread.CurrentThread.ManagedThreadId, "pulsed all");

            Monitor.Exit(lockObj);
            Logger.Log(Thread.CurrentThread.ManagedThreadId, "unlocked lockObj");
        }

        private static void Main(string[] args)
        {
            var tasks = new Task[Environment.ProcessorCount];
            for (var i = 0; i < tasks.Length - 1; i++)
            {
                tasks[i] = Task.Run(new Action(Thread1));
            }
            Thread.Sleep(1000);
            tasks[Environment.ProcessorCount - 1] = Task.Run(new Action(Thread2));

            Task.WaitAll(tasks);
        }
    }
}