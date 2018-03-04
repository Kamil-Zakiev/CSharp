using System;
using System.Linq;
using EntitiesAndMaps.Books;
using NHibernate;
using NHibernate.Linq;

namespace CrudOnPoco
{
    internal class Program
    {
        /// <summary> Пример создания объекта и сохранения его в БД </summary>
        /// <param name="sessionFactory"></param>
        private static void CreateExample(ISessionFactory sessionFactory)
        {
            var session = sessionFactory.OpenSession();
            var book = new Book { Title = "Book ", Year = 2010 };

            var res2 = session.Save(book);
            // SQL: select nextval('book_id_seq'); => 1
            Console.WriteLine(res2);

            session.Flush();
            // insert into book(id, title, year) values (123, 'Book 1', 2014);

            session.Close();
        }
        
        /// <summary> Пример создания нескольких объектов и их сохранения в БД </summary>
        /// <param name="sessionFactory"></param>
        private static void MultipleCreateExample(ISessionFactory sessionFactory)
        {
            var session = sessionFactory.OpenSession();
            // todo: configeru postgres batcher
            // session.SetBatchSize(20);
            var books = Enumerable.Range(1, 10)
                .Select(number => new Book()
                {
                    Title = "Book #" + number,
                    Year = 2010 + number / 20
                })
                .ToArray();

            foreach (var book in books)
            {
                var res = session.Save(book);
                // NHibernate: select nextval ('book_id_seq')
                
                Console.Write(res);
                Console.Write(", ");
            }
            Console.WriteLine();

            Console.WriteLine("Before session flush");
            session.Flush();
            Console.WriteLine("After session flush");
            // 

            session.Close();
        }

        /// <summary> Пример обновления объекта и сохранения его в БД </summary>
        private static void UpdateExample(ISessionFactory sessionFactory)
        {
            var session = sessionFactory.OpenSession();
            var bookQuery = session.Query<Book>();
            var earliestBooks = bookQuery
                .Where(b => b.Year == bookQuery.Select(x => x.Year).Min())
                .ToList();
            // NHibernate: select book0_.id as id1_0_, book0_.title as title2_0_, book0_.year as year3_0_
            // from book book0_ where book0_.year=(select min(book1_.year) from book book1_)

            foreach (var earliestBook in earliestBooks)
            {
                earliestBook.Title = "Наидревнейшая книга в этой библиотеке!";
                session.Update(earliestBook);
            }

            session.Flush();
            // SQL: many update commands

            session.Close();
        }

        private static void DeleteExample(ISessionFactory sessionFactory)
        {
            var session = sessionFactory.OpenSession();
            var bookQuery = session.Query<Book>();
            var newestBooks = bookQuery
                .Where(b => b.Year == bookQuery.Max(x => x.Year))
                .ToList();

            foreach (var newiestBook in newestBooks)
            {
                session.Delete(newiestBook);
            }

            session.Flush();
            // many delete commands

            session.Close();
        }

        private static void Main(string[] args)
        {
            var sessionFactory = SessionFactoryBuilder.SessionFactoryCreator.GetOrCreateSessionFactory();
            DeleteExample(sessionFactory);
        }
    }
}