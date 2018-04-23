using System;
using EntitiesAndMaps.Books;
using SessionFactoryBuilder;

namespace CacheQuirk
{
    internal class Program
    {
        /// <summary>
        /// Предыстория: первый редактирует, но не флашит, а второй пометил удаленным, и успел коммитнуть
        ///  первый потом флашит, в итоге будет ли воскресать запись? Да
        /// 
        /// Обоснование: если сущность попадёт в кеш первой сессии раньше, чем вторая сессия проведет на ней какие-то изменения,
        /// и при этом первая сессия поменяет какое-либо поле этой сущности, то изменения второй сессии будут стёрты.
        /// Подразумевается, что первая сессия не зафлашит изменения до работы второй сессии, иначе вторая транзакция будет ждать конца первой сессии.
        /// </summary>
        private static void Example1()
        {
            var sessionFactory = SessionFactoryCreator.GetOrCreateSessionFactory();

            var session1 = sessionFactory.OpenSession();
            var tx1 = session1.BeginTransaction();
            var book1 = session1.Get<Book>(1L);
            Console.WriteLine("Initial year = " + book1.Year);

            #region Изменения из второго подключения к БД

            var session2 = sessionFactory.OpenSession();
            var tx2 = session2.BeginTransaction();
            var book2 = session2.Get<Book>(1L);
            book2.Year = 66;
            tx2.Commit();
            session2.Close();

            #endregion

            book1.Title = "Modified by 1st tx at " + DateTime.Now.Ticks;
            tx1.Commit();
            session1.Close();

            var session3 = sessionFactory.OpenSession();
            var tx3 = session3.BeginTransaction();
            var book3 = session3.Get<Book>(1L);
            Console.WriteLine(book3.Title);
            Console.WriteLine("Fact year = " + book3.Year);
            tx3.Commit();
            session3.Close();
        }

        public static void Main(string[] args)
        {
            Example1();
        }
    }
}