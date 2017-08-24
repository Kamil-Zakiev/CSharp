using System.Linq;
using EntitiesAndMaps.Persons;
using NHibernate.Linq;
using SessionFactoryBuilder;

namespace Cache
{
    internal class Program
    {
        /// <summary> При GetAll, как и при Get, данные также попадают в кеш </summary>
        private static void Example1()
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();

            var session = sessionFactory.OpenSession();

            var persons = session.Query<Person>().ToList();
            // SQL:  select person0_.id as id1_1_, person0_.name as name2_1_, person0_.fav_book_id as fav3_1_ from person person0_

            var person = session.Get<Person>(91l);
            // No SQL, got from cache
            // SQL after tooltip: SELECT book0_.id as id1_0_0_, book0_.title as title2_0_0_, book0_.year as year3_0_0_ FROM book book0_ WHERE book0_.id=1;
            session.Close();
        }

        /// <summary> Способ удаления объекта из кеша. При повторном запросе к объекту по этому ID будет отправлен запрос к БД </summary>
        private static void Example2()
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();
            var session = sessionFactory.OpenSession();

            var persons = session.Query<Person>().ToList();
            // SQL:  select person0_.id as id1_1_, person0_.name as name2_1_, person0_.fav_book_id as fav3_1_ from person person0_

            var firstPerson = persons.First();
            session.Evict(firstPerson);

            var person = session.Get<Person>(firstPerson.Id);
            // SQL:  SELECT person0_.id as id1_1_0_, person0_.name as name2_1_0_, person0_.fav_book_id as fav3_1_0_ FROM person person0_ WHERE person0_.id=91

            session.Close();
        }

        private static void Main(string[] args)
        {
            
        }
    }
}