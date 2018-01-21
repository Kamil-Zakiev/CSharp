using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwaitThreadsIds
{
    internal class Program
    {
        private static void Log(int offset, string message)
        {
            var offsetStep = "____";
            var offsetSb = new StringBuilder(offsetStep);
            while (offset > 0)
            {
                offsetSb.Append(offsetStep);
                offset--;
            }
            Console.WriteLine(offsetSb.Append(message).ToString());
        }

        private static async Task<int> DownloadAsync(int j)
        {
            var i = j % 8;
            using (var fs = new FileStream(listOfFileNames[i], FileMode.Open, FileAccess.Read))
            {
                Log(1, $"Started #{j} by thread with id: {Thread.CurrentThread.ManagedThreadId}");
                var bytes = await InnerAsync(i, fs, j);

                Log(3, $"Continued #{j} by thread with id: {Thread.CurrentThread.ManagedThreadId}");
                return bytes;
            }
        }

        private static async Task<int> InnerAsync(int i, FileStream fs, int j)
        {
            Log(2, $"Executing0 #{j} by thread with id: {Thread.CurrentThread.ManagedThreadId}");
            await Task.Run(() =>// <- it is principal to not to execute following method but not to run in thread of Pool
            {
                Thread.Sleep(100);
                Log(2, $"Executing1 #{j} by thread with id: {Thread.CurrentThread.ManagedThreadId}");

            });
            Log(2, $"Executing11 #{j} by thread with id: {Thread.CurrentThread.ManagedThreadId}");
            var bytes = await fs.ReadAsync(_buffers[i], 0, _buffers[i].Length);

            Log(2, $"Executing2 #{j} by thread with id: {Thread.CurrentThread.ManagedThreadId}");
            return bytes;
        }

        private static void MethodWithAsync()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var count = 8;
            var tasks = new Task<int>[count];
            for (var i = 0; i < count; i++)
            {
                tasks[i] = DownloadAsync(i);
            }

            Task.WaitAll(tasks);

            Log(1, "async, elapsed: " + stopWatch.ElapsedMilliseconds + "ms");
        }

        /// <summary>
        ///     Сначала выполняется код до последнего в цепочке ожиданий ключевого слова await,
        ///     а следующий код рассматривается как код из ContinueWith объекта Task, который возвращает оператор await.
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            MethodWithAsync();
        }

        #region TestData

        private static readonly List<byte[]> _buffers = new List<byte[]>
        {
            new byte[9788 * 1024],
            new byte[9788 * 1024],
            new byte[9788 * 1024],
            new byte[9788 * 1024],
            new byte[9788 * 1024],
            new byte[9788 * 1024],
            new byte[9788 * 1024],
            new byte[9788 * 1024]
        };

        private static readonly List<string> listOfFileNames = new List<string>
        {
            @"G:\CSharp\AsyncAwait\AsyncAwait\file1.txt",
            @"G:\CSharp\AsyncAwait\AsyncAwait\file2.txt",
            @"G:\CSharp\AsyncAwait\AsyncAwait\file3.txt",
            @"G:\CSharp\AsyncAwait\AsyncAwait\file4.txt",
            @"G:\CSharp\AsyncAwait\AsyncAwait\file5.txt",
            @"G:\CSharp\AsyncAwait\AsyncAwait\file6.txt",
            @"G:\CSharp\AsyncAwait\AsyncAwait\file7.txt",
            @"G:\CSharp\AsyncAwait\AsyncAwait\file8.txt"
        };

        #endregion
    }
}