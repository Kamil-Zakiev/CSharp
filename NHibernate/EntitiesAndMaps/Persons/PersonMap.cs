using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace EntitiesAndMaps.Persons
{
    public class PersonMap : ClassMapping<Person>
    {
        public PersonMap()
        {
            Table("person");
            Id(x => x.Id, m =>
            {
                m.Column("id");
                m.Generator(Generators.Sequence, g => g.Params(new {sequence = "book_id_seq" }));
            });
            Property(x => x.Name, m => m.Column("name"));
            
            ManyToOne(x => x.FavouriteBook, m =>
            {
                m.Column("fav_book_id");
                m.NotNullable(true);
                m.Cascade(Cascade.All);

                // типа не будет доп запроса на получение свойств, потому что все данные подгрузятся сразу
                // m.Fetch(FetchKind.Join);
                m.Fetch(FetchKind.Select);
               // m.Lazy(LazyRelation.NoProxy);
            });
        }
    }
}