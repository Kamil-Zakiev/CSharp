namespace Iterator
{
    public interface IIterator
    {
        object Current { get; }
        bool MoveNext();
    }
}
