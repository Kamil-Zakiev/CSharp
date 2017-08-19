using System;

namespace SingletonDependsOnScoped.Services
{
    internal interface IHasGuidId
    {
        Guid GuidId { get; }
    }
}