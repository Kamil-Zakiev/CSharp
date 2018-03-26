using System;

namespace ChainOfResponsibility
{
    public class StringMessageHandler:IHandler<string>
    {
        private int Id => this.GetHashCode();
        
        private IHandler<string> _nextHandler;

        private Func<string, bool> _hasToTrigger;

        public StringMessageHandler(Func<string, bool> hasToTrigger)
        {
            _hasToTrigger = hasToTrigger;
        }
        
        public void SetNextHandler(IHandler<string> nextHandler)
        {
            _nextHandler = nextHandler;
        }

        public void HandleMessage(string message)
        {
            if (_hasToTrigger(message))
            {
                Console.WriteLine("Component with Id = "+ Id +" has handled message " + message);
                return;
            }

            _nextHandler?.HandleMessage(message);
        }
    }
}