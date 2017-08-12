using System;
using TransientDiposableHolding.Services;

namespace TransientDiposableHolding.Components
{
    internal class Component2 : IService2
    {
        private readonly Guid _guidId;

        public Component2()
        {
            _guidId = Guid.NewGuid();
            Console.WriteLine($"Component2 was created with GuidId = {_guidId}");
        }

        ~Component2()
        {
            Console.WriteLine("Destroyed Component2 with guid = {0}", _guidId);
        }

        public Guid GuidId => _guidId;

        public void Dispose()
        {
            Console.WriteLine("Disposed Component2 with guid = {0}", _guidId);
        }
    }
}