using System;
using TransientHolding.Services;

namespace TransientHolding.Components
{
    internal class Component1 : IService1
    {
        private readonly Guid _guidId;
        public Guid GuidId => _guidId;

        public Component1()
        {
            _guidId = Guid.NewGuid();
            Console.WriteLine($"Component1 was created with GuidId = {_guidId}");
        }

        ~Component1()
        {
            Console.WriteLine("Destroyed component1 with guid = {0}", _guidId);
        }
    }
}