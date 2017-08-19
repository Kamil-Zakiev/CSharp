using System;
using System.Linq;
using EntitiesAndMaps.Books;
using EntitiesAndMaps.Persons;
using NHibernate.Linq;
using SessionFactoryBuilder;

namespace GetLoadAndLazyFetch
{
    internal class Program
    {
        private static void CreatePerson()
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();

            var session = sessionFactory.OpenSession();

            var book = session.Get<Book>(1l);
            var person = new Person
            {
                FavouriteBook = book,
                Name = "Камиль"
            };
            session.Save(person);
            session.Flush();
            
            session.Close();
        }

        /// <summary>
        /// Если ссылка замаплена со стратегией Fetch = Select, то при каждом обращении по ссылке будет отправляться SQL-запрос
        /// </summary>
        private static void Example1()
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();

            var session = sessionFactory.OpenSession();

            // в маппинге m.Fetch(FetchKind.Select)
            var person = session.Get<Person>(91l);
            // SQL: select person where id = 91

            Console.WriteLine("Любимая книга: {0}", person.FavouriteBook.Title);
            // SQL: select book where id = 1

            session.Close();
        }

        /// <summary>
        /// Если ссылка замаплена со стратегией Fetch = Join, то все данные об объекте-ссылке подтянутся в одном SQL-запросе
        /// </summary>
        private static void Example2()
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();

            var session = sessionFactory.OpenSession();

            // в маппинге m.Fetch(FetchKind.Join)
            var person = session.Get<Person>(91l);
            // SQL: select person inner join book where person.id = 91 and book.id = person.fav_book_id

            Console.WriteLine("Любимая книга: {0}", person.FavouriteBook.Title);

            session.Close();
        }

        /// <summary>
        /// Load откладывает подгрузку объекта до того момента, пока не потребуются его свойства
        /// Если объекта по ID нет, то выкидывается ошибка
        /// Логика стратегии Fetch работает
        /// </summary>
        private static void Example3()
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();

            var session = sessionFactory.OpenSession();

            // в маппинге m.Fetch(FetchKind.Select)
            var person = session.Load<Person>(91l);

            Console.WriteLine("Имя: {0}", person.Name);
            // SQL: SELECT person0_.id as id1_1_0_, person0_.name as name2_1_0_, person0_.fav_book_id as fav3_1_0_ FROM person person0_ WHERE person0_.id = 91
            Console.WriteLine("Любимая книга: {0}", person.FavouriteBook.Title);
            // SQL: SELECT book0_.id as id1_0_0_, book0_.title as title2_0_0_, book0_.year as year3_0_0_ FROM book book0_ WHERE book0_.id = 1
            
            session.Close();
        }
        
        /// <summary>
        /// С помощью Fetch обеспечивается подгрузка объектов, тем самым избавляемся от проблемы n+1 запросов
        /// </summary>
        private static void Example4()
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();

            var session = sessionFactory.OpenSession();
            
            // var persons = session.Query<Person>().ToList();
            var persons = session.Query<Person>().Fetch(x => x.FavouriteBook).ToList();
            
            session.Close();
        }
        
        /// <summary>
        /// Не стоит обращаться к объектам после закрытия сессии
        /// </summary>
        private static void Example5()
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();

            var session = sessionFactory.OpenSession();
            var person = session.Load<Person>(91l);

            Console.WriteLine(person.Name);
            session.Close();
            Console.WriteLine(person.FavouriteBook.Title);
            // Exception если стратегия Fetch = Select
            // Хой-хой-хой если стратегия Fetch = Join, ведь данные уже подгружены
        }

        private static void Main(string[] args)
        {

            Example5();
        }
    }
}