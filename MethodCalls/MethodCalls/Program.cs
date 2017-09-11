using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodCalls
{
    public class MyClass
    {
        public virtual void Method()
        {
            Console.WriteLine("MyClass.Method()");
        }
    }

    public class Derived : MyClass
    {
        public override void Method()
        {
            Console.WriteLine("Derived.Method()");
        }
    }

    public static class MyClassExtensions
    {
        public static void Method(this MyClass myClass)
        {
            Console.WriteLine("MyClassExtensions.Method()");
        }
    }

    public interface IInterface
    {

    }

    public class MyClass2 : IInterface
    {
        public virtual void Method()
        {
            Console.WriteLine("MyClass.Method()");
        }
    }
    
    public static class IInterfaceExtensions
    {
        public static void Method(this IInterface myClass2)
        {
            Console.WriteLine("IInterfaceExtensions.Method()");
        }
    }

    class Program
    {
        /// <summary>
        /// При наличии экстеншн-метода и экземплярного метода выбирается экземплярный метод
        /// Однако экстеншн-метод можно запустить явно
        /// </summary>
        /// <param name="args"></param>
        static void Example1(string[] args)
        {
            var obj = new Derived();
            obj.Method();
            // output: Derived.Method()

            MyClassExtensions.Method(obj);
            // output: MyClassExtensions.Method()
        }

        /// <summary>
        /// Какой метод можно вызвать - решает тип переменной, 
        /// если есть явный метод, то вызывается он,
        ///  иначе - на этапе компиляции вставляется вызов экстеншена
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            IInterface m = new MyClass2();
            m.Method();
            // output: IInterfaceExtensions.Method()
        }
    }
}
