using EntitiesAndMaps.Books;

namespace EntitiesAndMaps.Persons
{
    public class Person
    {
        // 1. должен быть конструктор без параметров
        // 2. все свойства делаем виртуальными, чтобы NH смог создать прокси-класс, переобпределив в нем все свойства

        public virtual long Id { get; set; }

        public virtual string Name { get; set; }

        public virtual Book FavouriteBook { get; set; }
    }
}