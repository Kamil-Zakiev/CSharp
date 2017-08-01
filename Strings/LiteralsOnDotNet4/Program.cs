namespace Literals
{
    using System;
    using System.Runtime.CompilerServices;

    internal class Program
    {
        private static void Main(string[] args)
        {
            Example();
        }

        /// <summary>
        ///     Компилятор помещает строку "asd" в метаданные и далее использует ссылку. Происходит интернирование, хоть я и
        ///     повесил атрибут и на сборку, и на метод
        /// </summary>
        [CompilationRelaxations(CompilationRelaxations.NoStringInterning)]
        private static void Example()
        {
            string s1 = "asd";
            string s2 = "asd";

            Console.WriteLine(ReferenceEquals(s1, s2));
            // output: true
        }
    }
}