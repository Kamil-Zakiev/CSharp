using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Remoting.Messaging;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace ExecutionContextTest
{
    [DebuggerDisplay("{" + nameof(Name) + "}")]
    class MyIdentity : IIdentity
    {
        public MyIdentity(bool isAuthenticated, string name, string authenticationType)
        {
            IsAuthenticated = isAuthenticated;
            Name = name;
            AuthenticationType = authenticationType;
        }

        public string Name { get; }
        public string AuthenticationType { get; }
        public bool IsAuthenticated { get; }

        public override string ToString()
        {
            return Name;
        }
    }
    
    internal static class Program
    {
        public static void Main(string[] args)
        {
            TestTlsForTaskWithAsync().Wait();
        }

        private static void TaskTest()
        {
            var myIdentity = new MyIdentity(true, "kamil", "manual");
            var myPrincipal = new GenericPrincipal(myIdentity, new[] {"code author"});

            // Thread.CurrentThread.GetExecutionContextReader().LogicalCallContext.Principal = myPrincipal
            Thread.CurrentPrincipal = myPrincipal;
            CallContext.LogicalSetData("name", "from main thread");

            // Thread.CurrentPrincipal устанавливает Principal для текущего потока и далее он копируется в дочерние потоки (то же верно для контекста)

            ShowCurrentThreadContext(); // "ContextID: 0"; kamil

            var t = Task.Run(() =>
            {
                Console.WriteLine(CallContext.LogicalGetData("name"));
                ShowCurrentThreadContext();

                var myIdentity2 = new MyIdentity(true, "kamil2", "manual");
                var myPrincipal2 = new GenericPrincipal(myIdentity2, new[] {"code author"});
                Thread.CurrentPrincipal = myPrincipal2;

                ShowCurrentThreadContext();
            });

            t.Wait();
            ShowCurrentThreadContext();
        }

        private static void ThreadsTest()
        {
            var myIdentity = new MyIdentity(true, "kamil", "manual");
            var myPrincipal = new GenericPrincipal(myIdentity, new[] {"code author"});
            // Thread.CurrentThread.GetExecutionContextReader().LogicalCallContext.Principal = myPrincipal
            Thread.CurrentPrincipal = myPrincipal;
            System.Runtime.Remoting.Messaging.CallContext.LogicalSetData("name", "from main thread 2");
            
            // Thread.CurrentPrincipal sets Principal for the current thread and then it`s copied to child treads
            // the same is true for the context

            ShowCurrentThreadContext(); // "ContextID: 0"; kamil

            var newThread = new Thread(() =>
            {
                ShowCurrentThreadContext();
                
                Console.WriteLine(CallContext.LogicalGetData("name"));

                var myIdentity2 = new MyIdentity(true, "kamil2", "manual");
                var myPrincipal2 = new GenericPrincipal(myIdentity2, new[] {"code author"});
                Thread.CurrentPrincipal = myPrincipal2;

                ShowCurrentThreadContext();
            });

            newThread.Start();
            newThread.Join();
            ShowCurrentThreadContext();
        }

        [ThreadStatic] public static int Value;

        private static void TestTlsForThread()
        {
            Value = 10;
            Console.WriteLine($"Value = {Value}"); // shows 10
            var newThread = new Thread(() =>
            {
                Console.WriteLine($"Value = {Value}"); // show 0, as expected since a new thread is started
            });

            newThread.Start();
            newThread.Join();
        }

        private static void TestTlsForTask()
        {
            Value = 10;
            Console.WriteLine($"Value = {Value}"); // shows 10
            var t = Task.Run(() =>
            {
                Console.WriteLine($"Value = {Value}"); // show 0, as expected since a new thread is started
            });

            t.Wait();
        }
        
        private static async Task TestTlsForTaskWithAsync()
        {
            Value = 10;
            Console.WriteLine($"Value = {Value}\n"); // shows 10
            await Task.Delay(1000);
            Console.WriteLine($"Value = {Value}"); // show 0...
        }
        
        private static void ShowCurrentThreadContext()
        {
            Console.WriteLine(Thread.CurrentThread.ExecutionContext.GetHashCode());
            Console.WriteLine($"ThreadId = {Thread.CurrentThread.ManagedThreadId}: {Thread.CurrentContext}; {Thread.CurrentPrincipal.Identity}");
        }
    }
}