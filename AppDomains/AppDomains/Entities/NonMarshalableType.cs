using System;
using System.Threading;

namespace AppDomains.Entities
{
    // Экземпляры не допускают продвижение между доменами
    // [Serializable]
    public sealed class NonMarshalableType : object
    {
        public NonMarshalableType()
        {
            Console.WriteLine("Executing in " + Thread.GetDomain().FriendlyName);
        }
    }
}