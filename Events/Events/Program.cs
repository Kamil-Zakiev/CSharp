using System;
using System.Collections.Generic;

namespace SimpleExample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var sender = new Sender();
            NewMethod(sender);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine("End of Main");
        }

        private static void NewMethod(Sender sender)
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


            foreach (var subscriber in listOfSubscribers)
            {
                subscriber.StopListenTo(sender);
            }
        }
    }
}