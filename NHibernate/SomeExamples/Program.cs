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
        
        public static void Main(string[] args)
        {
        }
    }
}