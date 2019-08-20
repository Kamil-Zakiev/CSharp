using System.Collections.Generic;
using System.Linq;

namespace Composite
{
    public interface IHasPrice
    {
        int Price { get; }
    }

    public class Box : IHasPrice
    {
        private List<IHasPrice> _items = new List<IHasPrice>();

        public IReadOnlyList<object> Items => _items;

        public int Price => _items.Sum(item => item.Price);
    }

    public class Item : IHasPrice
    {
        public int Price => 123;
    }

    public class SomeClass
    {
        public string GetOrderCost(IHasPrice smthWithPrice)
        {
            return $"Order total = {smthWithPrice.Price}";
        }
    }
}
