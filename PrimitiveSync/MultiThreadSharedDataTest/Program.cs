using System;
using System.Threading;

namespace MultiThreadSharedDataTest
{
    internal class Program
    {
        private static bool _flag = true;
        private static int _x;

        private static void Method()
        {
            while (_flag)
            {
            }

            Interlocked.Increment(ref _x);
            //_x++;
        }

        public static void Main(string[] args)
        {
            const int count = 6;
            for (var i = 0; i < count; i++)
                new Thread(Method).Start();

            Thread.Sleep(200);
            _flag = false;
            Console.WriteLine("Expected Value:\t" + count);
            Console.WriteLine("Fact Value:\t" + _x);
        }
    }
}