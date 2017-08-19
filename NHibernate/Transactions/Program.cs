using System;
using System.Linq;
using EntitiesAndMaps.Books;
using NHibernate.Linq;
using SessionFactoryBuilder;

namespace Transactions
{
    internal class Program
    {
        /// <summary>
        ///     так нельзя, будет n+1 запросов
        /// </summary>
        private static void Example0()
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();

            var session = sessionFactory.OpenSession();

            var bookQuery = session.Query<Book>();
            // так нельзя, при переборе будет n+1 запросов
            var newBooks = bookQuery.AsEnumerable()
                .Where(b => b.Year == bookQuery.Max(x => x.Year))
                .AsEnumerable();

            session.Close();
        }

        /// <summary>
        ///     так нельзя, при коммите второй транзакции будет ошибка о Dispose
        /// </summary>
        private static void Example1()
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();

            var session = sessionFactory.OpenSession();

            var transaction = session.BeginTransaction();
            var transaction2 = session.BeginTransaction();
            transaction2.Commit();
            transaction.Commit();

            session.Close();
        }

        /// <summary>
        ///     последовательные транзакции, так можно
        /// </summary>
        private static void Example2()
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();

            var session = sessionFactory.OpenSession();

            var transaction = session.BeginTransaction();

            transaction.Commit();

            var transaction2 = session.BeginTransaction();
            transaction2.Commit();

            session.Close();
        }

        /// <summary>
        ///     последовательные транзакции, изменение сотосяния объекта
        ///     Коммит транзакции фиксирует изменения в БД
        ///     Сессия не делает запроса к БД, если в кеше есть копия объекта
        /// </summary>
        private static void Example3()
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();

            var session = sessionFactory.OpenSession();

            var transaction = session.BeginTransaction();

            var book = session.Get<Book>(1l);
            // SQL: SELECT book0_.id as id1_0_0_, book0_.title as title2_0_0_, book0_.year as year3_0_0_ FROM book book0_ WHERE book0_.id=1;

            Console.WriteLine(book.Title);
            book.Title = "Хоп-хоп-хоп";
            Console.WriteLine(book.Title);

            transaction.Commit();
            // SQL:update

            var transaction2 = session.BeginTransaction();

            Console.WriteLine();
            book = session.Get<Book>(1l);
            // SQL: - без предварительного "session.Clear()" нет запроса, берется из кеша

            Console.WriteLine(book.Title);
            book.Title = "Хой-хой-хой";
            Console.WriteLine(book.Title);

            transaction2.Commit();
            // SQL: update
            session.Flush();
            // SQL: ничего не происходит

            session.Close();
        }

        /// <summary>
        ///     так нельзя, при откате транзакции после коммита будет ошибка о Dispose
        /// </summary>
        private static void Example4()
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();

            var session = sessionFactory.OpenSession();

            var transaction = session.BeginTransaction();


            transaction.Commit();
            transaction.Rollback();

            session.Close();
        }


        /// <summary>
        ///     так нельзя, без флаша или коммита транзакции изменение в БД не фиксируется 
        /// </summary>
        private static void Example5()
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();

            var session = sessionFactory.OpenSession();

            var book = session.Get<Book>(1l);
            // SQL: SELECT book0_.id as id1_0_0_, book0_.title as title2_0_0_, book0_.year as year3_0_0_ FROM book book0_ WHERE book0_.id=1;

            Console.WriteLine(book.Title);
            book.Title = "Хоп-хоп-хоп";
            Console.WriteLine(book.Title);

            session.Close();
        }

        private static void Main(string[] args)
        {
            Example2();
        }
    }
}