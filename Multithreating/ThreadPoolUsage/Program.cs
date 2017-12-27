using System;
using System.Threading;

namespace ThreadPoolUsage
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Example2();
        }

        /// <summary>
        ///     Использование потока из пула.
        ///     Поток фоновый, т.е. завершается, когда все активные потоки финишировали,
        ///     хоть метод обратного вызова не выполнится до конца
        /// </summary>
        private static void Example1()
        {
            ThreadPool.QueueUserWorkItem(Start, 25);
            Console.WriteLine("Program.Main finished!");
            Thread.Sleep(3000);
            // output: Program.Main finished!
        }

        /// <summary>
        ///     Необработанное исключение убивает процесс
        /// </summary>
        private static void Example2()
        {
            ThreadPool.QueueUserWorkItem(RaiseException, 25);
            Console.WriteLine("Program.Main finished!");
            Thread.Sleep(3000);
            // output: Необработанное исключение: System.Exception: Выдано исключение типа "System.Exception".
        }

        private static void Start(object n)
        {
            Console.WriteLine("Method Program.Start() got {0} as parameter", n);
            // Thread.CurrentThread.Abort(); - есть возможность прервать поток
            Console.WriteLine("Method Program.Start() is running!");
            Thread.Sleep(3000);
            Console.WriteLine("Method Program.Start() is completed!");
        }

        private static void RaiseException(object n)
        {
            Console.WriteLine("Method Program.RaiseException() got {0} as parameter", n);
            throw new Exception();
        }
    }
}