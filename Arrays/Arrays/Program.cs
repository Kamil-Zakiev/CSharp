using System;
using System.Collections;
using System.Collections.Generic;

namespace Arrays
{
    class MyEnumerable : IEnumerable<int>
    {
        public IEnumerator<int> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }


    internal class Program
    {
        private static void Method(IEnumerable enumerable)
        {

        }
        private static void Method2(IEnumerable<object> enumerable)
        {

        }
        
        /// <summary>
        /// Нельзя передавать массив структур в метод, 
        /// в котором ожидает обобщенный интерфейс коллекции элементов бахового типа элементов массива.
        /// CLR не заморачивается по этому поводу для значимых типов
        /// </summary>
        private static void Example1()
        {
            var listOfStruct = new DateTime[100];

            Method(listOfStruct);

            Method2(listOfStruct);
        }
        private static void Main()
        {
            Example1();
        }
    }
}