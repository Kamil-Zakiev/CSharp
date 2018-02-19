using System;
using System.Linq;
using System.Reflection;
using NullGuard;

namespace FodyTesting
{
    public class Person
    {
        public string Name { get; set; }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            var person = new Person();

            try
            {
                person.Name = null;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e);
                Console.WriteLine();
            }            
            
            var nameProp = person.GetType().GetTypeInfo().DeclaredProperties.Single(p => p.Name == "Name");
            nameProp.SetValue(person, "123");
            
            Console.WriteLine(person.Name);
        }
    }
}