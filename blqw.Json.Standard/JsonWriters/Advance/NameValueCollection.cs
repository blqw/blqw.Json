using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace blqw.JsonServices.JsonWriters
{
    internal class NameValueCollectionWrite : IJsonWriter
    {
        public Type Type => typeof(NameValueCollection);

        public void Write(object obj, JsonWriterSettings args)
        {
            if (obj == null)
            {
                args.WriteNull();
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
                args.Write(name);
                writer.Write(':');
                //TODO:这里可以考虑下怎么处理
                args.Write(str);
            }
            args.EndObject();
        }
    }
}