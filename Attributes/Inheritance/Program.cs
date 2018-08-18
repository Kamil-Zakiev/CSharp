using System;
using System.Linq;
using System.Reflection;

namespace Inheritance
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    internal class MyAttribute: Attribute
    {
        public string Text { get; set; }
    }
    
    internal class Base
    {
        [My(Text = "Base class property attribute")]
        public virtual string Name1 { get; set; }

        [My(Text = "Base class property attribute")]
        public virtual string Name2 { get; set; }

        [My(Text = "Base class property attribute")]
        public virtual string Name3 { get; set; }
    }

    internal class Derived1 : Base
    {
       [My(Text = "Derived1 class property attribute")]
       public override string Name1 { get; set; }

        [My(Text = "Base class property attribute")]
        public override string Name2 { get; set; }
    }

    internal class Derived2 : Derived1
    {
       [My(Text = "Derived2 class property attribute")]
       public override string Name1 { get; set; }

        public override string Name3 { get; set; }
    }
    
    internal class Program
    {
        public static void Main(string[] args)
        {
            GetCustomAttributesTest();
            GetCustomAttributeTest();
        }

        private static void GetCustomAttributeTest()
        {
            var name1Pi = typeof(Derived2).GetProperty("Name1");
            // Console.WriteLine((name1Pi.GetCustomAttribute(typeof(MyAttribute)) as MyAttribute).Text); // ambi exc
            Console.WriteLine((name1Pi.GetCustomAttribute(typeof(MyAttribute), false) as MyAttribute).Text); // Der2

            //Console.WriteLine((name1Pi.GetCustomAttribute(typeof(MyAttribute), true) as MyAttribute).Text); // ambi exc

            var name3Pi = typeof(Derived2).GetProperty("Name3");

            Console.WriteLine((name3Pi.GetCustomAttribute(typeof(MyAttribute)) as MyAttribute).Text); // base
            // Console.WriteLine((name3Pi.GetCustomAttribute(typeof(MyAttribute), false) as MyAttribute).Text); // null exc
            Console.WriteLine((name3Pi.GetCustomAttribute(typeof(MyAttribute), true) as MyAttribute).Text); // base
        }

        private static void GetCustomAttributesTest()
        {
            var name1Pi = typeof(Derived2).GetProperty("Name1");
            
            Console.WriteLine(name1Pi.GetCustomAttributes(typeof(MyAttribute)).Count()); // 3
            Console.WriteLine((name1Pi.GetCustomAttributes(typeof(MyAttribute), true).Single() as MyAttribute).Text); // Derived2
            Console.WriteLine((name1Pi.GetCustomAttributes(typeof(MyAttribute), false).Single() as MyAttribute).Text); // Derived2

            var name3Pi = typeof(Derived2).GetProperty("Name3");
            
            Console.WriteLine(name3Pi.GetCustomAttributes(typeof(MyAttribute)).Count()); // 1
            Console.WriteLine(name3Pi.GetCustomAttributes(typeof(MyAttribute), true).Length == 0); // True
            Console.WriteLine(name3Pi.GetCustomAttributes(typeof(MyAttribute), false).Length == 0); // True
        }
    }
}