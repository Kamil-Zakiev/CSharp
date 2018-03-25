using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Helpers;

namespace ShellSort
{
    public class ShellSort
    {
        private class PrattSequence
        {
            private int? _prevValue;
            private bool _switchFlag;

            private Stack<int> Sequence = new Stack<int>();

            private int Next()
            {
                if (!_prevValue.HasValue)
                {
                    _prevValue = 2 * 3;
                    return _prevValue.Value;
                }

                if (_switchFlag)
                    _prevValue *= 2;
                else
                    _prevValue *= 3;

                _switchFlag = !_switchFlag;
                return _prevValue.Value;
            }

            public PrattSequence(int[] array)
            {
                GenerateSequence(array.Length);
            }

            public void GenerateSequence(int n)
            {
                var elem = Next();
                while (elem <= n / 2)
                {
                    Sequence.Push(elem = Next());
                }
            }

            public int GetElem()
            {
                return Sequence.Pop();
            }

            public bool IsEmpty()
            {
                return !Sequence.Any();
            }
        }

        private static void  StartInner(int[] a, int step)
        {
            var start = step;
            if (step == 1)
            {
                start = 0;
            }
            for (var i = start; i < a.Length; i++)
            {
                var j = i;
                var item = a[i];
                while (j >= step && a[j-1] > item)
                {
                    a[j] = a[j - 1];
                    j-= step;
                }
                
                a[j] = item;
            }
        }

        public static void Start(int[] a)
        {
            var prattSequence = new PrattSequence(a);
            var d = prattSequence.GetElem();
            while (!prattSequence.IsEmpty())
            {
                StartInner(a, d);
                d = prattSequence.GetElem();
            }
            
            // insert sort
            StartInner(a, 1);
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
            ShellSort.Start(array);
            sw.Stop();

            Console.WriteLine(sw.ElapsedMilliseconds + "ms");
            // 4ms for 100000

            Console.WriteLine(array.IsAscSort());
        }
    }
}