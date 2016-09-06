using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace blqw.Serializable.JsonWriters
{
    internal class NameValueCollectionWrite : IJsonWriter
    {
        public Type Type => typeof(NameValueCollection);

        public void Write(object obj, JsonWriterArgs args)
        {
            if (obj == null)
            {
                args.WriterContainer.GetNullWriter().Write(null, args);
                return;
            }
            var writer = args.Writer;
            var value = (NameValueCollection)obj;
            var comma = new CommaHelper(writer);
            args.BeginObject();
            for (int i = 0, length = value.Count; i < length; i++)
            {
                var name = value.GetKey(i);
                var str = value.Get(i);
                if (args.IgnoreNullMember)
                {
                    if (str == null)
                    {
                        continue;
                    }
                }
                comma.AppendCommaIgnoreFirst();
                args.WriterContainer.GetWriter<string>().Write(name, args);
                writer.Write(':');
                args.WriterContainer.GetWriter<string>().Write(str, args);
            }
            args.EndObject();
        }
    }
}