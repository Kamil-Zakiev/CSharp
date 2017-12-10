using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace DeepCloneCreation
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var obj = GetObject();

            var copy = GetDeepCopy(obj);
        }

        private static object GetDeepCopy(object obj)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Context = new StreamingContext(StreamingContextStates.Clone);
                formatter.Serialize(stream, obj);
                stream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(stream);
            }
        }

        private static object GetObject()
        {
            const int itemsCount = 50;
            var data = new List<string>(itemsCount);
            for (var i = 0; i < itemsCount; i++)
            {
                var item = "item" + i;
                data.Add(item);
            }

            return data;
        }
    }
}