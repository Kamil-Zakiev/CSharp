using System;

namespace FactoryMethod
{
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
