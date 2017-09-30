using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Lambdas
{
    /// <summary>
    /// см. ILDasm
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            // ReSharper disable once ConvertClosureToMethodGroup
            int a = Console.Read();
            MyMethod(n => Console.WriteLine(a));
        }

        private static void MyMethod(WaitCallback actionRecievingInt)
        {
            actionRecievingInt(5);
        }
    }
}