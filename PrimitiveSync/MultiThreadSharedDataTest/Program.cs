using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreadSharedDataTest
{
    internal class Program
    {
        private static bool _flag = true;
        private static int _x;

        private static void Method()
        {
            while (_flag) { }

            Interlocked.Increment(ref _x);
            //_x++;
        }

        public static void Main(string[] args)
        {
            const int count = 600;
            var tasks = new Task[count];
            for (var i = 0; i < count; i++)
            {
                tasks[i] = new Task(Method);
                tasks[i].Start();
            }
            
            _flag = false;
            Task.WaitAll(tasks);
            
            Console.WriteLine("Expected Value:\t" + count);
            Console.WriteLine("Fact Value:\t" + _x);
        }
    }
}