using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable
{
    struct CommaHelper
    {
        const char COMMA = ',';
        private readonly TextWriter _writer;
        private bool _first;
        public CommaHelper(TextWriter writer)
        {
            _writer = writer;
            _first = true;
        }

        public void AppendCommaIgnoreFirst()
        {
            if (_first)
                _first = false;
            else
                _writer.Write(COMMA);
        }
    }
}
