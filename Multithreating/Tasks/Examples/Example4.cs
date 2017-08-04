using System.Threading;
using System.Threading.Tasks;

namespace Tasks
{
    internal partial class Program
    {
        /// <summary>
        ///     Пример возникновения исключения в задании и его НЕобработка - никаких исключений!
        ///     *Но есть возможность подписаться на уведомление о необработанных ошибок
        /// </summary>
        private static void Example4()
        {
            var task = new Task<int>(RaiseException, 182);
            task.Start(TaskScheduler.Current);
            Thread.Sleep(2000);

            // output: Method Program.RaiseException() got 182 as parameter
            //         Для продолжения нажмите любую клавишу. . .
        }
    }
}