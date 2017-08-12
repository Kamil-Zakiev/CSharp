using System;

namespace TransientDiposableHolding.Services
{
    internal interface IHasGuidId
    {
        Guid GuidId { get; }
    }
}