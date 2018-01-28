using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleWay
{
    internal class Program
    {
        private static readonly CountdownEvent CountdownEvent = new CountdownEvent(Environment.ProcessorCount);

        private static Singleton SingletonGetter()
        {
            CountdownEvent.Wait();

            return Singleton.GetSingleton();
        }

        private static void Main()
        {
            var tasks = new Task<Singleton>[Environment.ProcessorCount];
            for (var i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(() => SingletonGetter());
                CountdownEvent.Signal();
            }
            
            Task.WaitAll(tasks);

            var firstResult = tasks.First().Result;
            var allTheSame = tasks.Skip(1).All(t => t.Result == firstResult);
            Console.WriteLine($"The statement \"All tasks returned the same object\" is {allTheSame}");
        }
    }
}