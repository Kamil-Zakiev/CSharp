using System.Threading;

namespace DoubleCheckLocking
{
    internal class Singleton
    {
        // закрытый конструктор, чтобы никто не мог создать экземпляр посредством new
        private Singleton() { }

        private static Singleton _singleton;

        private static readonly object LockObj = new object();

        public static Singleton GetSingleton()
        {
            // Проверка №1
            if (_singleton != null)
            {
                return _singleton;
            }

            Monitor.Enter(LockObj);
            // внутри этого блока поток имеет монопольный доступ

            // Проверка №2
            if (_singleton != null)
            {
                #region Пояснение
                // если второй поток оказался настолько шустрым, что успел взять блокировку, создать объект и вернуть блокировку 
                // после проверки №1, то второй раз создавать объект не нужно 
                #endregion
                Monitor.Exit(LockObj);
                return _singleton;
            }

            #region Пояснение
            // так как компилятор может сначала выделить память для объекта, а потом вызвать его конструктор,
            // то второй поток может получить невалидный объект, у которого конструктор не завершил работу.
            // Решение - волотильная запись, которая будет происходит только в момент вызова функции Volatile.Write
            // при этом гарантируется, что операция записи в переменную temp завершится до записи в _singleton 
            #endregion
            var temp = new Singleton();
            Volatile.Write(ref _singleton, temp);
            
            Monitor.Exit(LockObj);

            return _singleton;
        }
    }
}