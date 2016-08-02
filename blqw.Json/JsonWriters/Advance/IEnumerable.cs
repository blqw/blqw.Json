using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable.JsonWriters
{
    class IEnumerableWriter : IJsonWriter
    {
        public Type Type { get; } = typeof(IEnumerable);

        public void Write(object obj, JsonWriterArgs args)
        {
            if (obj == null)
            {
                JsonWriterContainer.NullWriter.Write(null, args);
                return;
            }
            var writer = args.Writer;

            writer.Write('[');
            var comma = new CommaHelper(writer);
            foreach (var value in (IEnumerable)obj)
            {
                if (args.IgnoreNullMember)
                {
                    if (value == null || value is DBNull)
                    {
                        continue;
                    }
                }

                comma.AppendCommaIgnoreFirst();
                args.WriteCheckLoop(value);
            }

            writer.Write(']');
        }

    }
}
