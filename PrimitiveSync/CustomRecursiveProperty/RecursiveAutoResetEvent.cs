using System;
using System.Threading;

namespace CustomRecursiveProperty
{
    internal class RecursiveAutoResetEvent : IDisposable
    {
        private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
        private int _owningThreadId;
        private int _recursionCount;

        public void Dispose()
        {
            _autoResetEvent.Dispose();
        }

        public void Enter()
        {
            var currentThreadId = Thread.CurrentThread.ManagedThreadId;
            if (_owningThreadId == currentThreadId)
            {
                _recursionCount++;
                return;
            }

            _autoResetEvent.WaitOne();

            _recursionCount = 1;
            _owningThreadId = currentThreadId;
        }

        public void Leave()
        {
            var currentThreadId = Thread.CurrentThread.ManagedThreadId;
            if (currentThreadId != _owningThreadId)
            {
                throw new InvalidOperationException();
            }

            _recursionCount--;
            if (_recursionCount == 0)
            {
                _owningThreadId = 0;
                _autoResetEvent.Set();
            }
        }
    }
}