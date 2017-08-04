using System;
using System.Threading;

namespace Tasks
{
    internal partial class Program
    {
        private static void Main(string[] args)
        {
            Example1();
        }

        /// <summary> Долгоиграющая операция </summary>
        private static void Start(object n)
        {
            Console.WriteLine("Method Program.Start() got {0} as parameter", n);

            // Thread.CurrentThread.Abort(); // - есть ли возможность прервать поток? ДА
            Console.WriteLine("Method Program.Start() is running!");
            Thread.Sleep(3000);
            Console.WriteLine("Method Program.Start() is completed!");
        }

        /// <summary> Операция с результатом </summary>
        private static int GetResult(object n)
        {
            Console.WriteLine("Method Program.Start() got {0} as parameter", n);
            var nInt = (int) n;

            Console.WriteLine("Method Program.Start() is running!");
            Thread.Sleep(3000);
            Console.WriteLine("Method Program.Start() is completed!");
            return nInt * nInt;
        }

        /// <summary> Операция с возникающим внутри необработанным исключением </summary>
        private static int RaiseException(object n)
        {
            Console.WriteLine("Method Program.RaiseException() got {0} as parameter", n);
            throw new ArgumentException("Что то пошло не так........");
        }
    }
}