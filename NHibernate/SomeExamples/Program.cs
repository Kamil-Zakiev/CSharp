using System.Linq;
using EntitiesAndMaps.Books;
using NHibernate.Linq;
using SessionFactoryBuilder;

namespace SomeExamples
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
        /// getall.Where.Any генерирует тот же запрос, что и getall.Any
        /// </summary>
        private static void Example1()
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();

            var session = sessionFactory.OpenSession();

            var test = session.Query<Book>()
                .Where(book => book.Title == "any title")
                .Where(book => book.Year == 2010)
                .Where(book => book.Id == 25)
                .Any();
            
            // select book0_.id as id1_0_, book0_.title as title2_0_, book0_.year as year3_0_
            // from book book0_
            // where book0_.title=:p0 and book0_.year=:p1 and book0_.id=:p2
            // limit 1;

            var test2 = session
                .Query<Book>()
                .Any(book => book.Title == "any title" && book.Year == 2010 && book.Id == 25);
            
            // select book0_.id as id1_0_, book0_.title as title2_0_, book0_.year as year3_0_
            // from book book0_
            // where book0_.title=:p0 and book0_.year=:p1 and book0_.id=:p2
            // limit 1;
            
            session.Close();
        }
        
        public static void Main(string[] args)
        {
            Example1();
        }
    }
}