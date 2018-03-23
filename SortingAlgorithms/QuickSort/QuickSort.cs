namespace QuickSort
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Quick sorting implementation. O(n^2) in initial desc case 
    /// </summary>
    public static class QuickSort
    {
        private static void QuickSortInner<T>(T[] array, int start = 0, int end = -1) where T : IComparable<T>
        {
            if (end == -1)
            {
                end = array.Length - 1;
            }

            if (end - start == 1 && array[start].CompareTo(array[end]) > 0)
            {
                // case of two items
                var c = array[end];
                array[end] = array[start];
                array[start] = c;
                return;
            }

            // case of three or more items
            var anchor = (start + end) / 2;
            var startPointForSecondPart = anchor + 1;

            for (var i = 0; i < anchor; i++)
            {
                // if (array[i] <= array[anchor])
                if (array[i].CompareTo(array[anchor]) <= 0)
                {
                    continue;
                }

                // need to swap
                var swapI = i;
                if (anchor - i > 1)
                {
                    var preAnchor = anchor - 1;
                    var z = array[i];
                    array[i] = array[preAnchor];
                    array[preAnchor] = z;

                    swapI = preAnchor;
                    i = i - 1;
                }

                var z2 = array[swapI];
                array[swapI] = array[anchor];
                array[anchor] = z2;

                anchor = anchor - 1;
            }

            for (var i = startPointForSecondPart; i <= end; i++)
            {
                if (array[i].CompareTo(array[anchor]) >= 0)
                {
                    continue;
                }

                // need to swap
                var swapI = i;
                if (i - anchor > 1)
                {
                    var postAnchor = anchor + 1;
                    var z = array[i];
                    array[i] = array[postAnchor];
                    array[postAnchor] = z;

                    swapI = postAnchor;
                }

                var z2 = array[swapI];
                array[swapI] = array[anchor];
                array[anchor] = z2;

                anchor = anchor + 1;
            }

            if (start < anchor - 1)
            {
                var task = new Task(() => QuickSortInner(array, start, anchor - 1),
                    TaskCreationOptions.AttachedToParent);
                task.Start();
            }

            if (anchor + 1 < end)
            {
                var task = new Task(() => QuickSortInner(array, anchor + 1, end), TaskCreationOptions.AttachedToParent);
                task.Start();
            }
        }

        public static void Start<T>(T[] array) where T : IComparable<T>
        {
            // Task.Run is not appropriate, because it is using TaskCreationOptions.DenyChildAttach
            var task = new Task(() => QuickSortInner(array));
            task.Start();
            task.Wait();
        }
    }
}