namespace ProxyTypes
{
    public interface IFreezable
    {
        bool IsFrozen { get; }

        void Freeze();
    }
}