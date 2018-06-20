namespace OptimisticLockTest.Entities
{    
    /// <summary> Собака </summary>
    public class Dog
    {
        /// <summary> Идентификатор собаки </summary>
        public virtual long Id { get; set; }

        /// <summary> Имя </summary>
        public virtual string Name { get; set; }

        /// <summary> Бодствует ли? </summary>
        public virtual bool IsAwake { get; set; }

        /// <summary> Возраст </summary>
        public virtual int Age { get; set; }

        /// <summary> Версия записи. Увеличивается на 1-цу при обновлении и создании </summary>
        public virtual int RowVersion { get; set; }
    }
}