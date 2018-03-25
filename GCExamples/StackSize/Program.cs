using System;

namespace StackSize
{
    class MyStruct
    
    {
        public int Count;
        public int Garbage1;
        public int Garbage2;
        public int Garbage3;
        
    }
    internal class Program
    {
        private static void M(MyStruct myStruct)
        {
            if (myStruct.Count > 1000)
            {
                return;
            }

            myStruct.Count++;
            M(myStruct);
        }
        
        /// <summary>
        /// GC.GetTotalMemory gets only heap, not a stack size
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var startBytes = GC.GetTotalMemory(true);

            var s = new MyStruct(); 
            M(s);
            
            var endBytes = GC.GetTotalMemory(false);

            Console.WriteLine(s.Count);
            Console.WriteLine(endBytes - startBytes);
        }
    }
}