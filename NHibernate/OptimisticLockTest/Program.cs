namespace OptimisticLockTest
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Entities;
    using EntitiesAndMaps.Books;
    using NHibernate;
    using SessionFactoryBuilder;

    internal class Program
    {
        public static ISessionFactory SessionFactory { get; } = SessionFactoryCreator.GetOrCreateSessionFactory();
        
        public static AutoResetEvent AutoResetEvent = new AutoResetEvent(false);

        internal static Task InTransactionTask(Action<ISession> action)
        {
           return Task.Run(() => { InTransaction(action); });
        }

        private static void InTransaction(Action<ISession> action)
        {
            using (var session = SessionFactory.OpenSession())
            using (var tr = session.BeginTransaction())
            {
                try
                {
                    action(session);
                    tr.Commit();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    tr.Rollback();
                    throw;
                }
            }
        }

        public static void Main(string[] args)
        {
            long dogId = 0;
            InTransaction((session) =>
            {
                var dog = new Dog
                {
                    Age = 5,
                    Name = "Sharik",
                    IsAwake = true
                };

                session.Save(dog);
                dogId = dog.Id;
                Console.WriteLine(dog.Name);
                // Sharik
            });
            
            var t1 = InTransactionTask((session1) =>
            {
                var dog = session1.Get<Dog>(dogId);
                dog.Name = "Name by session 1 " + DateTime.UtcNow;
                
                session1.Update(dog);
                
                // wait for second thread complete
                AutoResetEvent.WaitOne();
                // ... exception if optimistic lock enabled
            });
            
            Thread.Sleep(100);

            var t2 = InTransactionTask((session2) =>
                {
                    var dog = session2.Get<Dog>(dogId);
                    dog.Name = "Name by session 2 " + DateTime.UtcNow;
                    
                    session2.Update(dog);
                })
                .ContinueWith(t => AutoResetEvent.Set());
            
            Task.WaitAll(t1, t2);
            
            InTransaction((session3) =>
            {
                var dog = session3.Get<Dog>(dogId);
                
                Console.WriteLine(dog.Name);
            });
        }
    }
}