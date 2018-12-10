using System;
using System.Collections.Generic;
using System.Linq;

namespace Composite
{
    class Program
    {
        static void CalculateOrder(IHasPrice smthWithPrice)
        {
            Console.WriteLine($"Order total = {smthWithPrice.Price}");
        }
    }

    public interface IHasPrice
    {
        int Price { get; }
    }

    public class Box : IHasPrice
    {
        private List<IHasPrice> _items = new List<IHasPrice>();

        public void PutItem(IHasPrice item)
        {
            _items.Add(item);
        }

        public int Price => _items.Sum(item => item.Price);
    }

    public class Item : IHasPrice
    {
        public int Price { get; }

        public Item(int price)
        {
            Price = price;
        }
    }

}
