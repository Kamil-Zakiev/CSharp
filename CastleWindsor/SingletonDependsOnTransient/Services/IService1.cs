using System;

namespace SingletonDependsOnTransient.Services
{
    internal interface IService1 : IHasGuidId// , IDisposable
    {
        IService2 Service2 { get; set; }
    }
}