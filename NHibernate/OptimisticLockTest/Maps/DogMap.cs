namespace OptimisticLockTest.Maps
{
    using Entities;
    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    public class DogMap : ClassMapping<Dog>
    {
        public DogMap()
        {
            Table("DOG");
            
            Id(dog => dog.Id, m =>
            {
                m.Column("ID");
                m.Generator(Generators.Sequence, g => g.Params(new
                {
                    sequence = "book_id_seq"
                }));
            });

            Property(dog => dog.Age, m =>
            {
                m.Column("AGE");
                m.NotNullable(true);
            });

            Property(dog => dog.Name, m =>
            {
                m.Column("NAME");
                m.NotNullable(true);
                
                // m.OptimisticLock(false); - turns version incrementing off
            });

            Property(dog => dog.IsAwake, m =>
            {

                m.Column("IS_AWAKE");
                m.NotNullable(true);
            });

            Version(dog => dog.RowVersion, m => { m.Column("row_version"); });
        }
    }
}