using System;
using System.Threading;

namespace InterprocessExample
{
    internal class Program
    {
        /// <summary>
        ///     При запуске exe-файла происходит обращение к объектам ядра - на самом деле к одному и тому же объекту, поскольку
        ///     имя одно и то же
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            bool createdNew;
            // Пытаемся создать объект ядра с указанным именем     
            using (new Semaphore(0, 1, "SomeUniqueStringIdentifyingMyApp", out createdNew))
            {
                if (createdNew)
                {
                    Console.WriteLine("Запуск приложения успешен.");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("Запускается повторно. Отмена.");
                Console.ReadKey();
            }
        }
    }
}