using System;

namespace TransientDiposableHolding.Services
{
    internal interface IService1 : IHasGuidId //, IDisposable // можно убрать, т.к. компонент сам реализовывает IDisposable
    {
    }
}