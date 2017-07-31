namespace ExplicitImplementation
{
    using System;

    internal interface IMyInterface
    {
        void MyMethod();
    }

    /// <summary>
    /// Класс, в котором новый метод воспринимается компилятором как реадизация метода интерфейса
    /// </summary>
    internal class MyClass1 : IMyInterface
    {
        public void MyMethod()
        {
            Console.WriteLine("MyClass1 implementation of IMyInterface");
        }
    }

    /// <summary>
    /// Класс, в котором метод интерфейса реализуется явно
    /// </summary>
    internal class MyClass2 : IMyInterface
    {
        void IMyInterface.MyMethod()
        {
            Console.WriteLine("MyClass2 EXPLICIT implementation of IMyInterface");
        }

        public void MyMethod()
        {
            Console.WriteLine("MyClass2 implementation of IMyInterface");
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            Example1();
            Example2();
        }

        /// <summary>
        ///     Метод класса и Интерфейсный метод имеют разные записи в таблице методов, но указывают на одну реализацию
        /// </summary>
        private static void Example1()
        {
            var m1 = new MyClass1();
            m1.MyMethod();
            // output: MyClass1 implementation of IMyInterface

            IMyInterface i1 = m1;
            i1.MyMethod();
            // output: MyClass1 implementation of IMyInterface
        }

        /// <summary>
        ///     <para>Метод класса и Интерфейсный метод имеют разные записи в таблице методов</para>
        ///     <para>И указывают на РАЗНЫЕ реализации</para>
        /// </summary>
        private static void Example2()
        {
            var m2 = new MyClass2();
            m2.MyMethod();
            // output: MyClass2 implementation of IMyInterface

            IMyInterface i2 = m2;
            i2.MyMethod();
            // output: MyClass2 EXPLICIT implementation of IMyInterface
        }
    }
}