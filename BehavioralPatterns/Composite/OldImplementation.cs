using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Composite.OldImplementation
{
    public class Box
    {
        private List<object> _items = new List<object>();

        public IReadOnlyList<object> Items => _items;
    }

    public class Item
    {
        public int Price => 123;
    }

    public class SomeClass
    {
        private int CalculateBoxPrice(Box box)
        {
            return box.Items
                .Sum(x =>
                {
                    switch (x)
                    {
                        case Box b: return CalculateBoxPrice(b);
                        case Item i: return i.Price;
                        default: return 0;
                    }
                });
        }

        public string GetOrderCost(Box box)
        {
            return "Order cost is " + CalculateBoxPrice(box);
        }
    }
}
