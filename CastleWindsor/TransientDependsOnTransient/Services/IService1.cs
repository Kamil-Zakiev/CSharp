namespace SingletonDependsOnTransient.Services
{
    internal interface IService1 : IHasGuidId
    {
        IService2 Service2 { get; set; }
    }
}