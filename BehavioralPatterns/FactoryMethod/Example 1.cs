namespace FactoryMethod
{
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

    public class ConcreteProduct : IProduct
    {
        public ConcreteProduct(int price)
        {
            Price = price;
        }

        public int Price { get; }
    }

    public class DerivedClass : BaseClass
    {
        protected override IProduct CreateProduct()
        {
            return new ConcreteProduct(100);
        }
    }
}
