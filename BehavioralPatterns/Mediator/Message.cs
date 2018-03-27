using Mediator.Colleagues;

namespace Mediator
{
    public class Message
    {
        public Message(string text)
        {
            Text = text;
        }

        public IColleague<Message> Sender { get; set; }
        
        public string Text { get; set; }
    }
}