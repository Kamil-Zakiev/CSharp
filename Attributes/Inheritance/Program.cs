using System;
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
        public virtual string Name { get; set; }
    }

    internal class Derived1 : Base
    {
       [My(Text = "Derived1 class property attribute")]
       public override string Name { get; set; }
    }
    

    internal class Derived2 : Base
    {
        public override string Name { get; set; }
    }
    
    internal class Program
    {
        public static void Main(string[] args)
        {
            var namePi = typeof(Derived1).GetProperty("Name");
            Console.WriteLine(namePi.DeclaringType);
            
            Console.WriteLine(namePi.GetCustomAttributes(typeof(MyAttribute), false).Length); // 1
            Console.WriteLine(namePi.GetCustomAttributes(typeof(MyAttribute), true).Length); // 1
            
            // если искать атрибут с помощью GetCustomAttribute и указать inherit = false, то поиск будет вестить только для свойства текущего класса
            Console.WriteLine((namePi.GetCustomAttribute(typeof(MyAttribute), false) as MyAttribute).Text);
            // Derived1 class property attribute
            
            // если искать атрибут с помощью GetCustomAttribute и указать inherit = true, то будет вестись поиск в род. классах
            var namePi2 = typeof(Derived2).GetProperty("Name");
            Console.WriteLine((namePi2.GetCustomAttribute(typeof(MyAttribute), true) as MyAttribute).Text);
            // Base class property attribute
        }
    }
}