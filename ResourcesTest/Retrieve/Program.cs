using System;
using System.IO;
using System.Reflection;

namespace Retrieve
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var currentAssembly = Assembly.GetCallingAssembly();
            var names = currentAssembly.GetManifestResourceNames();

            Console.WriteLine(string.Join(Environment.NewLine, names));

            // getting stream with resource bytes
            var stream = currentAssembly.GetManifestResourceStream(names[2]);
            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);

            Console.WriteLine(memoryStream.Length);
            
            
            // emtpy values
            var info = currentAssembly.GetManifestResourceInfo(names[2]);

            Console.WriteLine("info.FileName = " + info.FileName);
            Console.WriteLine("info.ReferencedAssembly.FullName = " + info.ReferencedAssembly?.FullName);
            Console.WriteLine("info.ResourceLocation = " + info.ResourceLocation);

            // saving resource
            var file = File.OpenWrite("test" + Path.GetExtension(names[2]));
            var bufferSize = 1024;
            var buffer = new byte[bufferSize];
            stream.Seek(0, SeekOrigin.Begin);
            var bytes = 0;
            while ((bytes = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                file.Write(buffer, 0, bytes);
            }
            file.Close();
        }
    }
}