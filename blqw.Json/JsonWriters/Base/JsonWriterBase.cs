using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable.JsonWriters
{
    abstract class JsonWriterBase : IJsonWriter
    {
        public abstract Type Type { get; }

        public abstract void Write(object obj, JsonWriterArgs args);


        void IJsonWriter.Write(object obj, JsonWriterArgs args)
        {
            if (args.Entry(obj))
            {
                Write(obj, args);
            }
            else
            {
                args.Writer.Write("undefined");
            }
        }
    }
}
