using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskContinuasion
{
    internal class Program
    {
        /// <summary> Пример использования задачи-продолжения </summary>
        private static void Main()
        {
            var task = new Task<string>(() =>
            {
                Thread.Sleep(500); 
                // throw new Exception();
                return "----->  Result of task  <---";
            });

            task.ContinueWith(t => Console.WriteLine(t.Result), TaskContinuationOptions.OnlyOnRanToCompletion);
            task.ContinueWith(t => Console.WriteLine("Ошибка произошла."), TaskContinuationOptions.OnlyOnFaulted);

            task.Start();
            Thread.Sleep(1000); // потому что задание - это частный случай использования потока из пула
        }
    }
}