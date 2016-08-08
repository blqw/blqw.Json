using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable.JsonWriters
{
    interface IMultiJsonWirters
    {
        void Add(JsonWriterWrapper writer);
    }
}
