using System;

namespace SingletonDependsOnScoped.Services
{
    internal interface IService1 : IHasGuidId, IDisposable
    {
        IService2 Service2 { get; set; }
    }
}