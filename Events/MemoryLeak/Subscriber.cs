using System;

namespace MemoryLeak
{
    internal class Subscriber
    {
        private static int Number;

        private readonly string _name;

        public Subscriber()
        {
            Number++;
            _name = Number.ToString();
        }

        ~Subscriber()
        {
            Console.WriteLine($"Subscriber{_name} was destroyed.");
        }

        private void OnSenderOnWordIsSaid(string s)
        {
            Console.WriteLine($"Subscriber{_name} received \"{s}\"");
        }

        public void StartListenTo(Sender sender)
        {
            sender.WordIsSaid += OnSenderOnWordIsSaid;
        }
    }
}