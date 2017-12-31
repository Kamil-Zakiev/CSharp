using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cancellation
{
    internal partial class Program
    {
        /// <summary>
        /// При отмене задания выкидывается исключение "Операция была отменена.", если использовать ThrowIfCancellationRequested
        /// [Если этого не делать, то никаких исключений не будет] - пример далее  
        /// </summary>
        private static void TaskExample1()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            var task = new Task<int>(() => Sum(123, token));
            task.Start();
            Thread.Sleep(1000);
            cts.Cancel(true);

            try
            {
                Console.WriteLine("result = {0}", task.Result);
                #region output
                // output: Method Program.Start() got 123 as parameter
                //         Method Program.Start() is running...
                //         Method Program.Start() is running...
                //         Method Program.Start() is running...
                //         Method Program.Start() is running...
                //         Method Program.Start() is running...
                //         Method Program.Start() is running...
                //         Method Program.Start() is running...
                //         Method Program.Start() is running...
                //         Method Program.Start() is running...
                //         Method Program.Start() is running...
                //         result = 10 
                #endregion
            }
            catch (AggregateException aggregateException)
            {
                var message = string.Join(", ", aggregateException.InnerExceptions.Select(e => e.Message));
                Console.WriteLine(message);
                throw;

                #region output
                // output: Method Program.Start() got 123 as parameter
                //         Method Program.Start() is running...
                //         Method Program.Start() is running...
                //         Операция была отменена.

                //         Необработанное исключение: System.AggregateException: Произошла одна или несколько ошибок. 
                //         --->System.OperationCanceledException: Операция была отменена. 
                #endregion
            }
        }

        private static int Sum(int n, CancellationToken cancellationToken)
        {
            var k = 0;
            Console.WriteLine("Method Program.Start() got {0} as parameter", n);

            while (k<10)
            {
                cancellationToken.ThrowIfCancellationRequested();
                Thread.Sleep(500);
                k++;
                Console.WriteLine("Method Program.Start() is running...");
            }
            return k;
        }
    }
}