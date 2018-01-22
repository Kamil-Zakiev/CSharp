using System;
using System.Threading;

namespace InterlockedUsage
{
    internal sealed class AsyncCoordinator
    {
        private int m_opCount = 1;
        private int m_statusReported = 0;
        private Action<CoordinationStatus> m_callBack;
        private Timer m_timer;

        public void AboutToBegin(int opsToAdd = 1)
        {
            // Interlocked - как контроль над последовательным (потоки действуют на переменную друг за другом) увеличением значения m_opCount
            Interlocked.Add(ref m_opCount, opsToAdd);
        }

        public void JustEnded()
        {
            // Interlocked - как контроль над последовательным уменьшением значения m_opCount
            var opCount = Interlocked.Decrement(ref m_opCount);
            if (opCount == 0)
            {
                ReportStatus(CoordinationStatus.AllDone);
            }
        }

        public void AllBegun(Action<CoordinationStatus> callBack, int timeOut = Timeout.Infinite)
        {
            m_callBack = callBack;
            if (timeOut != Timeout.Infinite)
            {
                m_timer = new Timer(TimeExpired, null, timeOut, Timeout.Infinite);                
            }
            JustEnded();
        }

        private void TimeExpired(object state)
        {
            ReportStatus(CoordinationStatus.Timeout);
        }

        public void Cancel()
        {
            ReportStatus(CoordinationStatus.Cancel);
        }

        private void ReportStatus(CoordinationStatus status)
        {
            var oldValue = Interlocked.Exchange(ref m_statusReported, 1);
            if (oldValue == 0)
            {
                // Interlocked как арбитр:
                // гарантируется, что метод обратного вызова сработает только один раз, 
                // т.к. только один поток переведет m_statusReported из 0 в 1
                m_callBack(status);
            }
        }
    }
}