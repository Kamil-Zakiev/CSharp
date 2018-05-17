namespace CheckCtorExecutionTime
{
    using System;
    using System.Reflection;

    [AttributeUsage(AttributeTargets.Class)]
    public class MyAttr : Attribute
    {
        public string SomeInfo { get; }
        
        public MyAttr(string someInfo)
        {
            SomeInfo = someInfo;
            Console.WriteLine("ctor was executed");
        }
    }
    
    [MyAttr("test information")]
    internal class Program
    {
        /// <summary> ctor of attribute is executed in runtime </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine(1);
            var attr = typeof(Program).GetCustomAttribute<MyAttr>();
            Console.WriteLine(2);
            Console.WriteLine(attr.SomeInfo);
            Console.WriteLine(3);
        }
    }
}