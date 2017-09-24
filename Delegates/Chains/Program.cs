using System;

namespace Chains
{
    public delegate string GetString(int a);

    internal class Program
    {
        private static string Method1(int a)
        {
            Console.WriteLine("Method1 got " + a);
            return "Method1";
        }
        private static string Method2(int a)
        {
            Console.WriteLine("Method2 got " + a);
            return "Method2";
        }
        private string Method3(int a)
        {
            Console.WriteLine("Method3 got " + a);
            return "Method3";
        }
        private string Method4(int a)
        {
            Console.WriteLine("Method4 got " + a);
            return "Method4";
        }

        private static void Main(string[] args)
        {
            Example2();
        }

        /// <summary> Создание цепочки </summary>
        private static void Example1()
        {
            GetString chain = null;
            chain += Method1;
            chain += Method2;
            chain += new Program().Method3;
            chain += new Program().Method4;

            chain(1);
        }

        /// <summary> Удаление экз. делагата из цепочки </summary>
        private static void Example2()
        {
            GetString chain = null;
            chain += Method1;
            chain += Method2;
            var pr = new Program();
            chain += pr.Method3;
            chain += new Program().Method4;
            
            chain(1);
            Console.WriteLine();

            //-------------------------------------------
            chain -= new Program().Method3;
            chain(1);
            // удаление не сработало, т.к. _target разный
            //-------------------------------------------

            //-------------------------------------------
            Console.WriteLine();
            chain -= pr.Method3;
            chain(1);
            // удаление сработало
            //-------------------------------------------

            //-------------------------------------------
            Console.WriteLine();
            chain += Method1;
            chain(1);

            Console.WriteLine();
            chain -= Method1;
            chain(1);
            // из цепочки "удалился" лишь последний экз делегата
        }
    }
}