using System;
using System.Threading;

namespace PassCallbacks
{
    class Program
    {
        delegate String GetString();

        static void Method1(GetString stringGetter)
        {
            var str = stringGetter();
            Console.WriteLine(str);
        }

        static string GetMyString()
        {
            return "my  string!";
        }

        /// <summary>
        /// Пример передачи экземпляра делегата для осуществления обратного вызова
        /// </summary>
        static void Main(string[] args)
        {
            // три вызова ниже идентичны благодаря компилятору
            Method1(new GetString(Program.GetMyString));
            Method1(Program.GetMyString);
            Method1(GetMyString);

            Method1(() => "lamba as callback!");

            var str = "closure str";
            Method1(() =>
            {
                str += "asdasdasd";
                return str;
            });

            Thread.Sleep(1000);
            // изменение строки внутри лямбды неявно сказывается на внешней переменной - это хоть и ожидаемо, но не очень хорошо, 
            // вывод - если передаем переменные в замыкании, то внутри лямбы их не менять незя
            Console.WriteLine(str);

        }
    }
}
