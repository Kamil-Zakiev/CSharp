using System;
using System.Collections.Generic;
using System.Linq;

namespace TemplateMethod
{
    public abstract class IntegersCache
    {
        protected IReadOnlyList<int> _integers;
        
        protected abstract void InitCache();

        public int Median()
        {
            InitCache();
            throw new NotImplementedException();
        }

        public double Average()
        {
            InitCache();
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

        protected override void InitCache()
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

        protected override void InitCache()
        {
            if (_integers == null)
            {
                // read integers from file available by _filePath field
            }
        }
    }

}
