using System;
using System.Collections.Generic;

namespace MemoryLeak
{
    internal class Program
    {
        private static void Method(Sender sender)
        {
            var listOfSubscribers = new List<Subscriber>
            {
                new Subscriber(),
                new Subscriber(),
                new Subscriber(),
                new Subscriber(),
                new Subscriber()
            };

            foreach (var subscriber in listOfSubscribers)
            {
                subscriber.StartListenTo(sender);
            }

            sender.SayWord("hello!");
        }

        private static void Main(string[] args)
        {
            var sender = new Sender();
            Method(sender);

            Console.WriteLine("GC started");
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // финализаторы подписчиков не вызвались => sender их удерживает (событие на делегатах, все-таки)

            Console.WriteLine("GC finished");
        } // финализаторы вызвались
    }
}