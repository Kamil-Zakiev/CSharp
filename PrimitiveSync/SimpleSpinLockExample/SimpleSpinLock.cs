using System.Threading;

namespace SimpleSpinLockExample
{
    internal struct SimpleSpinLock
    {
        private int m_resourceIsInUse;

        public int idleCount;

        public void Enter()
        {
            while (true)
            {
                var prevValue = Interlocked.Exchange(ref m_resourceIsInUse, 1);
                if (prevValue == 0)
                {
                    return;
                }

                // тут можно схитрожопить и задержать поток
                // Thread.Yield(); // <- попросить Windows запланировать для текущего процессора другой поток на след. такт
                // Thread.Sleep(0); // <- заствляет запланировать другой поток для исполнения ( потокам с низким приоритетом не дают исполняться)
                // Thread.Sleep(1); // <- включает принудительное переключение контекста
                // Thread.SpinWait(50); // <- про гиперпотоковые процессоры... (используется в SpinLock из FCL)
                Interlocked.Increment(ref idleCount);
            }
        }

        public void Leave()
        {
            // тут можно и просто " = 0", тем более для Int32 гарантирована атомарная запись
            // но, видимо, каждый дрочит как хочет
            // Volatile.Write(ref m_resourceIsInUse, 0);
            m_resourceIsInUse = 0;
        }
    }
}