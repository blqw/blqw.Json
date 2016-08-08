using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable.JsonWriters
{
    class IEnumeratorWriter : IJsonWriter
    {
        public Type Type { get; } = typeof(IEnumerator);

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
            var ee = (IEnumerator) obj;
            while (ee.MoveNext())
            {
                var value = ee.Current;
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
