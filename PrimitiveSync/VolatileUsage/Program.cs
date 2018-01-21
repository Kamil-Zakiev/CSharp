using System;
using System.Threading;

namespace VolatileUsage
{
    internal class Program
    {
        private static bool s_stopWorker;

        /// <summary>
        ///     Пример 1: при релизной версии кода код ведет себя некорректно в многопоточной среде из-за оптимизации
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            Console.WriteLine("Main: letting worker run for 2 seconds");
            new Thread(Worker).Start();
            Thread.Sleep(2000);
            s_stopWorker = true;
            Console.WriteLine("Main: waiting for worker to stop");
        }

        private static void Worker()
        {
            var x = 0;
            while (!Volatile.Read(ref s_stopWorker))
                // while (!s_stopWorker) // <- при релизной версии кода будет зацикливание: 
                //    считывание при каждой итерации происходить не будет
            {
                x++;
            }

            Console.WriteLine("Worker: stopped when x={0}", x);
        }
    }
}