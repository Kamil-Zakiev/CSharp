using System;
using System.Linq;
using EntitiesAndMaps.Books;
using NHibernate;
using NHibernate.Linq;
using SessionFactoryBuilder;

namespace ReadOnlyStrategy
{
    internal class Program
    {
        /// <summary> На сущности, которые были созданы и сохранены (Save), действие  DefaultReadOnly = true не распространяется  </summary>
        public static void Example1(ISessionFactory sessionFactory)
        {
            var session = sessionFactory.OpenSession();
            session.DefaultReadOnly = true;
            using (var tr = session.BeginTransaction())
            {
                var book = new Book()
                {
                    Title = "New Book",
                    Year = 2018
                };

                session.Save(book);
                session.Flush();
                // INSERT INTO book (title, year, id) VALUES (:p0, :p1, :p2);

                book.Title += DateTime.Now.Ticks;

                tr.Commit();
                // UPDATE book SET title = :p0, year = :p1 WHERE id = :p2;
            }
        }

        /// <summary> При установке DefaultReadOnly = true изменения в БД не фиксируются </summary>
        public static void Example2(ISessionFactory sessionFactory)
        {
            var session = sessionFactory.OpenSession();
            Console.WriteLine($"Initial value of session's parameter DefaultReadOnly = {session.DefaultReadOnly}");

            Console.WriteLine("Changing book's title in transaction...");
            using (var tr = session.BeginTransaction())
            {
                var book = session.Query<Book>().First();
                book.Title = "First book " + DateTime.Now.Ticks;
                tr.Commit();
                // UPDATE book SET title = :p0, year = :p1 WHERE id = :p2;
            }

            session.DefaultReadOnly = true;
            Console.WriteLine("Changed session's DefaultReadOnly parameter to true");
            Console.WriteLine("Changing book's title in transaction...");
            using (var tr = session.BeginTransaction())
            {
                var book = session.Query<Book>().First();
                book.Title = "First book " + DateTime.Now.Ticks;

                session.Update(book);
                tr.Commit();
                // no query
            }
        }

        /// <summary> При DefaultReadOnly = true кеш работает правильно </summary>
        public static void Example3(ISessionFactory sessionFactory)
        {
            var session = sessionFactory.OpenSession();
            session.DefaultReadOnly = true;
            using (var tr = session.BeginTransaction())
            {
                var book = session.Query<Book>().First();
                book.Title = "First book " + DateTime.Now.Ticks;

                var book2 = session.Get<Book>(book.Id); // no request, cache is used
                Console.WriteLine(ReferenceEquals(book, book2)); // True
                Console.WriteLine(ReferenceEquals(book.Title, book2.Title)); // True

                session.Update(book);
                tr.Commit();
                // no query
            }
        }

        /// <summary> GetAll также загружает сущности в режиме чтения </summary>
        public static void Example4(ISessionFactory sessionFactory)
        {
            var session = sessionFactory.OpenSession();
            session.DefaultReadOnly = true;
            using (var tr = session.BeginTransaction())
            {
                var book = session.Query<Book>().ToArray().First();
                book.Title = "First book " + DateTime.Now.Ticks;

                var book2 = session.Get<Book>(book.Id); // no request, cache is used
                Console.WriteLine(ReferenceEquals(book, book2)); // True
                Console.WriteLine(ReferenceEquals(book.Title, book2.Title)); // True

                session.Update(book);
                tr.Commit();
                // no query
            }
        }

        /// <summary> Если сущность загружена до проставления DefaultReadOnly = true, то она не станет сущностью только для чтения  </summary>
        public static void Example5(ISessionFactory sessionFactory)
        {
            var session = sessionFactory.OpenSession();
            using (var tr = session.BeginTransaction())
            {
                var book = session.Query<Book>().ToArray().First();
                book.Title = "First book " + DateTime.Now.Ticks;

                session.DefaultReadOnly = true;
                tr.Commit();
                // UPDATE book SET title = :p0, year = :p1 WHERE id = :p2;
            }
        }

        /// <summary>  Refresh перезагружает сущность из БД </summary>
        public static void Example6(ISessionFactory sessionFactory)
        {
            var session = sessionFactory.OpenSession();
            using (var tr = session.BeginTransaction())
            {
                var book = session.Query<Book>().First();
                book.Title = "First book " + DateTime.Now.Ticks;
                Console.WriteLine(book.Title);

                session.Refresh(book);
                Console.WriteLine(book.Title);

                tr.Commit();
                // no
            }
        }

        /// <summary>  Refresh не поможет сделать  сущность только для чтения </summary>
        public static void Example7(ISessionFactory sessionFactory)
        {
            var session = sessionFactory.OpenSession();
            using (var tr = session.BeginTransaction())
            {
                var book = session.Query<Book>().First();

                session.DefaultReadOnly = true;
                session.Refresh(book);

                book.Title = "First book " + DateTime.Now.Ticks;

                tr.Commit();
                // UPDATE book SET title = :p0, year = :p1 WHERE id = :p2;
            }
        }

        /// <summary> Если перед обновлением сущности вытащить её из кеша, то обновление сработает </summary>
        public static void Example8(ISessionFactory sessionFactory)
        {
            var session = sessionFactory.OpenSession();
            session.DefaultReadOnly = true;
            using (var tr = session.BeginTransaction())
            {
                var book = session.Query<Book>().First();
                book.Title = DateTime.Now.Ticks.ToString();

                session.Evict(book);
                session.Update(book);
                tr.Commit();
                // UPDATE book SET title = :p0, year = :p1 WHERE id = :p2;
            }
        }

        public static void Main(string[] args)
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();
            Example8(sessionFactory);
        }
    }
}