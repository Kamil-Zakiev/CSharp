using System;
using System.Collections.Generic;
using System.Text;

namespace TemplateMethod.OldImplementation
{
    public class IntegersCache
    {
        private IReadOnlyList<int> _integers;

        private readonly string _filePath;

        public IntegersCache(string filePath)
        {
            _filePath = filePath;
        }

        protected void Init()
        {
            if (_integers == null)
            {
                // read from file
            }
        }

        public int Median()
        {
            Init();
            throw new NotImplementedException();
        }

        public double Average()
        {
            Init();
            throw new NotImplementedException();
        }
    }
}
