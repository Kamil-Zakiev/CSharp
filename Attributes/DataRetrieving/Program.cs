namespace DataRetrieving
{
    using System;
    using System.Linq;
    using System.Reflection;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    internal class MyOwnAttribute : Attribute
    {
        public MyOwnAttribute(string config)
        {
            Config = config;
        }

        public string Config { get; }

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
            Example2();
        }

        /// <summary>
        ///     Вытягивание данных из атрибута
        /// </summary>
        private static void Example1()
        {
            var members = typeof(BaseType).GetMembers()
                .Where(x => x.MemberType == MemberTypes.Method)
                .Where(x => x.DeclaringType == typeof(BaseType))
                .ToArray();
            if (members.Length > 0)
                PrintAttributeInfo(members[0]);
        }

        /// <summary>
        ///     Вытягивание данных из атрибута. Проверка наследования атрибута
        /// </summary>
        private static void Example2()
        {
            var members = typeof(DerivedType).GetMembers()
                .Where(x => x.MemberType == MemberTypes.Method)
                .Where(x => x.DeclaringType == typeof(DerivedType))
                .ToArray();
            if (members.Length > 0)
                PrintAttributeInfo(members[0]);
        }

        private static void PrintAttributeInfo(MemberInfo memberInfo)
        {
            var attributes = memberInfo.GetCustomAttributes(typeof(Attribute), true);

            foreach (var attribute in attributes)
            {
                Console.WriteLine(attribute.GetType());

                var myOwnAttr = attribute as MyOwnAttribute;
                if (myOwnAttr != null)
                    Console.WriteLine("{0} : Config = {1}, SomeAddData = {2}", typeof(MyOwnAttribute), myOwnAttr.Config,
                        myOwnAttr.SomeAdditionalData);
            }
        }
    }
}