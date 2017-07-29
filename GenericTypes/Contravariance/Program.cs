namespace Contravariance
{
    internal interface IGenericType<in T> { }

    public class GenericClass<T> : IGenericType<T> { }

    public class A { }

    public class B : A { }


    public sealed class Program
    {
        private static void Method(IGenericType<B> gen)
        {
        }

        static void Main(string[] args)
        {
            ContravarianceOnClass();
            ContravarianceOnInterface();
        }

        private static void ContravarianceOnClass()
        {
            var a = new GenericClass<A>();
            var b = new GenericClass<B>();
            // Error CS0029  Cannot implicitly convert type 'Contravariance.GenericClass<Contravariance.A>' to 'Contravariance.GenericClass<Contravariance.B>'
            // b = a;
        }


        private static void ContravarianceOnInterface()
        {
            IGenericType<A> a = new GenericClass<A>();
            IGenericType<B> b = new GenericClass<B>();
            b = a;

            // контрвариантность позволяет передать объект класса, реализующий IGenericType<A>, в метод, ожидающий объект,  реализующий IGenericType<B>
            // на интерфейсах это пиздецки сложно догнать, на делегатах - очевидное поведение
            Method(a);
        }
    }
}
