using System;
using System.Diagnostics;
using System.Linq;
using Helpers;

namespace InsertSort
{
    public class InsertSort
    {
        public static void  Start(int[] a)
        {
            for (var i = 1; i < a.Length; i++)
            {
                var j = i;
                var item = a[i];
                while (j > 0 && a[j-1] > item)
                {
                    a[j] = a[j - 1];
                    j--;
                }
                
                a[j] = item;
            }
        }
    }
    
    internal class Program
    {
        public static void Main(string[] args)
        {
            const int count = 100000;
            var array = Enumerable.Range(1, count).Shuffle().ToArray();

            var sw = new Stopwatch();
            sw.Start();
            InsertSort.Start(array);
            sw.Stop();

            Console.WriteLine(sw.ElapsedMilliseconds + "ms");
            // 3614ms for 100000

            Console.WriteLine(array.IsAscSort());
        }
    }
}