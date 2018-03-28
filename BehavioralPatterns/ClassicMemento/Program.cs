using System;

namespace ClassicMemento
{
    internal static class Program
    {
        public static void Main()
        {
            var someOriginator = new SomeOriginator()
            {
                State = new State()
                {
                    FirstParameter = "123",
                    SecondParameter = "456"
                }
            };

            DoSomeWork(someOriginator);

            Console.WriteLine(someOriginator.State.FirstParameter);
            Console.WriteLine(someOriginator.State.SecondParameter);
        }

        private static void DoSomeWork(SomeOriginator someOriginator)
        {
            var caretaker = new Caretaker<Memento<State>>()
            {
                Memento = someOriginator.CreateMemento()
            };

            try
            {
                someOriginator.State.FirstParameter = "modified";
                someOriginator.State.SecondParameter = "modified";
                
                throw new Exception("...Something was wrong...");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                someOriginator.SetMemento(caretaker.Memento);
            }
        }
    }
}