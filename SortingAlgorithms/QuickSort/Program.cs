namespace QuickSort
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using Helpers;

    internal class Program
    {
        public static void Main(string[] args)
        {
            var count = 1000000;
            var array = Enumerable.Range(1, count).Shuffle().ToArray();
            
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            
            QuickSort.Start(array);
            
            stopWatch.Stop();
            Console.WriteLine("Check: " + array.IsAscSort());
            Console.WriteLine($"Spended {stopWatch.ElapsedMilliseconds}ms when sorting {count} items");
        }
    }
}