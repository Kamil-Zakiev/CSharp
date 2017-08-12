using System;

namespace SingletonDependsOnScoped.Services
{
    internal interface IService2 : IHasGuidId, IDisposable
    {
    }
}