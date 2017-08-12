using System;

namespace SingletonDependsOnTransient.Services
{
    internal interface IHasGuidId
    {
        Guid GuidId { get; }
    }
}