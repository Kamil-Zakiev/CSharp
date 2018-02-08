using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace MyWebApp
{
    public static class Logger
    {
        private const string Path = @"G:\CSharp\MyWebApp\MyWebApp\log.txt";

        static Logger()
        {
            File.WriteAllText(Path, DateTime.Now.ToString("g") + Environment.NewLine);
        }
        
        public static void Log(Guid appGuid,[CallerFilePath] string callerFilePath = "",  [CallerMemberName]string callerMemberName = "")
        {
          /*  var csFile = callerFilePath.Split('\\').Last();
            File.AppendAllLines(Path,
                new[]
                {
                    $"{csFile}, {callerMemberName}: {appGuid} was called by Thread #{Thread.CurrentThread.ManagedThreadId}"
                });*/
        }
    }
}