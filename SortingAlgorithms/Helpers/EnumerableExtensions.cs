namespace Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumerableExtensions
    {
        /// <summary> Shuffle enumerable </summary>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
        {
            var random = new Random();

            var array = enumerable.ToArray();
            for (var i = 0; i < array.Length - 1; i++)
            {
                var j = random.Next(i + 1, array.Length);
                var z = array[j];
                array[j] = array[i];
                array[i] = z;
            }

            foreach (var item in array)
            {
                yield return item;
            }
        }

        /// <summary> Check if enumerable is sorted by ascending </summary>
        public static bool IsAscSort<T>(this IEnumerable<T> enumerable) where T : IComparable<T>
        {
            T prev = default(T);
            foreach (var item in enumerable)
            {
                if (prev == null || prev.Equals(default(T)))
                {
                    prev = item;
                    continue;
                }

                if (item.CompareTo(prev) < 0)
                {
                    return false;
                }

            }

            return true;
        }
    }
}