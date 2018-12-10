using System;

namespace FactoryMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    public class ConcreteProduct : IProduct
    {
        public ConcreteProduct(int price)
        {
            Price = price;
        }

        public int Price { get; }
    }

    public interface IProduct
    {
        int Price { get; }
    }


    public abstract class BaseClass
    {
        protected abstract IProduct CreateProduct();

        public int CalculatePrice()
        {
            var product = CreateProduct();
            return product.Price;
        }
    }

    public class DerivedClass : BaseClass
    {
        protected override IProduct CreateProduct()
        {
            return new ConcreteProduct(100);
        }
    }

    public class SomeClass
    {
        private Func<IProduct> _priceFactory;

        public SomeClass(Func<IProduct> priceFactory)
        {
            _priceFactory = priceFactory;
        }

        public int CalculatePrice()
        {
            var product = _priceFactory();
            return product.Price;
        }
    }
}
