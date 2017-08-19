using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ValueTypeHashCode
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var listOfKeys = new List<KeyStruct>();

            var dict1 = GetDict(listOfKeys);

            listOfKeys.Shuffle();

            var totalSum = 0m;
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            foreach (var key in listOfKeys)
            {
                var value = dict1[key];
                totalSum += value;
            }
            stopWatch.Stop();
            Console.WriteLine("Total sum = " + totalSum);
            Console.WriteLine("Elapsed ms = " + stopWatch.ElapsedMilliseconds);
        }

        private static Dictionary<KeyStruct, decimal> GetDict(List<KeyStruct> listOfKeys)
        {
            var dict1 = new Dictionary<KeyStruct, decimal>();
            for (var i = 0; i < 1000; i++)
            {
                var key = new KeyStruct
                {
                    Guid1 = Guid.NewGuid(),
                    Guid2 = Guid.NewGuid(),
                    Guid3 = Guid.NewGuid(),
                    Guid4 = Guid.NewGuid()
                };
                listOfKeys.Add(key);
                dict1.Add(key, (decimal) i * i);
            }

            return dict1;
        }
    }

    public struct KeyStruct
    {
        public Guid? Guid1;
        public Guid? Guid2;
        public Guid? Guid3;
        public Guid? Guid4;

        public override int GetHashCode()
        {
            return Guid1.GetHashCode() ^ (Guid2.GetHashCode() ^ 13) ^ (Guid3.GetHashCode() ^ 26) ^ (32 ^ Guid4.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var isTheSameType = obj is KeyStruct?;
            if (!isTheSameType)
            {
                return false;
            }
            var value = (KeyStruct)obj;

            if (value.Guid1 == Guid1 && value.Guid2 == Guid2 && value.Guid3 == Guid3 && value.Guid4 == Guid4)
            {
                return true;
            }

            return false;
        }

    }
}