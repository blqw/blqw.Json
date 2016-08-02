using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable.JsonWriters
{
    sealed class BooleanWriter : IJsonWriter
    {
        public Type Type { get; } = typeof(bool);

        public void Write(object obj, JsonWriterArgs args)
        {
            var value = (bool)obj;
            if (args.BooleanToNumber)
            {
                JsonWriterContainer.Int32Writer.Write(value ? 1 : 0, args);
            }
            else if (args.QuotWrapBoolean)
            {
                args.Writer.Write('"');
                args.Writer.Write(value);
                args.Writer.Write('"');
            }
            else
            {
                args.Writer.Write(value);
            }
        }
    }
}
