using System;
using System.Collections;

namespace blqw.JsonServices.JsonWriters
{
    internal class IDictionaryWriter : IJsonWriter
    {
        public Type Type => typeof(IDictionary);

        public void Write(object obj, JsonWriterSettings args)
        {
            if (obj == null)
            {
                args.WriteNull();
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

                args.Write(ee.Key.To<string>());
                writer.Write(':');
                args.WriteCheckLoop(value, null);
            }

            args.EndObject();
        }
    }
}