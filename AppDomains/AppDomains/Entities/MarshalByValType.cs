using System;
using System.Threading;

namespace AppDomains.Entities
{
    // Экземпляры допускают продвижение по значению через границы доменов
    [Serializable]
    public sealed class MarshalByValType
    {
        private DateTime m_creationTime = DateTime.Now;

        // ПРИМЕЧАНИЕ: DateTime помечен атрибутом [Serializable]
        public MarshalByValType()
        {
            Console.WriteLine("{0} ctor running in {1}, Created on {2:D}",
                GetType(),
                Thread.GetDomain().FriendlyName,
                m_creationTime);
        }

        public override string ToString()
        {
            return m_creationTime.ToLongDateString() + $" (executed in {Thread.GetDomain().FriendlyName})";
        }
    }
}