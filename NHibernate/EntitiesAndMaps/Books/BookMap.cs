using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace EntitiesAndMaps.Books
{
    public class BookMapping : ClassMapping<Book>
    {
        public BookMapping()
        {
            Table("book");
            Id(x => x.Id, m =>
            {
                m.Column("id");
                m.Generator(Generators.Sequence, g => g.Params(new {sequence = "book_id_seq"}));
            });
            Property(x => x.Title, m => m.Column("title"));
            Property(x => x.Year, m => m.Column("year"));
        }
    }
}