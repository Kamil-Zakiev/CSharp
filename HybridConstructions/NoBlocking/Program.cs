using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NoBlocking
{
    internal class Program
    {
        private static readonly Barrier Barrier = new Barrier(Environment.ProcessorCount);

        private static Singleton SingletonGetter()
        {
            Barrier.SignalAndWait();

            return Singleton.GetSingleton();
        }

        private static void Main()
        {
            var tasks = new Task<Singleton>[Environment.ProcessorCount];
            for (var i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(() => SingletonGetter());
            }

            Task.WaitAll(tasks);

            var firstResult = tasks.First().Result;
            var allTheSame = tasks.Skip(1).All(t => t.Result == firstResult);
            Console.WriteLine($"The statement \"All tasks returned the same object\" is {allTheSame}");
            firstResult.Method();
        }
    }
}