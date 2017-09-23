using System;

namespace Covariance
{
    internal delegate object GetString(MyClass2 derived);

    internal class MyClass
    {
    }

    internal class MyClass2 : MyClass
    {
    }

    internal class Program
    {
        private static string Method1(MyClass @base)
        {
            return "test";
        }

        /// <summary>
        ///     Ковариантность: тип возвращаемого значения метода Method1 — унаследованный тип от типа возвращаемого значения
        ///     делегата
        ///     Контравариантность:  тип входного значения метода Method1 — базовый тип для типа входного значения делегата
        /// </summary>
        private static void Main(string[] args)
        {
            var func = new GetString(Method1);

            var result = func(new MyClass2());
            Console.WriteLine(result);
        }
    }
}