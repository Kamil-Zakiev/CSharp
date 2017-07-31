namespace InterfaceMethodCall
{
    using System;

    interface IMyInterface
    {
        void Method();
    }

    internal class MyBaseClass : IMyInterface
    {
        public void Method()
        {
            Console.WriteLine("Base class implementation");
        }
    }

    internal class MyDerivedClass : MyBaseClass
    {
    }

    internal class MyDerivedClass2 : MyBaseClass, IMyInterface
    {
        public void Method()
        {
            Console.WriteLine("MyDerivedClass2 implementation");
        }
    }
    
    internal class Program
    {
        private static void Main(string[] args)
        {

            Example3();
        }

        /// <summary> Вызов интерфейсного метода с помощью интерфейсной переменной </summary>
        private static void Example1()
        {
            IMyInterface b = new MyBaseClass();
            b.Method();
            // output: Base class implementation

            IMyInterface d = new MyDerivedClass();
            d.Method();
            // output: Base class implementation
        }

        /// <summary> Вызов интерфейсного метода с помощью интерфейсной переменной </summary>
        private static void Example2()
        {
            IMyInterface b = new MyBaseClass();
            b.Method();
            // output: Base class implementation

            IMyInterface d = new MyDerivedClass2();
            d.Method();
            // output: MyDerivedClass2 implementation
        }

        /// <summary> Вызов интерфейсного метода с помощью переменной базового типа и интерфейсоной переменной</summary>
        private static void Example3()
        {
            MyDerivedClass2 d = new MyDerivedClass2();

            MyBaseClass b = d;
            Console.WriteLine(b == d); // True - экземпляры тождественны
            b.Method();
            Console.WriteLine(b.GetType());
            // output: Base class implementation
            //         InterfaceMethodCall.MyDerivedClass2

            ((IMyInterface)b).Method();
            // output: MyDerivedClass2 implementation

            // для вызова интерфейсного метода CLR просматривает таблицу методов объекта-типа
        }
    }
}