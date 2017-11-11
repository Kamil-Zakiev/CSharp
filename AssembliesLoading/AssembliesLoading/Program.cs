using System;
using System.Linq;
using SomeAssembly;

namespace AssembliesLoading
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Example1();
        }

        /// <summary>
        ///     Сборка загружается в домен при генерации машинного кода JIT-компилятором
        ///     Использование класса = создание экземпляра или вызов стат. метода
        ///     а также использование информация о классе через отржение
        /// </summary>
        private static void Example1()
        {
            PrintAssemblesInCurrentAppDomain();
            Console.WriteLine();
            Method();
            Console.WriteLine();
            PrintAssemblesInCurrentAppDomain();
        }

        private static void Method()
        {
            Console.WriteLine("Методы класса SomeClass:" + 
                typeof(SomeClass).GetMethods().Select(x => x.Name).Aggregate((n1,n2) => n1 += "; "  + n2));

            //var t = new SomeClass();

            // SomeClass.M();
        }

        private static void PrintAssemblesInCurrentAppDomain()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Console.WriteLine(string.Join(Environment.NewLine, assemblies.Select(assembly => assembly.FullName)));
        }
    }
}