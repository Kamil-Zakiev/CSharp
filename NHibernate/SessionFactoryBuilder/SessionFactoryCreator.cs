using System;
using System.Linq;
using EntitiesAndMaps.Books;
using EntitiesAndMaps.Persons;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Mapping.ByCode;

namespace SessionFactoryBuilder
{
    internal class FlushEventListener : IFlushEventListener
    {
        public void OnFlush(FlushEvent @event)
        {
            Console.WriteLine("Flush!");
        }
    }

    public class SessionFactoryCreator
    {
        private static ISessionFactory _sessionFactory;

        public static ISessionFactory GetOrCreateSessionFactory()
        {
            if (_sessionFactory != null)
            {
                return _sessionFactory;
            }

            var cfg = new Configuration();

            // настройка подключения к БД
            cfg.DataBaseIntegration(d =>
            {
                // диалект контролирует синтаксис sql-запросов
                d.Dialect<NHibernate.Dialect.PostgreSQLDialect>();

                // драйвер отвечает за отправку и прием данных
                d.Driver<NHibernate.Driver.NpgsqlDriver>();

                d.ConnectionString = "Server=localhost;Port=5432;Database=library;User ID=postgres;Password=123";
                d.LogSqlInConsole = true;
                
            });

            var oldListenets = cfg.EventListeners.FlushEventListeners.ToList();
            oldListenets.Add(new FlushEventListener());

            cfg.EventListeners.FlushEventListeners = oldListenets.ToArray();

            // указываем маппинги сущностей
            var modelMapper = new ModelMapper();
            modelMapper.AddMapping<BookMapping>();
            modelMapper.AddMapping<PersonMap>();
            cfg.AddMapping(modelMapper.CompileMappingForAllExplicitlyAddedEntities());

            // создаем объект ISessionFactory, хранящий в себе настройки, в единственном экземпляре
            // этот объект не содержит подключения к БД
            var sessionFactory = cfg.BuildSessionFactory();

            _sessionFactory = sessionFactory;
            return _sessionFactory;
        }

    }
}