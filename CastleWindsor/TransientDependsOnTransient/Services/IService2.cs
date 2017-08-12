using System;

namespace SingletonDependsOnTransient.Services
{
    internal interface IService2 : IHasGuidId, IDisposable
    {
    }
}