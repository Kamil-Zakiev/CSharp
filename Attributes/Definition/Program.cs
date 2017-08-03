namespace Definition
{
    using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    internal class MyOwnAttribute : Attribute
    {
        public string Config { get; }
        
        public MyOwnAttribute(string config)
        {
            Config = config;
        }

        public string SomeAdditionalData { get; set; }
    }

    public class BaseType
    {
        [MyOwn("Some Config", SomeAdditionalData = "add data")]
        public virtual void Example()
        {
        }
    }

    public class DerivedType : BaseType
    {
        public override void Example()
        {
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
        }
    }
}