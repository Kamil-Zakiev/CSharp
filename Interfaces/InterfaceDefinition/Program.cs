namespace InterfaceDefinition
{
    using System;

    /// <summary>     Пример определения интерфейса </summary>
    public interface IMyInterface1
    {
        /// <summary> Определение свойства </summary>
        long Id { get; set; }

        /// <summary> Определение метода </summary>
        void Method();

        /// <summary> Определение события </summary>
        event Action<string> WaitingForMessage;
    }

    class MyClass : IMyInterface1
    {
        public long Id { get; set; }
        public void Method()
        {
            WaitingForMessage?.Invoke("");
        }

        public event Action<string> WaitingForMessage;
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            IMyInterface1 my = new MyClass();
            
        }
    }
}