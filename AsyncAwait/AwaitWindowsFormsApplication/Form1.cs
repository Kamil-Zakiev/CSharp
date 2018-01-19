using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AwaitWindowsFormsApplication
{
    public partial class Form1 : Form
    {
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

        public Form1()
        {
            InitializeComponent();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            InfoBox.Items.Clear();
            MethodWithAsync();
        }

        private async Task<int> DownloadAsync2(int j)
        {
            var i = j % 8;
            using (var fs = new FileStream(listOfFileNames[i], FileMode.Open, FileAccess.Read))
            {
                InfoBox.Items.Add("Started #" + j + " by thread with id: " + Thread.CurrentThread.ManagedThreadId);
                var bytes = await fs.ReadAsync(_buffers[i], 0, _buffers[i].Length);
                InfoBox.Items.Add("...Continued #" + j + " by thread with id: " + Thread.CurrentThread.ManagedThreadId);
                return bytes;
            }
        }

        private void MethodWithAsync()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var count = 64;
            var tasks = new Task<int>[count];
            for (var i = 0; i < count; i++)
            {
                tasks[i] = DownloadAsync2(i);
            }

            // Task.WaitAll(tasks); <= это приводит к блокировке UI потока и он не может идти дальше и обновить инфобокс (а это может сделать только GUI-поток)

            InfoBox.Items.Add("async, elapsed: " + stopWatch.ElapsedMilliseconds + "ms");
        }

        private void WithoutContextSyncButton_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                InfoBox.Items.Clear();
                MethodWithAsync();
            });
        }
    }
}