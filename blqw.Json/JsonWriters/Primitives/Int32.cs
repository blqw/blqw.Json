using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable.JsonWriters
{
    sealed class Int32Writer : IJsonWriter
    {
        public Type Type
        {
            get
            {
                return typeof(int);
            }
        }

        public void Write(object obj, JsonWriterArgs args)
        {
            var value = (int)obj;
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
