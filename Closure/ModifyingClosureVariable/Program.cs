using System;

namespace ModifyingClosureVariable
{
    internal class Program
    {
        public static int b = 123;
        private static void Main(string[] args)
        {
            var a = 1; // temp.a = 1
            Action lamba = () =>
            {
                Console.WriteLine(a);
                // Console.WriteLine(temp.a);
            };
            a = 2; // temp.a = 2
            lamba();
            // output: 2
        }
    }
}