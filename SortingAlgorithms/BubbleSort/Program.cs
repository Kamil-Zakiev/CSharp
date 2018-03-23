using System;
using System.Diagnostics;
using System.Linq;
using Helpers;

namespace BubbleSort
{
    public class BubbleSort
    {
        public static void Start(int[] array)
        {
            for (var i = 0; i < array.Length; i++)
            {
                var isSorted = true;
                for (int j = 0; j < array.Length - i - 1; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        var z = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = z;
                        isSorted = false;
                    }
                }

                if (isSorted)
                {
                    return;
                }
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
            BubbleSort.Start(array);
            sw.Stop();

            Console.WriteLine(sw.ElapsedMilliseconds + "ms");
            // 17860ms for 100000

            Console.WriteLine(array.IsAscSort());
        }
    }
}