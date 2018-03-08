using System;
using System.Linq;
using EntitiesAndMaps.Books;
using NHibernate.Linq;
using SessionFactoryBuilder;

namespace TransactionIsRequired
{
    internal class Program
    {
        /// <summary>
        ///     Перед запросом всех сущностей не происходит учитывание новых объектов, т.к. нет запроса insert перед query all
        /// </summary>
        public static void Example1()
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();
            var session1 = sessionFactory.OpenSession();

            var newBook = new Book
            {
                Title = "Brand new book",
                Year = 2018
            };
            session1.Save(newBook);

            var isNewBookConsidered = session1.Query<Book>().Any(book => book.Id == newBook.Id);
            Console.WriteLine("Was new book considered?");
            Console.WriteLine(isNewBookConsidered ? "yes" : "no");

            session1.Close();
        }

        /// <summary>
        ///     Перед запросом всех сущностей происходит учитывание новых объектов, т.к. есть запрос insert перед query all
        /// </summary>
        public static void Example2()
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();
            var session1 = sessionFactory.OpenSession();

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


        public static void Main(string[] args)
        {
            Example2();
        }
    }
}