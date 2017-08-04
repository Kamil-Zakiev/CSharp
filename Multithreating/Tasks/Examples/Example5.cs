using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tasks
{
    internal partial class Program
    {
        /// <summary>
        ///     Есть возможность подписаться на уведомление о необработанных ошибок
        /// </summary>
        private static void Example5()
        {
            TaskScheduler.UnobservedTaskException += (sender, args) => Console.WriteLine("GOTCHA!!!");
            Example5Inner();
            GC.Collect();

            // output: Method Program.RaiseException() got 182 as parameter
            //         GOTCHA!!!
            //         Для продолжения нажмите любую клавишу . . .
        }

        //нужно, потому GC.Collect() не затронет task внутри функции, т.к. жизнь лок. переменных продлевается
        private static void Example5Inner()
        {
            var task = new Task<int>(RaiseException, 182);
            task.Start(TaskScheduler.Current);
            Thread.Sleep(2000);
        }
    }
}