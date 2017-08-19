using System;
using System.Collections.Generic;

namespace ValueTypeHashCode
{
    public static class ListExtensions
    {
        private static Random _random = new Random();

        private static void Swap<T>(List<T> list, int i1, int i2)
        {
            T c = list[i1];
            list[i1] = list[i2];
            list[i2] = c;
        }

        public static void Shuffle<T>(this List<T> list)
        {
            for (var index = 0; index < list.Count; index++)
            {
                var i1 = _random.Next() % list.Count;
                var i2 = _random.Next() % list.Count;

                Swap(list, i1, i2);
            }
        }
    }
}