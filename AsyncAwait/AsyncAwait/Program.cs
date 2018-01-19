using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait
{
    class Program
    {
        static List<byte[]> _buffers= new List<byte[]>()
        {
            new byte[9788*1024],
            new byte[9788*1024],
            new byte[9788*1024],
            new byte[9788*1024],
            new byte[9788*1024],
            new byte[9788*1024],
            new byte[9788*1024],
            new byte[9788*1024]
        };

        static List<string> listOfFileNames = new List<string>()
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
         
        static void Main(string[] args)
        {
             MethodWithAsync(); // ~ 40ms

            /* Какие потоки продолжают: потоки из пула
            Started #0 by thread with id: 1
            Started #1 by thread with id: 1
            Started #2 by thread with id: 1
            Started #3 by thread with id: 1
            Started #4 by thread with id: 1
            Started #5 by thread with id: 1
            Started #6 by thread with id: 1
            Started #7 by thread with id: 1
            Continued #0 by thread with id: 4
            Continued #1 by thread with id: 5
            Continued #4 by thread with id: 8
            Continued #5 by thread with id: 9
            Continued #2 by thread with id: 6
            Continued #7 by thread with id: 10
            Continued #3 by thread with id: 7
            Continued #6 by thread with id: 11
             */
            // MethodWithSeq(); // ~ 48ms
        }

        private static void MethodWithSeq()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            Download2(0);
            Download2(1);
            Download2(2);
            Download2(3);
            Download2(4);
            Download2(5);
            Download2(6);
            Download2(7);
                               
            Console.WriteLine("seq, elapsed: " + stopWatch.ElapsedMilliseconds + "ms");
        }

        private static byte[] Download()
        {
            using (var webClient = new WebClient())
            {
                return webClient.DownloadData("https://www.wallpaperup.com/wallpaper/download/505510");
            }
        }


        private static int Download2(int i)
        {
            using (var fs = new FileStream(listOfFileNames[i], FileMode.Open, FileAccess.Read))
            {
                return fs.Read(_buffers[i], 0, _buffers[i].Length);
            }
        }

        private static async Task<int> DownloadAsync2(int j)
        {
            var i = j % 8;
            using (var fs = new FileStream(listOfFileNames[i], FileMode.Open, FileAccess.Read))
            {
                Console.WriteLine("Started #" + j + " by thread with id: " + Thread.CurrentThread.ManagedThreadId);
                var bytes = await fs.ReadAsync(_buffers[i], 0, _buffers[i].Length);
                Console.WriteLine("...Continued #" + j + " by thread with id: " + Thread.CurrentThread.ManagedThreadId);
                return bytes;
            }
        }

        private static void MethodWithAsync()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var count = 16*4;
            var tasks = new Task<int>[count];
            for (int i = 0; i < count; i++)
            {
                tasks[i] = DownloadAsync2(i);
            }

            Task.WaitAll(tasks);
            Console.WriteLine("async, elapsed: " + stopWatch.ElapsedMilliseconds + "ms");
        }

        private static async Task<Byte[]> DownloadAsync()
        {
            using (var webClient = new WebClient())
            {
                var bytes = await webClient.DownloadDataTaskAsync(
                    "https://www.wallpaperup.com/wallpaper/download/505510");
                return bytes;
            }
        }
    }
}
