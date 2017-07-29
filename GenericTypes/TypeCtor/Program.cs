namespace TypeCtor
{
    using System;

    public class GenericClass<T>
    {
        private T _field;

        static GenericClass()
        {
            Console.WriteLine("GenericStaticCtor: Class was closed with type " + typeof(T).FullName);
        }
    }

    public class A : GenericClass<A>
    {
        static A()
        {
            Console.WriteLine("static A ctor");
        }

        public A()
        {
            Console.WriteLine("instance .ctor A");
        }

        public static void Method()
        {
            Console.WriteLine("static method in class A");
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            // стат. конструктор выполняется сразу после использования типа: создание экземплярв типа, вызов стат методов
            InstanceObject();
            StaticMethodCall();
            TypeMetaUsing();
        }

        private static void InstanceObject()
        {
            Console.WriteLine("InstanceObject:");
            var a = new A();
            // static A ctor
            // GenericStaticCtor: Class was closed with type TypeCtor.A
            // instance.ctor A

            Console.WriteLine();
        }

        private static void StaticMethodCall()
        {
            Console.WriteLine("StaticMethodCall:");
            A.Method();
            // static A ctor
            // static method in class A

            Console.WriteLine();
        }

        private static void TypeMetaUsing()
        {
            Console.WriteLine("TypeMetaUsing:");
            Console.WriteLine(typeof(A));
            // TypeCtor.A

            Console.WriteLine();
        }
    }
}