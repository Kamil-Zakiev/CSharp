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