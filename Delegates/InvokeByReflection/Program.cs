using System;
using System.Reflection;

namespace InvokeByReflection
{
    internal delegate void MyDelegate(int n);

    internal struct Program
    {
        /// <summary>
        ///     При помощи Delegate.DynamicInvoke можно вызвать обратный метод,
        ///     но если аргумент - экз. значимого типа - будет упаковка
        /// </summary>
        private static void Example1()
        {
            Action<int> action = n => Console.WriteLine(n);
            Delegate del = action;
            del.DynamicInvoke();
            // public object DynamicInvoke(params object[] args)
        }

        private static void Method(int n)
        {
            Console.WriteLine(n);
        }

        /// <summary>
        ///     Создание экземпляра делегата - обертки для стат. функции - и последующий вызов с помощью механизма отражения
        /// </summary>
        private static void Example2()
        {
            var delType = Type.GetType("InvokeByReflection.MyDelegate");
            var mi = typeof(Program).GetTypeInfo().GetDeclaredMethod("Method");

            // создание обертки-делегата для функции
            var deleg = mi.CreateDelegate(delType);
            deleg.DynamicInvoke(64);
        }


        private static void Main(string[] args)
        {
            Example2();
        }
    }
}