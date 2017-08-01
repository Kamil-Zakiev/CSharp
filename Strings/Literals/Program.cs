namespace Literals
{
    using System;
    using System.IO;
    using System.Linq;

    internal class Program
    {
        private static void Main(string[] args)
        {
            Example1();
            Example2();
            Example3();
        }

        /// <summary>
        ///     Компилятор помещает строку "asd" в метаданные и далее использует ссылку на этот объект.
        /// </summary>
        private static void Example1()
        {
            var s1 = "asd";
            var s2 = "asd";

            Console.WriteLine(ReferenceEquals(s1, s2));
            // output: true
        }

        /// <summary>
        ///     При обычном чтении строк из файла интернирования не происходит, даже без указания атрибутов
        /// </summary>
        private static void Example2()
        {
            var lines = File.ReadAllLines(@"G:\CSharp\Strings\Literals\TextFile.txt");
            var firstLine = lines.First();
            var words = firstLine.Split(' ');

            var s1 = words[0];
            var s2 = words[1];
            Console.WriteLine("\"" + s1 + "\"");
            Console.WriteLine("\"" + s2 + "\"");
            Console.WriteLine(ReferenceEquals(s1, s2));
            // output: false
        }


        /// <summary>
        ///     Явное интернирование
        /// </summary>
        private static void Example3()
        {
            var lines = File.ReadAllLines(@"G:\CSharp\Strings\Literals\TextFile.txt");
            var firstLine = lines.First();
            var words = firstLine.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = String.Intern(words[i]);
            }

            var s1 = words[0];
            var s2 = words[1];
            Console.WriteLine("\"" + s1 + "\"");
            Console.WriteLine("\"" + s2 + "\"");
            Console.WriteLine(ReferenceEquals(s1, s2));
            // output: true
        }
    }
}