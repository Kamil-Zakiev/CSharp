using System;
using System.Threading;

namespace Cancellation
{
    internal partial class Program
    {
        /// <summary> Пример обработки ошибки в зарегистрированном методе </summary>
        private static void ThreadPoolExample3()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            token.Register(() =>
            {
                Console.WriteLine("Operation was cancelled");
                throw new Exception();
            });

            ThreadPool.QueueUserWorkItem(x => { Start(123, token); });

            Thread.Sleep(2000);
            // передаем параметр выброса исключения при ошибке в зарегистированном методе, иначе они все аггрегируются
            cts.Cancel(true);

            // output:Method Program.Start() got 123 as parameter
            //               Method Program.Start() is running...
            //               Method Program.Start() is running...
            //               Method Program.Start() is running...
            //               Operation was cancelled

            //               Необработанное исключение: Method Program.Start() is running...
            //               Method Program.Start() is completed!
            //               System.Exception: Выдано исключение типа "System.Exception".
        }
    }
}