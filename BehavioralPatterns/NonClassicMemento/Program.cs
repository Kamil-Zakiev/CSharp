using System;

namespace NonClassicMemento
{
    public class Caretaker
    {
        public Memento Memento;
    }
    
    public abstract class Memento
    {
        public abstract void RestoreState();
    }

    public class State
    {
        public string First { get; set; }
        public string Second { get; set; }
    }

    public class Originator
    {
        private class PrivateMemento:Memento
        {
            private State _state;

            private Originator _originator;

            public PrivateMemento(State state, Originator originator)
            {
                _originator = originator;
                _state = state;
            }

            public override void RestoreState()
            {
                _originator.State = _state;
            }
        }
        
        public State State;

        public Memento CreateMemento()
        {
            var stateCopy = new State
            {
                First = State.First,
                Second = State.Second
            };
            return new PrivateMemento(stateCopy, this);
        }
    }
    
    internal class Program
    {
        public static void Main(string[] args)
        {
            var origin = new Originator
            {
                State = new State
                {
                    First = "123",
                    Second = "456"
                }
            };

            var caretaker = new Caretaker {Memento = origin.CreateMemento()};

            Print(origin);
            origin.State.First = "qwe";
            origin.State.Second = "qwe";
            Print(origin);
            
            caretaker.Memento.RestoreState();
            Print(origin);

        }

        private static void Print(Originator origin)
        {
            Console.WriteLine(origin.State.First);
            Console.WriteLine(origin.State.Second);
        }
    }
}