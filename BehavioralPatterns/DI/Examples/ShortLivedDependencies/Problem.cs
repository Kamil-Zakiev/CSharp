namespace DI.Examples.ShortLivedDependencies.Problem
{
    interface IService
    {
        void DoSomething();
    }

    class DatabaseService : IService
    {
        public void DoSomething()
        {
            // opens a connection to DB
        }

        public void CloseConnection()
        {
            // closes the connection
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
