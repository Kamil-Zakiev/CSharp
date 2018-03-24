using System;
using System.Linq;
using EntitiesAndMaps.Books;
using EntitiesAndMaps.Persons;
using NHibernate;
using NHibernate.Linq;
using SessionFactoryBuilder;

namespace FlushExamples
{
    internal class Program
    {
        /// <summary> Пример создания объекта и сохранения его в БД </summary>
        private static void CreateExample(ISessionFactory sessionFactory)
        {
            var session = sessionFactory.OpenSession();
            var book = new Book {Title = "Book ", Year = 2010};

            var res2 = session.Save(book);
            // SQL: select nextval('book_id_seq'); => 123
            Console.WriteLine(res2);

            session.Flush();
            // insert into book(id, title, year) values (123, 'Book ', 2014);

            session.Close();
        }

        /// <summary> Пример создания объекта и отсутствие сохранения в БД </summary>
        private static void CreateExampleWithoutFlush(ISessionFactory sessionFactory)
        {
            var session = sessionFactory.OpenSession();
            var book = new Book {Title = "Book ", Year = 2010};

            var res2 = session.Save(book);
            // SQL: select nextval('book_id_seq'); => 123
            Console.WriteLine(res2);

            // без "session.Flush();" ничего в БД не сохранится
            session.Close();
        }

        /// <summary> Пример создания объекта и сохранения в БД с помощью транзакций</summary>
        private static void CreateExampleTransaction(ISessionFactory sessionFactory)
        {
            var session = sessionFactory.OpenSession();
            var tr = session.BeginTransaction();
            Console.WriteLine("tr.WasCommitted == " + tr.WasCommitted);
            Console.WriteLine("tr.IsActive == " + tr.IsActive);

            var book = new Book {Title = "Book ", Year = 2010};
            var res2 = session.Save(book);
            Console.WriteLine(res2);

            // при коммите транзакции происходит Flush сессии и отправка запроса в БД
            tr.Commit();
            Console.WriteLine("tr.WasCommitted == " + tr.WasCommitted);
            Console.WriteLine("tr.IsActive == " + tr.IsActive);

            session.Close();
        }

        /// <summary> Пример отправки запроса на обновление объекта, при открытой транзакции </summary>
        private static void CreateExampleImplicitAction(ISessionFactory sessionFactory)
        {
            var session = sessionFactory.OpenSession();
            var tr = session.BeginTransaction();

            var book = session.Get<Book>(406L);
            book.Title = "#3 Testing implicit";

            // перед запросом отправляются изменения в БД в рамках открытой транзакции, но не фиксируются
            var allBooks = session.Query<Book>().ToArray();

            // а здесь отправки запроса не будет
            // var allPersons = session.Query<Person>().Count();

            // Flush + фиксация в БД (commit)
            tr.Commit();
            session.Close();
        }
        
        /// <summary> Пример отправки запроса на обновление объекта, при открытой транзакции </summary>
        private static void CreateExampleImplicitAction3(ISessionFactory sessionFactory)
        {
            var session = sessionFactory.OpenSession();

            var tr = session.BeginTransaction();
            var book = session.Get<Book>(406L);
            tr.Commit();
            book.Title = "#3 Testing implicit";
            
            tr = session.BeginTransaction();
            // перед запросом изменения НЕ отправляются в БД в рамках открытой транзакции
            var allBooks = session.Query<Book>().ToArray();

            // Flush + фиксация в БД (commit)
            tr.Commit();
            session.Close();
        }

        /// <summary> Пример отправки запроса на обновление объекта при открытой транзакции </summary>
        private static void CreateExampleImplicitAction2(ISessionFactory sessionFactory)
        {
            var session = sessionFactory.OpenSession();
            var tr = session.BeginTransaction();

            var book = session.Get<Book>(406L);
            book.Title = "#3 Testing implicit";

            // отправка запроса на обновление
            session.Flush();

            // фиксация изменений в БД не произойдет, будет откат транзакции
            session.Close();
        }

        /// <summary>
        ///     FlushMode.Commit:
        ///     Перед запросом всех сущностей не происходит учитывание новых объектов, т.к. нет запроса insert перед query all
        /// </summary>
        public static void Example1()
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();
            var session1 = sessionFactory.OpenSession();
            session1.FlushMode = FlushMode.Commit;

            var newBook = new Book
            {
                Title = "Brand new book",
                Year = 2018
            };
            session1.Save(newBook);

            var tx1 = session1.BeginTransaction();
            Console.WriteLine("Transaction was opened.");
            var isNewBookConsidered = session1.Query<Book>().Any(book => book.Id == newBook.Id);
            Console.WriteLine("Was new book considered?");
            Console.WriteLine(isNewBookConsidered ? "yes" : "no");

            tx1.Commit();
            session1.Close();
        }

        /// <summary>
        ///     statelessSession - отправка запроса сразу в БД при вызове команд
        ///     Перед запросом всех сущностей происходит учитывание новых объектов, т.к. есть запрос insert перед query all
        /// </summary>
        public static void Example2()
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();
            var statelessSession = sessionFactory.OpenStatelessSession();

            var newBook = new Book
            {
                Title = "Brand new book",
                Year = 2018
            };
            statelessSession.Insert(newBook);

            var isNewBookConsidered = statelessSession.Query<Book>().Any(book => book.Id == newBook.Id);
            Console.WriteLine("Was new book considered?");
            Console.WriteLine(isNewBookConsidered ? "yes" : "no");

            statelessSession.Close();
        }

        public static void Main(string[] args)
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();
            CreateExampleImplicitAction3(sessionFactory);
        }
    }
}