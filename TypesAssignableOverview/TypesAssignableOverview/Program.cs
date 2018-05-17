namespace TypesAssignableOverview
{
    using System;
    using System.Linq;

    public interface ISomeInterface
    {
        
    }
    
    public interface ISomeGenericInterface<T> where T: new()
    {
        
    }

    class SomeClass: ISomeInterface
    {
        
    }

    class SomeOpenGenericClass<T>: ISomeGenericInterface<T>, ISomeInterface where T: new()
    {
        
    }
    
    class SomeClosedGenericClass: ISomeGenericInterface<object>, ISomeInterface
    {
        
    }

    class Derived : SomeClosedGenericClass
    {
        
    }
    
    internal class Program
    {
        public static void Main(string[] args)
        {
            
        }

        /// <summary> Список интерфейсов, включая унаследованные </summary>
        public static void Example1()
        {
            var type = typeof(Derived);
            var interfaces = type.GetInterfaces();
            Console.WriteLine("Interfaces of " + type.Name);
            foreach (var interfaceType in interfaces)
            {
                Console.WriteLine(interfaceType.FullName);
            }
        }

        /// <summary>
        /// IsGenericType = тип имеет отношение к дженерикам?
        /// IsGenericTypeDefinition = тип открытый?
        /// IsConstructedGenericType = тип закрытый?
        /// </summary>
        public static void Example2()
        {
            var type = typeof(ISomeGenericInterface<>);
            
            Console.WriteLine($"{type.Name}.IsGenericType = {type.IsGenericType}");
            Console.WriteLine($"{type.Name}.IsGenericTypeDefinition = {type.IsGenericTypeDefinition}");
            Console.WriteLine($"{type.Name}.IsConstructedGenericType = {type.IsConstructedGenericType}");
            
            type = typeof(ISomeGenericInterface<object>);
            
            Console.WriteLine($"{type.Name}.IsGenericType = {type.IsGenericType}");
            Console.WriteLine($"{type.Name}.IsGenericTypeDefinition = {type.IsGenericTypeDefinition}");
            Console.WriteLine($"{type.Name}.IsConstructedGenericType = {type.IsConstructedGenericType}");
        }

        /// <summary> Получение аргументов-параметров </summary>
        public static void Example3()
        {
            var type = typeof(ISomeGenericInterface<>);

            var t1 = type.GetGenericArguments().First();
            var t = t1.GetGenericParameterConstraints();

            Console.WriteLine($"{type.Name}.IsGenericParameter = {t1.IsGenericParameter}");
        }
    }
}