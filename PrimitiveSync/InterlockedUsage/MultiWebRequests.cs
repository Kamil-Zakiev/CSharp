using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace InterlockedUsage
{
    internal sealed class MultiWebRequests
    {
        private readonly AsyncCoordinator m_ac = new AsyncCoordinator();

        private Dictionary<string, object> m_servers = new Dictionary<string, object>
        {
            {"http://Wintellect.com/", null},
            {"http://Microsoft.com/", null},
            //{"http://1.1.1.1/", null}
        };

        public MultiWebRequests(int timeout = Timeout.Infinite)
        {
            m_ac.AboutToBegin(m_servers.Count);

            var keys = m_servers.Keys.ToArray();
            // httpClient нельзя оборачивать в using! из-за этого вызовется dispose в все текущие запросы автоматически отменятся!
            var httpClient = new HttpClient();
            foreach (var mServer in keys)
            {
                httpClient.GetByteArrayAsync(mServer)
                    .ContinueWith(task =>
                    {
                        m_servers[mServer] = task.Exception != null
                            ? task.Exception.InnerException
                            : (object) task.Result.Length;
                        m_ac.JustEnded();
                    });
            }

            m_ac.AllBegun(AllDone, timeout);
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