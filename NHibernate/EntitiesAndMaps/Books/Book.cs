namespace EntitiesAndMaps.Books
{
    public class Book
    {
        // 1. должен быть конструктор без параметров
        // 2. все свойства делаем виртуальными, чтобы NH смог создать прокси-класс, переобпределив в нем все свойства

        public virtual long Id { get; set; }

        public virtual string Title { get; set; }

        public virtual int Year { get; set; }
    }
}