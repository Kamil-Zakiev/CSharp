using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cancellation
{
    internal partial class Program
    {
        /// <summary>
        ///     Если использовать IsCancellationRequested - никаких исключений не выкидывается
        /// </summary>
        private static void TaskExample2()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            var task = new Task<int>(() => Sum2(123, token));
            task.Start();
            Thread.Sleep(1000);
            cts.Cancel();
            
            Console.WriteLine("result = {0}", task.Result);

            #region output

            // output: Method Program.Start() got 123 as parameter
            //         Method Program.Start() is running...
            //         Method Program.Start() is running...
            //         result = 2 

            #endregion
        }

        private static int Sum2(int n, CancellationToken cancellationToken)
        {
            var k = 0;
            Console.WriteLine("Method Program.Start() got {0} as parameter", n);

            while (k < 10 && !cancellationToken.IsCancellationRequested)
            {
                Thread.Sleep(500);
                k++;
                Console.WriteLine("Method Program.Start() is running...");
            }
            return k;
        }
    }
}