using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using SomeAssembly;

namespace AssembliesLoading
{
    internal class Program
    {
        static List<Type> types = new List<Type>();

        private static void Main(string[] args)
        {
            Example3();
        }

        /// <summary>
        ///     Сборка загружается в домен при генерации машинного кода JIT-компилятором
        ///     Использование класса = создание экземпляра или вызов стат. метода
        ///     а также использование типа
        /// </summary>
        private static void Example1()
        {
            PrintAssemblesInCurrentAppDomain();
            Console.WriteLine();
            Method();
            Console.WriteLine();
            PrintAssemblesInCurrentAppDomain();
        }

        /// <summary> Явная загрузка сборки </summary>
        private static void Example2()
        {
            var assem = Assembly.Load("SomeAssembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            Console.WriteLine("Assembly was found at url: " + assem.CodeBase);
            Console.WriteLine("Базовая папка текущего домена: " + AppDomain.CurrentDomain.BaseDirectory);

            Console.WriteLine("Exported types:");
            foreach (var assemExportedType in assem.ExportedTypes)
            {
                Console.WriteLine(assemExportedType.FullName);
            }
        }

        /// <summary> Создание экземпляра типа, определенного в загруженной сборке </summary>
        private static void Example3()
        {
           // var assem = Assembly.Load("SomeAssembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            PrintAssemblesInCurrentAppDomain();

            // не дает загружать типы из другой сборки, хоть сборка и в домене
            // требуется явно указать сборку
            var someType = Type.GetType("SomeAssembly.SomeClass, SomeAssembly", true);
            Console.WriteLine();
            PrintAssemblesInCurrentAppDomain();

            // var someType = assem.ExportedTypes.First(t => t.Name == "SomeClass");

            object someInstance1 = System.Activator.CreateInstance(someType, 1, 2, 3);

            // десериализованный объект ?
            object someInstance2 = System.Activator.CreateInstance("SomeAssembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", 
                "SomeAssembly.SomeClass")
                .Unwrap();
            // десериализованный объект - хотя мб хендлер указывает на объект в текущем домене
            var isProxy = RemotingServices.IsTransparentProxy(someInstance2);

            object someInstance3 = AppDomain.CurrentDomain.CreateInstanceAndUnwrap(
                "SomeAssembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                "SomeAssembly.SomeClass");
            isProxy = RemotingServices.IsTransparentProxy(someInstance3);

            object someInstance4 = someType.GetTypeInfo()
                .DeclaredConstructors
                .First()
                .Invoke(null);

            // создание экземпляра обощенного типа
            object o5 = Activator.CreateInstance(typeof(List<>).MakeGenericType(typeof(int)));
        }

        /// <summary> Вызов метода </summary>
        private static void Example4()
        {

        }

        /// <summary> Вызов метода, предлагаемый Рихтером </summary>
        private static void Example5()
        {

        }



        private static void Method()
        {
            types.Add(typeof(SomeClass));

            //Console.WriteLine("Методы класса SomeClass:" + typeof(SomeClass).GetMethods().Select(x => x.Name).Aggregate((n1,n2) => n1 += "; "  + n2));

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