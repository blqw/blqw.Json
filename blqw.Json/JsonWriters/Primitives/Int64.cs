using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable.JsonWriters
{
    sealed class Int64Writer : IJsonWriter
    {
        public Type Type
        {
            get
            {
                return typeof(long);
            }
        }

        public void Write(object obj, JsonWriterArgs args)
        {
            var value = (long)obj;
            var writer = args.Writer;
            if (args.QuotWrapNumber)
            {
                writer.Write('"');
                writer.Write(value);
                writer.Write('"');
            }
            else
            {
                writer.Write(value);
            }
        }
    }
}
