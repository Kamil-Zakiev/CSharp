﻿using System;
using Mediator.Mediators;

namespace Mediator.Colleagues
{
    internal class VerySmartColleague : AbstractColleague
    {
        public VerySmartColleague(IMediator<IColleague<Message>, Message> mediator, string name) : base(mediator, name)
        {
        }

        public override void DoWork()
        {
            Console.WriteLine($"{Name} is doing some work");
            var now = DateTime.UtcNow;
            if (now.Ticks % 2 != 0) SendMessage(new Message("something has happened with me"));
        }

        public override void Notify(Message message)
        {
            Console.WriteLine($"very smart {Name} has recieved {message.Sender.Name}\'s message \"{message.Text}\"");
        }
    }
}