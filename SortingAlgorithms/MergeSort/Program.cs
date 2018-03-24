using System;
using System.Diagnostics;
using System.Linq;
using Helpers;

namespace MergeSort
{
    public class MergeSort
    {
        public static void StartInner(int[] a, int start = 0, int end = -1)
        {
            if (end == -1)
            {
                end = a.Length - 1;
            }
            
            if()
            
            var delimiter = (start + end) / 2;
            
            
            
            
        }

        public static void Start(int[] a)
        {
            
        }
    }
    
    internal class Program
    {
        public static void Main(string[] args)
        {
            //const int count = 100000;
            const int count = 10;
            var array = Enumerable.Range(1, count).Shuffle().ToArray();

            var sw = new Stopwatch();
            sw.Start();
            MergeSort.Start(array);
            sw.Stop();

            Console.WriteLine(sw.ElapsedMilliseconds + "ms");
            // 17860ms for 100000

            Console.WriteLine(array.IsAscSort());
        }
    }
}