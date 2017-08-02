namespace Enums
{
    using System;

    internal enum Color : byte
    {
        Green, // по умолчанию = 0
        Pink, // по умолчанию = 1
        Black, // по умолчанию = 2
        White // по умолчанию = 3
    }

    class Program
    {
        static void Main(string[] args)
        {
            Example4();
        }

        /// <summary> Получение типа, лежащего в основе перечисления </summary>
        private static void Example1()
        {
            var baseType = typeof(Color).GetEnumUnderlyingType();
            Console.WriteLine(baseType);
        }

        /// <summary> Различные преобразования перечисления в строку  </summary>
        private static void Example2()
        {
            var color = Color.Pink;
    
            Console.WriteLine(color.ToString());
            // output: Pink

            Console.WriteLine(color.ToString("G"));
            // output: Pink

            Console.WriteLine(color.ToString("D"));
            // output: 1

            Console.WriteLine(color.ToString("X"));
            // output: 01
        }

        /// <summary> Получение идентификаторов перечисления </summary>
        private static void Example3()
        {
            var name = typeof(Color).GetEnumName(1);
            Console.WriteLine(name);
            // output: Pink

            var names = Enum.GetNames(typeof(Color));
            Console.WriteLine(string.Join(", ", names));
            // output: Green, Pink, Black, White
        }

        /// <summary> Парсинг строки для получения значения перечисления </summary>
        private static void Example4()
        {
            var color = Enum.Parse(typeof(Color), "Black");
            Console.WriteLine(color);
            // output: Black

            color = Enum.Parse(typeof(Color), "black");
            Console.WriteLine(color);
            // output: Необработанное исключение: System.ArgumentException: Запрошенное значение "black" не найдено.

            // также есть TryParse
        }
    }
}
