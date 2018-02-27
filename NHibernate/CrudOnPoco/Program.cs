using System;
using System.Linq;
using EntitiesAndMaps.Books;
using NHibernate;
using NHibernate.Linq;

namespace CrudOnPoco
{
    internal class Program
    {
        private static ISessionFactory _sessionFactory;
        
        /// <summary> Пример создания объекта и сохранения его в БД </summary>
        private static void CreateExample()
        {
            ISession session = _sessionFactory.OpenSession();
            var book = new Book { Title = "Book ", Year = 2010 };

            var res2 = session.Save(book);
            // SQL: select nextval('book_id_seq'); => 123
            Console.WriteLine(res2);

            // без "session.Flush();" ничего в БД не сохранится
            session.Flush();
            // insert into book(id, title, year) values (123, 'Book 1', 2014);

            session.Close();
        }

        /// <summary> Пример обновления объекта и сохранения его в БД </summary>
        private static void UpdateExample()
        {
            ISession session = _sessionFactory.OpenSession();
            var bookQuery = session.Query<Book>();
            var earliestBooks = bookQuery
                .Where(b => b.Year == bookQuery.Select(x => x.Year).Min())
                .ToList();

            foreach (var earliestBook in earliestBooks)
            {
                earliestBook.Title = "Наидревнейшая книга в этой библиотеке!";
                session.Update(earliestBook);
            }

            // без "session.Flush();" ничего в БД не сохранится
            session.Flush();
            // SQL: Batch commands...

            session.Close();
        }

        private static void DeleteExample()
        {
            ISession session = _sessionFactory.OpenSession();
            var bookQuery = session.Query<Book>();
            var newestBooks = bookQuery
                .Where(b => b.Year == bookQuery.Max(x => x.Year))
                .ToList();

            foreach (var newiestBook in newestBooks)
            {
                session.Delete(newiestBook);
            }

            // без "session.Flush();" ничего в БД не сохранится
            session.Flush();
            // SQL: Batch commands...

            session.Close();
        }

        private static void Main(string[] args)
        {
            _sessionFactory = SessionFactoryBuilder.SessionFactoryCreator.GetOrCreateSessionFactory();
            //log4net.Config.XmlConfigurator.Configure();
            CreateExample();
            return;
            ISession session = _sessionFactory.OpenSession();
            ITransaction transaction = session.BeginTransaction();



            var res3 = session.Query<Book>().Where(x => x.Year == 2014).Count();
            // SQL: insert into book(id, title, year) values (123, 'Book 1', 2014);
            // SQL: select count(*) from book where year = 2014;
            // => 1


            transaction.Commit();
            session.Close();
        }
    }
}