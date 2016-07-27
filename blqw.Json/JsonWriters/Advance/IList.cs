using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable.JsonWriters
{
    class IListWriter : IJsonWriter
    {
        public Type Type { get; } = typeof(IList);

        public void Write(object obj, JsonWriterArgs args)
        {
            if (obj == null)
            {
                JsonWriterContainer.NullWriter.Write(null, args);
                return;
            }
            var writer = args.Writer;
            var list = (IList)obj;

            writer.Write('[');
            var comma = new CommaHelper(writer);

            for (int i = 0, length = list.Count; i < length; i++)
            {
                var value = list[i];
                if (args.IgnoreNullMember)
                {
                    if (value == null || value is DBNull)
                        continue;
                }

                comma.AppendCommaIgnoreFirst();

                JsonWriterContainer.Write(value, args);
            }

            writer.Write(']');
        }
    }
}