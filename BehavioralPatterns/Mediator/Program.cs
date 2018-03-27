using Mediator.Colleagues;

namespace Mediator
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var mediator = new Mediators.Mediator();

            var col1 = new SmartColleague(mediator, "Jack");
            var col2 = new VerySmartColleague(mediator, "John");
            var col3 = new SmartColleague(mediator, "Patrik");
            var col4 = new VerySmartColleague(mediator, "Franchesco");
            var col5 = new SmartColleague(mediator, "Bob");
            var col6 = new VerySmartColleague(mediator, "Alice");

            // colleagues are working and can communicate through mediator
            col1.DoWork();
            col2.DoWork();
            col3.DoWork();
            col4.DoWork();
            col5.DoWork();
            col6.DoWork();

            // also they can just send a message to their colleagues
            col1.SendMessage(new Message("Hello!"));
            col2.SendMessage(new Message("Hello my friend!"));
            col4.SendMessage(new Message("I had a bad day don't talk to me please :("));
        }
    }
}