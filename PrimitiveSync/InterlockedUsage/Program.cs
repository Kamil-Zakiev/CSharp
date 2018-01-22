using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InterlockedUsage
{
    enum CoordinationStatus
    {
        Cancel,
        Timeout,
        AllDone
    }

    internal sealed class MultiWebRequests
    {
        private Dictionary<string, object> m_servers = new Dictionary<string, object>();

        public MultiWebRequests(int timeout = Timeout.Infinite)
        {
            using (var httpClient = new HttpClient())
            {
                foreach (var mServer in m_servers.Keys)
                {
                    httpClient.GetByteArrayAsync(mServer).ContinueWith(task => { });
                }
            }
        }

        private void ComputeResult(string server, Task<byte[]> task)
        {
            m_servers[server] = task.Exception != null ? task.Exception.InnerException : (object)task.Result.Length;
        }

        public void Cancel()
        {
            
        }

        private void AllDone(CoordinationStatus coordinationStatus)
        {
            if (coordinationStatus == CoordinationStatus.Cancel)
            {
                Console.WriteLine("Operation was cancelled");
                return;
            }

            if (coordinationStatus == CoordinationStatus.Timeout)
            {
                Console.WriteLine("Operation has timed out");
                return;
            }

            Console.WriteLine("Operation completed, results below:");

        }
    }

    class Program
    {
        

        static void Main(string[] args)
        {
            
        }
    }
}
