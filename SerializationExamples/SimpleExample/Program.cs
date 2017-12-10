using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace SimpleExample
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var obj = GetObjectForSerialization();

            var stream = new MemoryStream();
            SerializeToMemory(obj, stream);
            
            string path = AppDomain.CurrentDomain.BaseDirectory + "store";
            File.WriteAllBytes(path, stream.ToArray());
            
            stream = new MemoryStream();
            var bytes = File.ReadAllBytes(path);
            stream.Write(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            var desObj = DeserializeFromMemory(stream);
            Console.WriteLine($"desObj.Type = {desObj.GetType()}");
            
            var list = (List<string>) desObj;
            var lines = list.Select((item, i) => new
                {
                    item,
                    q = i / 25
                }).GroupBy(x => x.q)
                .Select(g => string.Join(",\t", g.Select(x => x.item)));
            var info = string.Join(Environment.NewLine, lines);
            Console.WriteLine(info);
        }

        private static object DeserializeFromMemory(MemoryStream stream)
        {
            var formatter = new BinaryFormatter();
            return formatter.Deserialize(stream);
        }

        private static void SerializeToMemory(object o, MemoryStream stream)
        {
            var formatter = new BinaryFormatter();
            Console.WriteLine(o.GetType().Assembly);
            Console.WriteLine(o.GetType());
            formatter.Serialize(stream, o);
        }

        private static Object GetObjectForSerialization()
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