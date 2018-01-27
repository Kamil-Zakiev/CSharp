using System;
using System.Threading;
using MultithreadUtils;

namespace SimpleHybridLock
{
    internal class SimpleHybridLock : IDisposable
    {
        private int _waitersCount = 0;

        private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(false);

        public void Enter()
        {
            var currentThreadId = Thread.CurrentThread.ManagedThreadId;
            Logger.Log(currentThreadId, "entered");
            // используем конструкцию пользовательского режима для определения необходимости блокировки потока конструкцией ядра
            var actualWaitersCount = Interlocked.Increment(ref _waitersCount);
            if (actualWaitersCount == 1)
            {
                // поток получает право выполняться дальше
                Logger.Log(currentThreadId, "got a green ligth");
                return;
            }

            // блокируем поток конструкцией ядра
            Logger.Log(currentThreadId, "got a red ligth and will use kernel costruction");
            _autoResetEvent.WaitOne();
            Logger.Log(currentThreadId, "got a green ligth after waiting");
        }

        public void Leave()
        {
            var currentThreadId = Thread.CurrentThread.ManagedThreadId;
            Logger.Log(currentThreadId, "is leaving");
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