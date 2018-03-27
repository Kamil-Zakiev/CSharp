namespace Mediator.Colleagues
{
    public interface IColleague<TMessage>
    {
        string Name { get; }

        void SendMessage(TMessage message);

        void DoWork();

        void Notify(TMessage message);
    }
}