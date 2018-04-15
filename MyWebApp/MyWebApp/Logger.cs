using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace MyWebApp
{
    public static class Logger
    {
        private const string Path = @"E:\Stash\CSharp\MyWebApp\MyWebApp\log.txt";
        
        private static readonly object _lock = new object();

        static Logger()
        {
            File.WriteAllText(Path, DateTime.Now.ToString("g") + Environment.NewLine);
        }
        
        public static void Log(Guid appGuid,[CallerFilePath] string callerFilePath = "",  [CallerMemberName]string callerMemberName = "")
        {
            var time = DateTime.Now;
            lock (_lock)
            {
                var csFile = callerFilePath.Split('\\').Last();
                File.AppendAllLines(Path,
                    new[]
                    {
                        $"{time}: {csFile}, {callerMemberName}: {appGuid} was called by Thread #{Thread.CurrentThread.ManagedThreadId}"
                    });
            }
            
           
        }
    }
}