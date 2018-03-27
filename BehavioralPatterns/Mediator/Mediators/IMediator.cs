using System.Collections.Generic;
using Mediator.Colleagues;

namespace Mediator.Mediators
{
    public interface IMediator<TColleague, TMessage>
        where TColleague : IColleague<TMessage>
    {
        IList<TColleague> Colleagues { get; }

        void NotifyColleagues(TMessage message);

        void AddColleague(IColleague<TMessage> colleague);
    }
}