using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using static blqw.Serializable.JsonWriterContainer;

namespace blqw.Serializable.JsonWriters
{
    internal class NameValueCollectionWrite : IJsonWriter
    {
        private JsonWriterWrapper _wrapper;
        public JsonWriterWrapper Wrapper => _wrapper ?? (_wrapper = GetWrap(typeof(string[])));
        public Type Type => typeof(NameValueCollection);


        public void Write(object obj, JsonWriterArgs args)
        {
            if (obj == null)
            {
                NullWriter.Write(null, args);
                return;
            }
            var writer = args.Writer;
            var value = (NameValueCollection)obj;
            var comma = new CommaHelper(writer);
            writer.Write('{');
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
                JsonWriterContainer.StringWriter.Write(name, args);
                writer.Write(':');
                JsonWriterContainer.StringWriter.Write(str, args);
            }
            writer.Write('}');
        }
    }
}