using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable.JsonWriters
{
    sealed class TimeSpanWriter : IJsonWriter
    {
        public Type Type
        {
            get
            {
                return typeof(TimeSpan);
            }
        }

        public void Write(object obj, JsonWriterArgs args)
        {
            var value = (TimeSpan)obj;
            var writer = args.Writer;
            if (value.TotalMinutes % 1 == 0)
            {
                JsonWriterContainer.StringWriter.Write(value.ToString("g"),args);
            }
            else
            {
                JsonWriterContainer.StringWriter.Write(value.ToString("G"), args);
            }
        }
    }
}
