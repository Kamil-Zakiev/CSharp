using System.Collections.Generic;
using Mediator.Colleagues;

namespace Mediator.Mediators
{
    internal class Mediator : IMediator<IColleague<Message>, Message>
    {
        public IList<IColleague<Message>> Colleagues { get; } = new List<IColleague<Message>>();

        public void NotifyColleagues(Message message)
        {
            foreach (var colleague in Colleagues)
            {
                if (colleague == message.Sender) continue;

                colleague.Notify(message);
            }
        }

        public void AddColleague(IColleague<Message> colleague)
        {
            Colleagues.Add(colleague);
        }
    }
}