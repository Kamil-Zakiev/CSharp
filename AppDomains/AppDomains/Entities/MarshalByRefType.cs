using System;
using System.Collections.Generic;
using System.Threading;

namespace AppDomains.Entities
{
    /// <summary>
    ///     Экземпляры допускают продвижение по ссылке через границы доменов
    /// </summary>
    public sealed class MarshalByRefType : MarshalByRefObject
    {
        private List<double> _longDouble = new List<double>(10000000);

        public int k;

        public MarshalByRefType()
        {
            Console.WriteLine($"{GetType()} ctor running in \"{Thread.GetDomain().FriendlyName}\"");
        }

        public void SomeMethod()
        {
            Console.WriteLine($"Code is executing in \"{Thread.GetDomain().FriendlyName}\"");
        }

        public MarshalByValType MethodWithReturn()
        {
            Console.WriteLine("Executing in " + Thread.GetDomain().FriendlyName);
            var t = new MarshalByValType();
            return t;
        }

        public NonMarshalableType MethodArgAndReturn(string callingDomainName)
        {
            // ПРИМЕЧАНИЕ: callingDomainName имеет атрибут [Serializable]
            Console.WriteLine($"Calling from '{callingDomainName}' to '{Thread.GetDomain().FriendlyName}'.");
            var t = new NonMarshalableType();
            return t;
        }
    }
}