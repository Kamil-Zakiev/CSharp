using System;
using SingletonDependsOnScoped.Services;

namespace SingletonDependsOnScoped.Components
{
    internal class Component3 : IService3
    {
        private readonly Guid _guidId;

        public Component3()
        {
            _guidId = Guid.NewGuid();
            Console.WriteLine($"Component3 was created with GuidId = {_guidId}");
        }

        ~Component3()
        {
            Console.WriteLine("Destroyed Component3 with guid = {0}", _guidId);
        }

        public Guid GuidId => _guidId;

        public void Dispose()
        {
            Console.WriteLine("Disposed Component3 with guid = {0}", _guidId);
        }
    }
}