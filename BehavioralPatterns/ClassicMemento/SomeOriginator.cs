namespace ClassicMemento
{
    public class SomeOriginator
    {
        public State State { get; set; }

        public Memento<State> CreateMemento()
        {
            var copyState = new State
            {
                FirstParameter = State.FirstParameter,
                SecondParameter = State.SecondParameter
            };
            return new Memento<State>(copyState);
        }

        public void SetMemento(Memento<State> memento)
        {
            State = memento.State;
        }
    }
}