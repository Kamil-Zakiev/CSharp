using System;
using System.Linq;
using EntitiesAndMaps.Books;
using NHibernate;
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


        /// <summary>
        ///     открытие транзакции блокирует чтение данных из БД, 
        ///     поэтому если после Флаша выполняется откат, то состояние БД возвращается в исходное состояние
        /// </summary>
        private static void Example6()
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();

            var session = sessionFactory.OpenSession();
            var tx = session.BeginTransaction();
            var book = session.Get<Book>(1l);
            
            book.Title = "Хоп!-хоп!-хоп!";
            session.Flush();

            tx.Rollback();

            session.Close();
        }


        /// <summary>
        ///     уровень изоляции транзакций ReadCommited гарантирует, что другие подключения к БД не смогу считать модиф. данные
        ///     (изменная строка блокируется до окончания транзакции)
        ///     если не использовать транзакции, то нет гарантии, то не будет грязного чтения
        /// </summary>
        private static void Example7()
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();

            var session = sessionFactory.OpenSession();
            var tx = session.BeginTransaction();
            var book = session.Get<Book>(1l);

            book.Title = "Хопееееее!-хоп!-хоп!";
            session.Flush();

            #region Изменения из второго подключения к БД

            var session2 = sessionFactory.OpenSession();
            //  var tx2 = session2.BeginTransaction();
            var book2 = session2.Get<Book>(1l);

            book2.Title = "Хоп?123!-хоп?!-хоп?!";

            // session2.Flush();
            // tx2.Commit();
            session2.Close(); 

            #endregion

            tx.Rollback();

            session.Close();
        }
        
        /// <summary> Пример отката транзакции при dispose транзакции </summary>
        private static void Example8(ISessionFactory sessionFactory)
        {
            var session = sessionFactory.OpenSession();
            sessionFactory.OpenSession();
            using (var tr = session.BeginTransaction())
            {
                var book = session.Get<Book>(406L);
                book.Title = "#3 Testing implicit";
                
                // фиксация изменений в БД не произойдет, будет rollback
            }

            session.Close();
        }


        private static void Main(string[] args)
        {
            Example3();
        }
    }
}