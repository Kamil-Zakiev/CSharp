using System;
using System.Threading;

namespace MemoryLeak
{
    internal class Sender
    {
        public event Action<string> WordIsSaid;

        public void SayWord(string word)
        {
            Console.WriteLine("Sender: " + word);
            OnWordSaid(word);
        }

        protected virtual void OnWordSaid(string word)
        {
            Volatile.Read(ref WordIsSaid)?.Invoke(word);
        }
    }
}