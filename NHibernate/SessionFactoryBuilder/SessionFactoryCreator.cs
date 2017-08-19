using EntitiesAndMaps.Books;
using EntitiesAndMaps.Persons;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;

namespace SessionFactoryBuilder
{
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
                d.Dialect<NHibernate.Dialect.MsSql2012Dialect>();

                // драйвер отвечает за отправку и прием данных
                d.Driver<NHibernate.Driver.SqlClientDriver>();

                d.ConnectionString = "Server=.;Initial Catalog=Library;Integrated Security=true";
                d.LogSqlInConsole = true;

            });

            // указываем маппинги сущностей
            var modelMapper = new ModelMapper();
            modelMapper.AddMapping<BookMapping>();
            modelMapper.AddMapping<PersonMap>();
            cfg.AddMapping(modelMapper.CompileMappingForAllExplicitlyAddedEntities());

            // создаем объект ISessionFactory, хранящий в себе настройки, в единственном экземпляре
            // этот объект не содержит подключения к БД
            ISessionFactory sessionFactory = cfg.BuildSessionFactory();

            _sessionFactory = sessionFactory;
            return _sessionFactory;
        }

    }
}