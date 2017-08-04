namespace ThreadCreation
{
    using System;
    using System.Threading;

    internal class Program
    {
        private static void Main(string[] args)
        {
            Example4();
        }

        /// <summary>
        ///     Создание потока с передачей параметров через замыкание.
        ///     Поток активный, т.е. не завершается, пока метод обратного вызова не выполнится до конца
        /// </summary>
        private static void Example1()
        {
            var myThread = new Thread(() => { Start(1); });
            myThread.Start();
            Console.WriteLine("Program.Main finished!");
            // output: Program.Main finished!
            //         Method Program.Start() is running!
        }

        /// <summary>
        ///     Создание потока с передачей параметров через замыкание.
        ///     Поток фоновый, т.е. завершается, когда все активные потоки финишировали, хоть метод обратного вызова не выполнится
        ///     до конца
        /// </summary>
        private static void Example2()
        {
            var myThread = new Thread(() => { Start(1); });
            myThread.IsBackground = true;
            myThread.Start();
            Console.WriteLine("Program.Main finished!");
            // output: Program.Main finished!
        }

        /// <summary>
        ///     Создание потока с передачей параметров через замыкание.
        ///     Ожидание завершения потока.
        /// </summary>
        private static void Example3()
        {
            var myThread = new Thread(() => { Start(1); });
            myThread.IsBackground = true;
            myThread.Start();
            myThread.Join();
            Console.WriteLine("Program.Main finished!");
            // output: Method Program.Start() got 1 as parameter
            //         Method Program.Start() is running!
            //         Program.Main finished!
        }

        /// <summary>
        ///     Создание потока с нестандартным приоритетом с передачей параметров через замыкание.
        /// </summary>
        private static void Example4()
        {
            var myThread = new Thread(() => { Start(1); });
            myThread.Priority = ThreadPriority.Highest;

            myThread.Start();

            Console.WriteLine("Program.Main finished!");
            // output: Program.Main finished!
            //         Method Program.Start() got 1 as parameter
            //         Method Program.Start() is running!
        }

        private static void Start(int n)
        {
            Console.WriteLine("Method Program.Start() got {0} as parameter", n);
            // Thread.CurrentThread.Abort();
            Console.WriteLine("Method Program.Start() is running!");
            Thread.Sleep(3000);
        }
    }
}