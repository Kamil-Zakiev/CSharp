using System;
using System.Linq;
using EntitiesAndMaps.Books;
using NHibernate;
using NHibernate.Linq;
using SessionFactoryBuilder;

namespace Transactions
{
    internal class Program
    {
        /// <summary>Вложенные транзации, так нельзя: при коммите второй(на самом деле той же) транзакции будет ошибка о Dispose </summary>
        private static void Example1()
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();
            var session = sessionFactory.OpenSession();

            var transaction = session.BeginTransaction();
            var count1 = session.Query<Book>().Count();

            var transaction2 = session.BeginTransaction();
            Console.WriteLine("transaction == transaction2 is " + (transaction == transaction2));
            var count2 = session.Query<Book>().Count();
            transaction2.Commit();

            transaction.Commit();
            session.Close();
        }

        /// <summary> Последовательные транзакции (разные объекты), так можно </summary>
        private static void Example2()
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();
            var session = sessionFactory.OpenSession();

            var transaction1 = session.BeginTransaction();
            var count1 = session.Query<Book>().Count();

            transaction1.Commit();

            var transaction2 = session.BeginTransaction();
            Console.WriteLine("transaction == transaction2 is " + (transaction1 == transaction2));
            var count2 = session.Query<Book>().Count();

            transaction2.Commit();
            session.Close();
        }

        /// <summary> Коммит транзакции фиксирует изменения в БД </summary>
        private static void Example3()
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();
            var session = sessionFactory.OpenSession();
            var session2 = sessionFactory.OpenSession();

            var transaction1 = session.BeginTransaction();
            var transaction2 = session2.BeginTransaction();

            var book = session.Get<Book>(1L);
            Console.WriteLine("Old title: " + book.Title);
            book.Title = new string(book.Title.Reverse().ToArray());
            transaction1.Commit();
            // SQL: update

            var book2 = session2.Get<Book>(1L);
            Console.WriteLine("New title: " + book2.Title);
            transaction2.Commit();

            session.Close();
            session2.Close();
        }

        /// <summary> Откат после коммита - так нельзя: будет ошибка System.ObjectDisposedException </summary>
        private static void Example4()
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();
            var session = sessionFactory.OpenSession();
            var transaction = session.BeginTransaction();

            transaction.Commit();
            transaction.Rollback();
            session.Close();
        }

        /// <summary> Пример отката транзакции при dispose транзакции </summary>
        private static void Example5(ISessionFactory sessionFactory)
        {
            var session = sessionFactory.OpenSession();
            using (var tr = session.BeginTransaction())
            {
                var book = session.Get<Book>(406L);
                book.Title = "#3 Testing implicit";

                // фиксация изменений в БД не произойдет, будет rollback
            }

            session.Close();
        }

        /// <summary>
        ///     [Read Committed is the default isolation level in PostgreSQL]
        ///     Изменная первой транзакцией строка блокируется до окончания транзакции
        /// </summary>
        private static void Example6()
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();

            var session1 = sessionFactory.OpenSession();
            var tx1 = session1.BeginTransaction();
            var book1 = session1.Get<Book>(1L);

            Console.WriteLine(book1.Title);
            book1.Title = "Modified by 1st tx";
            session1.Flush();
            // session.Flush отправляется запрос к БД, блокирующий данную строку на обновление другой транзакцией

            #region Изменения из второго подключения к БД

            // дедлок при использовании транзакции
            // без транзакций - все норм, считывается старое значение
            var session2 = sessionFactory.OpenSession();
            var tx2 = session2.BeginTransaction();
            var book2 = session2.Get<Book>(1L);

            Console.WriteLine(book2.Title);
            book2.Title = "Modified by 2nd tx";

            tx2.Commit(); // дедлок 
            session2.Close();

            #endregion

            tx1.Rollback();
            session1.Close();
        }

        private static void Main(string[] args)
        {
            Example1();
        }
    }
}