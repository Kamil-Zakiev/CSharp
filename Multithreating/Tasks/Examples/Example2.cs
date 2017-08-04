using System;
using System.Threading.Tasks;

namespace Tasks
{
    internal partial class Program
    {
        /// <summary>
        ///     Пример заверешния задания и получение результата
        /// </summary>
        private static void Example2()
        {
            var task = new Task<int>(GetResult, 182);
            task.Start(TaskScheduler.Current);

            // task.Wait(); - необязательно, т.к. task.Result вызывает Wait()
            Console.WriteLine(task.Result);

            // output: Method Program.Start() got 182 as parameter
            //         Method Program.Start() is running!
            //         Method Program.Start() is completed!
            //         33124
        }
    }
}