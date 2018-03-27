namespace ChainOfResponsibility
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var chainOrResponsibility = GetChainOfResponsibility();

            chainOrResponsibility.HandleMessage("small");
            chainOrResponsibility.HandleMessage("medium text");
            chainOrResponsibility.HandleMessage("very large text text text");
        }

        private static IHandler<string> GetChainOfResponsibility()
        {
            // smallMessageHandler, mediumMessageHandler, largeMessageHandler - группа объектов, каждый их которых должен обработать сообщение
            IHandler<string> smallMessageHandler = new StringMessageHandler(message => message.Length < 10);
            IHandler<string> mediumMessageHandler =
                new StringMessageHandler(message => message.Length >= 10 && message.Length < 20);
            IHandler<string> largeMessageHandler = new StringMessageHandler(message => message.Length >= 20);

            // создаем из объектов цепочку
            smallMessageHandler.SetNextHandler(mediumMessageHandler);
            mediumMessageHandler.SetNextHandler(largeMessageHandler);

            // получаем цепочку
            var chainOrResponsibility = smallMessageHandler;
            return chainOrResponsibility;
        }
    }
}