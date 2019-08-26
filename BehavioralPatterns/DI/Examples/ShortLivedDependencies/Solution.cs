using System;

namespace DI.Examples.ShortLivedDependencies.Solution
{
    interface IService
    {
        void DoSomething();
    }

    interface IManagedService : IService, IDisposable
    {
    }

    interface IManagedServiceFactory
    {
        IManagedService Create();
    }

    class ManagedService : IManagedService
    {
        public void Dispose()
        {
            CloseConnection();
        }

        public void DoSomething()
        {
            // opens a connection to DB
        }

        private void CloseConnection()
        {
            // closes the connection
        }
    }

    class DatabaseService : IService
    {
        private readonly IManagedServiceFactory _managedServiceFactory;

        public DatabaseService(IManagedServiceFactory managedServiceFactory)
        {
            _managedServiceFactory = managedServiceFactory;
        }

        public void DoSomething()
        {
            using (var service = _managedServiceFactory.Create())
            {
                service.DoSomething();
            }
        }
    }

    class SomeClass
    {
        private readonly IService _service;

        public SomeClass(IService service)
        {
            _service = service;
        }

        void SomeMethod()
        {
            _service.DoSomething();

            // todo: how to deal with closing connection?
        }
    }
}
