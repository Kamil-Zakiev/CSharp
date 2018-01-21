using System;
using System.Threading;

namespace VolatileUsage2
{
    internal sealed class ThreadsSharingData
    {
        private int m_flag = 0;
        private int m_value = 0;

        public void Thread1()
        {
            m_value = 5;
            m_flag = 1;
        }

        public void Thread2()
        {
            if (m_flag == 1)
            {
                Console.WriteLine(m_value);
            }
        }
    }

    internal class Program
    {
        /// <summary>
        ///     Здесь теоретически возможны косяки с {переключением между потоками}/{паралелльным выполнением потоков}
        ///     Проблемы решаются волатильным чтением/записью в конце/в начале инструкций
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            var tsd = new ThreadsSharingData();
            new Thread(tsd.Thread2).Start();
            new Thread(tsd.Thread1).Start();
        }
    }
}