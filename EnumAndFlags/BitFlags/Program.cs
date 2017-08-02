namespace BitFlags
{
    using System;

    [Flags]
    enum Goodness
    {
        None = 0,
        Car = 0x0001,
        Flat = 0x0002,
        Phone = 0x0004,
        Tablet = 0x0008,
        Guitar = 0x0010,
        Computer = 0x0020,
        Love = 0x0040,
        CarAndFlat = Car | Flat
    }

    class Program
    {
        static void Main(string[] args)
        {
        }

        /// <summary>
        /// Приведение к строкому типу
        /// </summary>
        private static void Example1()
        {
            var goods = Goodness.Flat | Goodness.Car | Goodness.Love;
            Console.WriteLine(goods);
            // output: CarAndFlat, Love
            // CarAndFlat, потому что битовые значения при приведении к типу строки сортируются по убыванию

            goods = (Goodness) 12;
            Console.WriteLine(goods);
            // output: Phone, Tablet


            goods = (Goodness)144;
            Console.WriteLine(goods);
            // output: 144
        }

        /// <summary> Пример проверки присутствия флага </summary>
        private static void Example2()
        {
            var someGoods = Goodness.Flat | Goodness.Car | Goodness.Love;
            
            if ((someGoods & Goodness.Love) == Goodness.Love)
            {
                Console.WriteLine("У человека есть любовь");
            }

            if (someGoods.Has(Goodness.Car))
            {
                Console.WriteLine("У человека есть машина");
            }

            if (!someGoods.Has(Goodness.Guitar))
            {
                Console.WriteLine("У человека нет гитары");
            }
        }
    }

    /// <summary> Пример добавления методов для перечисления при помощи методов расширения </summary>
    internal static class GoodnessEnumExtensions
    {
        public static bool Has(this Goodness goodness, Goodness someGood)
        {
            var has = (goodness & someGood) == someGood;
            return has;
        }
    }
}
