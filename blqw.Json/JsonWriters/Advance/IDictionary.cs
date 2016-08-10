using System;
using System.Collections;

namespace blqw.Serializable.JsonWriters
{
    internal class IDictionaryWriter : IJsonWriter
    {
        public Type Type => typeof(IDictionary);

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
            var ee = ((IDictionary) obj).GetEnumerator();
            while (ee.MoveNext())
            {
                var value = ee.Value;
                if (args.IgnoreNullMember)
                {
                    if (value == null || value is DBNull)
                        continue;
                }

                comma.AppendCommaIgnoreFirst();

                JsonWriterContainer.StringWriter.Write(ee.Key.To<string>(), args);
                writer.Write(':');
                args.WriteCheckLoop(value, null);
            }

            writer.Write('}');
        }
    }
}