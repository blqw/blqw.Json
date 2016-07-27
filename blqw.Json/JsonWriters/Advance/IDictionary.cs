using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Serializable.JsonWriters
{
    class IDictionaryWriter : IJsonWriter
    {
        public Type Type { get; } = typeof(IDictionary);

        public void Write(object obj, JsonWriterArgs args)
        {
            if (obj == null)
            {
                JsonWriterContainer.NullWriter.Write(null, args);
                return;
            }
            var writer = args.Writer;

            writer.Write('{');
            var comma = new CommaHelper(writer);
            var ee = ((IDictionary)obj).GetEnumerator();
            while (ee.MoveNext())
            {
                var v = ee.Value;
                if (args.IgnoreNullMember)
                {
                    if (v == null || v is DBNull)
                        continue;
                }

                comma.AppendCommaIgnoreFirst();

                JsonWriterContainer.StringWriter.Write(ee.Key.To<string>(), args);
                writer.Write(':');
                JsonWriterContainer.Write(ee.Value, args);
            }

            writer.Write('}');
        }
    }
}
