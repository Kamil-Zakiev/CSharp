using Mediator.Mediators;

namespace Mediator.Colleagues
{
    public abstract class AbstractColleague : IColleague<Message>
    {
        protected AbstractColleague(IMediator<IColleague<Message>, Message> mediator, string name)
        {
            Mediator = mediator;
            Name = name;
            Mediator.AddColleague(this);
        }

        private IMediator<IColleague<Message>, Message> Mediator { get; }

        public string Name { get; }

        public void SendMessage(Message message)
        {
            message.Sender = this;
            Mediator.NotifyColleagues(message);
        }

        public abstract void DoWork();

        public abstract void Notify(Message message);
    }
}