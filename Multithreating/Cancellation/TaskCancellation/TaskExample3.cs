using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cancellation
{
    internal partial class Program
    {
        /// <summary> Обработка исключения "Операция была отменена" </summary>
        private static void TaskExample3()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            token.Register(() => { Console.WriteLine("my registered method: Operation was cancelled"); });
            var task = new Task<int>(() => Sum(123, token), token);
            task.Start();
            Thread.Sleep(1000);
            cts.Cancel();

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
                aggregateException.Handle(exc => exc is OperationCanceledException);
                Console.WriteLine("Хоть операция и отменена, исключения обработаны!");
                
                #region output

                // output: Method Program.Start() got 123 as parameter
                //         Method Program.Start() is running...
                //         Method Program.Start() is running...
                //         Хоть операция и отменена, исключения обработаны!
                //         Для продолжения нажмите любую клавишу . . .
                
                #endregion
            }
        }
    }
}