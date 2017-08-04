namespace Cancellation
{
    using System;
    using System.Threading;

    internal partial class Program
    {
        private static void Main(string[] args)
        {
            TaskExample3();
        }
        
        private static void Start(int n, CancellationToken cancellationToken)
        {
            Console.WriteLine("Method Program.Start() got {0} as parameter", n);

            while (!cancellationToken.IsCancellationRequested)
            {
                Thread.Sleep(500);
                Console.WriteLine("Method Program.Start() is running...");
            }

            Console.WriteLine("Method Program.Start() is completed!");
        }
    }
}