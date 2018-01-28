using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace NoBlocking
{
    internal class Singleton
    {
        private static int IdCount;
        private int id;

        // закрытый конструктор, чтобы никто не мог создать экземпл€р последством new
        private Singleton()
        {
            id = ++IdCount;
        }

        ~Singleton()
        {
            Console.WriteLine("Finilizing Singleton: " + id);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Method()
        {
            
        }

        private static Singleton _singleton = new Singleton();
        
        public static Singleton GetSingleton()
        {
            if (_singleton != null)
            {
                return _singleton;
            }

            var temp = new Singleton();
            Interlocked.CompareExchange(ref _singleton, temp, null);

            // другие создавшиес€ объекты singleton будут уничтожены сборщиком мусора
            return _singleton;
        }
    }
}