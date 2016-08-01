using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable.JsonWriters
{
    internal sealed class Null : IJsonWriter
    {
        public Type Type => typeof(void);

        public void Write(object obj, JsonWriterArgs args)
        {
            args.Writer.Write("null");
        }
    }
}
