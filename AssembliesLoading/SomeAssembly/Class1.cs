using System;

namespace SomeAssembly
{
    public class SomeClass
    {
        public SomeClass()
        {
            
        }

        public SomeClass(int a, int b, int c)
        {
            Console.WriteLine($"ctor received parameters: a = {a}, b = {b}, c = {c}");
        }

        public static void M() { }
    }
}