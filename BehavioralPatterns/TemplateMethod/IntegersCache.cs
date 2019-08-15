using System;
using System.Collections.Generic;
using System.Linq;

namespace TemplateMethod
{
    public abstract class IntegersCache
    {
        protected IReadOnlyList<int> _integers;
        
        protected abstract void Init();

        public int Median()
        {
            Init();
            throw new NotImplementedException();
        }

        public double Average()
        {
            Init();
            return _integers.Average();
        }
    }

    public class NetworkIntegersCache : IntegersCache
    {
        private string _url;

        public NetworkIntegersCache(string url)
        {
            _url = url;
        }

        protected override void Init()
        {
            if (_integers == null)
            {
                // read integers by network
            }
        }
    }

    public class FileIntegersCache : IntegersCache
    {
        private string _filePath;

        public FileIntegersCache(string filePath)
        {
            _filePath = filePath;
        }

        protected override void Init()
        {
            if (_integers == null)
            {
                // read integers from file available by _filePath field
            }
        }
    }

}
