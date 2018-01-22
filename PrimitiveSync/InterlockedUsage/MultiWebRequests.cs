using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace InterlockedUsage
{
    internal sealed class MultiWebRequests
    {
        private AsyncCoordinator m_ac = new AsyncCoordinator();

        private Dictionary<string, object> m_servers = new Dictionary<string, object>
        {
            {"http://Wintellect.com/", null},
            {"http://Microsoft.com/", null},
            {"http://1.1.1.1/", null}
        };

        public MultiWebRequests(int timeout = Timeout.Infinite)
        {
            using (var httpClient = new HttpClient())
            {
                m_ac.AboutToBegin(m_servers.Count);
                foreach (var mServer in m_servers.Keys)
                {
                    var t = httpClient.GetByteArrayAsync(mServer);
                    t.ContinueWith(task => ComputeResult(mServer, task));
                }
                
                m_ac.AllBegun(AllDone, timeout);
            }
        }

        private void ComputeResult(string server, Task<byte[]> task)
        {
            m_servers[server] = task.Exception != null ? task.Exception.InnerException : (object)task.Result.Length;
        }

        /// <summary> Результаты игнорируются, но продолжают своб работу... </summary>
        public void Cancel()
        {
            m_ac.Cancel();
        }

        private void AllDone(CoordinationStatus coordinationStatus)
        {
            switch (coordinationStatus)
            {
                case CoordinationStatus.Cancel:
                    Console.WriteLine("Operation was cancelled");
                    return;
                case CoordinationStatus.Timeout:
                    Console.WriteLine("Operation has timed out");
                    return;
            }

            Console.WriteLine("Operation completed, results below:");
            foreach (var mServer in m_servers)
            {
                Console.Write($"{mServer.Key} ");
                var result = mServer.Value;
                if (result is Exception)
                {
                    Console.WriteLine($"failed due to {result.GetType().Name}.");
                    continue;
                }

                Console.WriteLine($"returned {result:D} bytes.");
            }

        }
    }
}