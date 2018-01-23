using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace InterlockedPattern
{
    internal class Program
    {
        private static bool _flag = true;

        private static double _x = 1.001;

        private static double Multiple(ref double target, double value)
        {
            var currentValue = target;
            double startValue;
            double desiredValue;

            do
            {
                startValue = currentValue;
                desiredValue = startValue * value;
                currentValue = Interlocked.CompareExchange(ref target, desiredValue, startValue);
            } while (startValue != currentValue);

            return desiredValue;
        }

        private static void Method()
        {
            // цикл, чтобы позже запустить потоки практически одновременно
            while (_flag)
            {
            }


            Multiple(ref _x, 1.001);
            //_x *= 1.001;
        }

        public static void Main(string[] args)
        {
            const int count = 6000;
            var tasks = new Task[count];
            for (var i = 0; i < count; i++)
            {
                tasks[i] = new Task(Method);
                tasks[i].Start();
            }

            _flag = false;
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            
            Task.WaitAll(tasks);

            Console.WriteLine("Elapsed ticks: " + stopWatch.ElapsedTicks); // 

            var expected = Math.Pow(1.001, count + 1);
            Console.WriteLine("Equals:\t\t" + (Math.Abs(expected - _x) < 0.000000000001));
            Console.WriteLine("Expected Value:\t" + expected);
            Console.WriteLine("Fact Value:\t" + _x);
        }
    }
}