namespace ClassicMemento
{
    public class Memento<TState>
    {
        // нарушение инкапсуляции объекта SomeOriginator: любой может прочитать внутреннее состояние объекта
        // Решение - сделать одну функцию восстановления "Restore" 
        public TState State { get; }

        public Memento(TState state)
        {
            State = state;
        }
    }
}