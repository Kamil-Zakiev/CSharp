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
                return "----->  Result of task  <---";
            });

            var ct = task.ContinueWith(t =>
            {
                Console.WriteLine(t.Result);
                throw new Exception();
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
            ct.ContinueWith(t => Console.WriteLine("Ошибка произошла."), TaskContinuationOptions.OnlyOnFaulted);

            task.Start();
            Thread.Sleep(1000); // придется подождать, потому что задание - это частный случай использования потока из пула
        }
    }
}