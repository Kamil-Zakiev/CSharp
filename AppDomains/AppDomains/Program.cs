using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Remoting;
using System.Threading;
using AppDomains.Entities;

namespace AppDomains
{
    internal class Program
    {
        /// <summary>
        ///     Создание объекта в другом домене; доступ к объекту с помощью продвижения по ссылке
        /// </summary>
        private static void Example1()
        {
            var adCallingThreadDomain = Thread.GetDomain();
            var callingDomainName = adCallingThreadDomain.FriendlyName;
            Console.WriteLine($"Default AppDomain's friendly name = \"{callingDomainName}\"");

            var exeAssemblyName = Assembly.GetEntryAssembly().FullName;
            Console.WriteLine($"Main asssembly name: \"{exeAssemblyName}\"");

            var ad2 = AppDomain.CreateDomain("AD number 2", null, null);

            // получаем ссылку на представитель
            var marshalByRefType =
                (MarshalByRefType) ad2.CreateInstanceAndUnwrap(exeAssemblyName, typeof(MarshalByRefType).FullName);

            Console.WriteLine("Type={0}", marshalByRefType.GetType());
            Console.WriteLine("Is proxy={0}", RemotingServices.IsTransparentProxy(marshalByRefType));

            marshalByRefType.SomeMethod();

            AppDomain.Unload(ad2);

            try
            {
                // Поскольку домен приложений выгружен, появляется исключение
                marshalByRefType.SomeMethod();
                Console.WriteLine("Successful call.");
            }
            catch (AppDomainUnloadedException)
            {
                Console.WriteLine("Failed call.");
            }
        }

        /// <summary>
        ///     Получение доступа к объекту в другом домене через механизм продвижения по значению
        /// </summary>
        private static void Example2()
        {
            var ad2 = AppDomain.CreateDomain("AD #2", null, null);
            var exeAssemblyName = Assembly.GetEntryAssembly().FullName;
            var mbrt =
                (MarshalByRefType) ad2.CreateInstanceAndUnwrap(exeAssemblyName, typeof(MarshalByRefType).FullName);

            // получение копии объекта, т.к. продвижение по значению
            var mbvt = mbrt.MethodWithReturn();

            // проверка, что не прокси, а копия
            Console.WriteLine("Is proxy={0}", RemotingServices.IsTransparentProxy(mbvt));

            Console.WriteLine("Returned object created " + mbvt);
            AppDomain.Unload(ad2);

            Console.WriteLine("Returned object created " + mbvt);
        }

        /// <summary>
        ///     Невозможность продвижения возвращаемого значения
        /// </summary>
        private static void Example3()
        {
            var ad2 = AppDomain.CreateDomain("AD #2", null, null);
            var exeAssemblyName = Assembly.GetEntryAssembly().FullName;
            var mbrt =
                (MarshalByRefType) ad2.CreateInstanceAndUnwrap(exeAssemblyName, typeof(MarshalByRefType).FullName);

            // выбросится эксепшен: продвижение возвращаемого объекта невозможно
            // В домен передаем строку - на самом деле копия не создается, потому что строка - неизменяемый тип
            mbrt.MethodArgAndReturn(Thread.GetDomain().FriendlyName);
        }

        private static void Main(string[] args)
        {
            Example4();
        }

        /// <summary> Обращение к полям реального объекта через прокси-представитель — дорогостоящая вещь </summary>
        private static void Example4()
        {
            var byRef = new MarshalByRefType();
            var common = new MyClass();

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            const int V = 100000000;
            for (var i = 0; i < V; i++)
            {
                common.k++;
            }
            stopWatch.Stop();
            Console.WriteLine("Common: elapsed {0} ms", stopWatch.ElapsedMilliseconds);
            stopWatch.Restart();
            for (var i = 0; i < V; i++)
            {
                byRef.k++;
            }
            stopWatch.Stop();
            Console.WriteLine("ByRef: elapsed {0} ms", stopWatch.ElapsedMilliseconds);
        }


        private class MyClass
        {
            public int k;
        }
    }
}