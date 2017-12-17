using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace StaticFields
{
    [Serializable]
    class Program
    {
        [NonSerialized]
        private static int k = 1;

        /// <summary>
        /// Стат. поля не сериализуются, это логично - стат. поле не относится к состоянию одного отдельно взятого объекта
        /// Если надо, то выход - кастомная настройка сериализации
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var obj = new Program();

            var stream = new MemoryStream();
            SerializeToMemory(obj, stream);

            Console.WriteLine("before serialization: k = " + k);
            string path = AppDomain.CurrentDomain.BaseDirectory + "store";
            File.WriteAllBytes(path, stream.ToArray());
            k = 2;
            Console.WriteLine("set: k = " + k);

            stream = new MemoryStream();
            var bytes = File.ReadAllBytes(path);
            stream.Write(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            var desObj = DeserializeFromMemory(stream);
            Console.WriteLine("after deserialization: k = " + k);
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
