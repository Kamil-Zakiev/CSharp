using System.Data;
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

        /// <summary> GetAll всегда отправляет запрос, не использует кеш </summary>
        private static void Example3()
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();
            var session = sessionFactory.OpenSession();

            var persons = session.Query<Person>().ToList();
            // SQL:  select person0_.id as id1_1_, person0_.name as name2_1_, person0_.fav_book_id as fav3_1_ from person person0_
            
            var persons2 = session.Query<Person>().ToList();
            // SQL:  select person0_.id as id1_1_, person0_.name as name2_1_, person0_.fav_book_id as fav3_1_ from person person0_

            session.Close();
        }

        /// <summary> Про проекцию: они кешируются (речь про x=> new{..., x = x, ...}) и, следовательно, отслеживаются </summary>
        private static void Example4()
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();
            var session = sessionFactory.OpenSession();

            var persons = session.Query<Person>().Select(x => new
            {
                asd = "asd",
                person = x
            }).ToList();
            // SQL: select person0_.id as id1_1_, person0_.name as name2_1_, person0_.fav_book_id as fav3_1_ from person person0_

             var person = persons.First().person;
            person.Name = "Человек, у sdfgdsfg Id = " + person.Id;
           
            session.Flush();
            // изменения зафиксировались в БД

            session.Close();
        }

        /// <summary>
        /// Похоже на то, что NH синхронизирует кешированные сущности между разными сессиями
        /// Это слишком сложная задача, видимо кеш находится на уровне фабрики
        /// </summary>
        private static void Example5()
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();
            var session = sessionFactory.OpenSession();

            var persons = session.Query<Person>().ToList();

            foreach (var person in persons)
            {
                person.Name += "_LALAL";
            }


            var session2 = sessionFactory.OpenSession();
            var persons2 = session.Query<Person>().ToList();
            foreach (var person in persons2)
            {
                person.Name += "DirtyName";
            }
            session2.Flush();
            session2.Close();

            session.Flush();
            session.Close();
        }


        /// <summary>
        /// Похоже, что NH синхронизирует кешированные сущности между разными сессиями
        /// Видимо кеш находится на уровне фабрики
        /// Если сделать выброс сущности из кеша, то изменения по кешу второго уровня его не достанут!
        /// </summary>
        private static void Example6()
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();
            var session = sessionFactory.OpenSession();

            var person = session.Get<Person>(91l);
            // Если сделать выброс сущности из кеша, то изменения по кешу второго уровня его не достанут!
            //session.Evict(person);

            person.Name += "_1";
            
            var session2 = sessionFactory.OpenSession();

            var person2 = session.Get<Person>(91l);
            person2.Name += "_2";
            session2.Flush();
            session2.Close();

            session.Flush();
            session.Close();
        }

        private static void Main(string[] args)
        {
            Example6();
        }
    }
}