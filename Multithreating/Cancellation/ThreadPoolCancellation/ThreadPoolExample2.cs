using System;
using System.Threading;

namespace Cancellation
{
    internal partial class Program
    {
        /// <summary> Пример регистрации метода, вызываемого после отмены операции </summary>
        private static void ThreadPoolExample2()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            var token2 = cts.Token;
            token.Register(() => { Console.WriteLine("Operation was cancelled"); });

            ThreadPool.QueueUserWorkItem(x => { Start(123, token2); });

            Thread.Sleep(2000);
            cts.Cancel(true);
            // output: Method Program.Start() got 123 as parameter
            //         Method Program.Start() is running...
            //         Method Program.Start() is running...
            //         Method Program.Start() is running...
            //         Operation was cancelled
            //         Method Program.Start is running...
            //         Method Program.Start is completed!
            
            // подождём, пока поток из пула завершит свою работу
            Thread.Sleep(200);
        }
    }
}