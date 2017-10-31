using System;
using System.Runtime.InteropServices;

namespace GCExamples
{
    internal class Program
    {
         void M1()
        {
            
        }

        static GCHandle M()
        {
            for (int i = 0; i < 100; i++)
            {
                var q = new Program();
                q.M1();
            }

            var gcHandle = GCHandle.Alloc(new int(), GCHandleType.Normal);

            Console.WriteLine("Target:\t\t" + gcHandle.Target);
            Console.WriteLine((IntPtr)gcHandle);
            Console.WriteLine("IsAllocated:\t" + gcHandle.IsAllocated);

            return gcHandle;
        }

        public static unsafe   void ghjk()
        {
            for (int i = 0; i < 100; i++)
            {
                var q = new Program();
            }

            IntPtr origin;
            var bytes = new byte[1999];
            
            fixed (Byte* pbytes = bytes)
            {
                origin = (IntPtr) pbytes;
            }

            GC.Collect();


            IntPtr second;
            fixed (Byte* pbytes = bytes)
            {
                second = (IntPtr)pbytes;
                Console.WriteLine(second == origin);
                // true
            }

        }

        private static void Main()
        {
            ghjk();
            return;
            
            var gcHandle = M();
            GC.Collect();
            GC.WaitForFullGCComplete();

            Console.WriteLine("Target:\t\t" + gcHandle.Target);
            Console.WriteLine((IntPtr)gcHandle);
            Console.WriteLine("IsAllocated:\t" + gcHandle.IsAllocated);
        }
    }
}