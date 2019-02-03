using System;
using System.Runtime.Serialization;

namespace blqw.JsonServices.JsonWriters
{
    internal class SerializationInfoWrite : IJsonWriter
    {
        public Type Type => typeof(SerializationInfo);

        public void Write(object obj, JsonWriterSettings args)
        {
            if (obj == null)
            {
                args.WriteNull();
                return;
            }
            var writer = args.Writer;
            var value = (SerializationInfo) obj;
            var comma = new CommaHelper(writer);
            foreach (var item in value)
            {
                if (args.IgnoreNullMember)
                {
                    if (item.Value == null || item.Value is DBNull)
                    {
                        continue;
                    }
                }
                comma.AppendCommaIgnoreFirst();
                args.Write(item.Name);
                writer.Write(':');

                args.WriteCheckLoop(item.Value, null);
            }
        }
    }
}