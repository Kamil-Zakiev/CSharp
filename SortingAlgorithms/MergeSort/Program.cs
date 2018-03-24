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

            if (start == end)
            {
                return;
            }

            if (end - start == 1)
            {
                if (a[start] <= a[end])
                {
                    return;
                }
                
                var z = a[start];
                a[start] = a[end];
                a[end] = z;
                return;
            }
            
            var delimiter = (start + end) / 2;
            StartInner(a, start, delimiter);
            StartInner(a, delimiter + 1, end);

            var b = new int[end - start + 1];
            var start1 = start;
            var end1 = delimiter;
            var start2 = delimiter + 1;
            var end2 = end;

            var j = start2;
            var k = 0;
            var i = start1;
            while(i <= end1)
            {
                if (a[i] > a[j])
                {
                    b[k++] = a[j];
                    j++;
                    if (j > end2)
                    {
                        break;
                    }
                }
                else
                {
                    b[k++] = a[i];
                    i++;
                }
            }

            while (j <= end2)
            {
                b[k++] = a[j];
                j++;
            }


            while (i <= end1)
            {
                b[k++] = a[i];
                i++;
            }

            for (int l = 0; l < b.Length; l++)
            {
                a[start + l] = b[l];
            }
        }

        public static void Start(int[] a)
        {
            StartInner(a);
        }
    }
    
    internal class Program
    {
        public static void Main(string[] args)
        {
            const int count = 100000;
            var array = Enumerable.Range(1, count).Reverse().ToArray();

            var sw = new Stopwatch();
            sw.Start();
            MergeSort.Start(array);
            sw.Stop();

            Console.WriteLine(sw.ElapsedMilliseconds + "ms");
            // 8ms for 100000

            Console.WriteLine(array.IsAscSort());
        }
    }
}