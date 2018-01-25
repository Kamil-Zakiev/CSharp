using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tasks
{
    internal partial class Program
    {
        /// <summary>
        ///     Пример возникновения исключения в задании и его обработка
        /// </summary>
        private static void Example3()
        {
            var task = new Task<int>(RaiseException, 182);
            task.Start(TaskScheduler.Current);

            try
            {
                // Thread.Sleep(3000); //-  no exception
                // Task.WaitAll(task); // exception
                // task.Wait();// - is not necessary because task.Result calls Wait() // exception
                Console.WriteLine(task.Result); // exception
            }
            catch (AggregateException aggregateException)
            {
                var message = string.Join(", ", aggregateException.InnerExceptions.Select(e => e.Message));
                Console.WriteLine(message);
                throw;
            }

            // output: Method Program.RaiseException() got 182 as parameter
            //         Что то пошло не так........

            //         Необработанное исключение: System.AggregateException: 
            //                             Произошла одна или несколько ошибок. --->System.ArgumentException: Что то пошло не так........
        }
    }
}