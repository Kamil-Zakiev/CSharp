using System;
using System.Threading;
using MultithreadUtils;

namespace SmartHybridLock
{
    internal class SmartHybridLock : IDisposable
    {
        private int _waitersCount = 0;
        private int _owningThreadId = 0;
        private int _recursiveCount = 0;

        private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(false);

        public void Enter()
        {
            var currentThreadId = Thread.CurrentThread.ManagedThreadId;
            Logger.Log(currentThreadId, "entered");

            // если поток пытается повторно завладеть блокирвкой просто увеличиваем счетчик рекурсии
            if (currentThreadId == _owningThreadId)
            {
                Logger.Log(currentThreadId, "got a green light again");
                _recursiveCount++;
                return;
            }

            // зацикливаем поток конструкцией пользовательского режима
            var spinWait = new SpinWait();
            Logger.Log(currentThreadId, "is waiting in user mode cycle...");
            for (int i = 0; i < 4000; i++)
            {
                var previousWaitersCount = Interlocked.CompareExchange(ref _waitersCount, 1, 0);
                if (previousWaitersCount == 0)
                {
                    // блокирующий поток освободил блокировку, значит можем 
                    // получает право выполняться дальше тот поток, 
                    // который сменил количество ожидающих поток с 0 на 1, а не с 1 на 1
                    Logger.Log(currentThreadId, "got a green ligth and won't use a kernel construction");
                    _owningThreadId = currentThreadId;
                    _recursiveCount = 1;
                    return;
                }

                // переключение контекста: даем шанс другим потокам выполниться и освободить блокировку
                spinWait.SpinOnce();
            }
            
            var actualWaitersCount = Interlocked.Increment(ref _waitersCount);
            if (actualWaitersCount == 1)
            {
                _owningThreadId = currentThreadId;
                _recursiveCount = 1;
                return;
            }

            // блокируем поток конструкцией ядра
            Logger.Log(currentThreadId, "has waited but got a red ligth and will use kernel costruction");
            _autoResetEvent.WaitOne();
            Logger.Log(currentThreadId, "got a green ligth after waiting");
        }

        public void Leave()
        {
            var currentThreadId = Thread.CurrentThread.ManagedThreadId;
            if (currentThreadId != _owningThreadId)
            {
                throw new SynchronizationLockException();
            }

            Logger.Log(currentThreadId, "is leaving");
            _recursiveCount--;
            if (_recursiveCount != 0)
            {
                return;
            }

            // уменьшаем количество ожидающих потоков 
            // (при правильном использовании гибридной конструкции прибегать к атомарному декременту необязательно)
            _waitersCount--;
            if (_waitersCount == 0)
            {
                // если ожидающих потоков нет, то конструкция ядра не использовалась и мы просто возвращаем управление
                Logger.Log(currentThreadId, "has left without using kernel construction");
                return;
            }

            // используя конструкция режима ядра, даем продолжить работу одному другому потоку
            _autoResetEvent.Set();
            Logger.Log(currentThreadId, "has left using kernel construction");
        }
        
        public void Dispose()
        {
            _autoResetEvent.Dispose();
        }
    }
}