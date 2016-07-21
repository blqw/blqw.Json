using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable.JsonWriters
{
    sealed class SingleWriter : IJsonWriter
    {
        public Type Type
        {
            get
            {
                return typeof(float);
            }
        }

        public void Write(object obj, JsonWriterArgs args)
        {
            var value = (float)obj;
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
