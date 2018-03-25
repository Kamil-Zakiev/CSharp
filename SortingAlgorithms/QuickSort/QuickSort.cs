namespace QuickSort
{
    using System;
    using System.Threading.Tasks;

    public enum QuickSortMode
    {
        Serial,
        Parallel
    }

    /// <summary>
    /// Quick sorting implementation. O(n^2) in initial desc case 
    /// </summary>
    public static class QuickSort
    {
        private static void QuickSortInner<T>(T[] array, QuickSortMode quickSortMode = QuickSortMode.Serial,
            int start = 0, int end = -1) where T : IComparable<T>
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
                if (quickSortMode == QuickSortMode.Serial)
                {
                    QuickSortInner(array, quickSortMode, start, anchor - 1);
                }
                else
                {
                    StartRightTask(array, quickSortMode, start, anchor);
                }

            }

            if (anchor + 1 < end)
            {
                if (quickSortMode == QuickSortMode.Serial)
                {
                    QuickSortInner(array, quickSortMode, anchor + 1, end);
                }
                else
                {
                    StartLeftTask(array, quickSortMode, end, anchor);
                }

            }
        }

        private static void StartRightTask<T>(T[] array, QuickSortMode quickSortMode, int start, int anchor)
            where T : IComparable<T>
        {
            var task = new Task(() => QuickSortInner(array, quickSortMode, start, anchor - 1),
                TaskCreationOptions.AttachedToParent);
            task.Start();
        }

        private static void StartLeftTask<T>(T[] array, QuickSortMode quickSortMode, int end, int anchor)
            where T : IComparable<T>
        {
            var task = new Task(() => QuickSortInner(array, quickSortMode, anchor + 1, end),
                TaskCreationOptions.AttachedToParent);
            task.Start();
        }

        public static void Start<T>(T[] array, QuickSortMode quickSortMode = QuickSortMode.Serial)
            where T : IComparable<T>
        {
            if (quickSortMode == QuickSortMode.Serial)
            {
                QuickSortInner(array, quickSortMode);
                return;
            }

            // Task.Run is not appropriate, because it is using TaskCreationOptions.DenyChildAttach
            var task = new Task(() => QuickSortInner(array, quickSortMode));
            task.Start();
            task.Wait();
        }

        public static void StartSerial<T>(T[] array) where T : IComparable<T>
        {
            QuickSortInner(array, QuickSortMode.Serial);
        }
    }
}