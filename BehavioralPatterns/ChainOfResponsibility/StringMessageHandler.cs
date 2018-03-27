using System;

namespace ChainOfResponsibility
{
    public class StringMessageHandler : IHandler<string>
    {
        private readonly Func<string, bool> _hasToTrigger;

        private IHandler<string> _nextHandler;

        public StringMessageHandler(Func<string, bool> hasToTrigger)
        {
            _hasToTrigger = hasToTrigger;
        }

        private int Id => GetHashCode();

        public void SetNextHandler(IHandler<string> nextHandler)
        {
            _nextHandler = nextHandler;
        }

        public void HandleMessage(string message)
        {
            if (_hasToTrigger(message))
            {
                Console.WriteLine("Component with Id = " + Id + " has handled message " + message);
                return;
            }

            _nextHandler?.HandleMessage(message);
        }
    }
}