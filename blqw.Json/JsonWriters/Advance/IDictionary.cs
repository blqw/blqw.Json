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
                args.WriterContainer.GetNullWriter().Write(null, args);
                return;
            }
            var writer = args.Writer;

            args.BeginObject();
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

                args.WriterContainer.GetWriter<string>().Write(ee.Key.To<string>(), args);
                writer.Write(':');
                args.WriteCheckLoop(value, null);
            }

            args.EndObject();
        }
    }
}