namespace FactoryMethod
{
    public interface IProduct
    {
        int Price { get; }
    }

    public abstract class ProductFactory
    {
        protected abstract IProduct CreateProduct();

        public int CalculatePrice()
        {
            var product = CreateProduct();
            return product.Price;
        }
    }

    public class Phone : IProduct
    {
        public Phone(int price)
        {
            Price = price;
        }

        public int Price { get; }
    }

    public class PhoneFactory : ProductFactory
    {
        protected override IProduct CreateProduct()
        {
            return new Phone(100);
        }
    }
}
