using System;
using System.Threading;

namespace StaticCtorAsConcurrent
{
    public class MyClass
    {
        static MyClass()
        {
            Console.WriteLine(Thread.CurrentThread.Name);
            Console.WriteLine("MyClass static ctor");
        }
    }

    internal class Program
    {
        private static void Method()
        {
            var a = new MyClass();
            Console.WriteLine(a);
        }

        /// <summary> Статический конструктор выполняется один раз, не получилось вызвать его дважды </summary>
        private static void Main(string[] args)
        {
            var thread1 = new Thread(Method);
            var thread2 = new Thread(Method);

            thread1.Name = "Thread1";
            thread2.Name = "Thread2";

            thread2.Start();
            thread1.Start();
        }
    }
}