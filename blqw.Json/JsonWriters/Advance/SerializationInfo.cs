using System;
using System.Runtime.Serialization;

namespace blqw.Serializable.JsonWriters
{
    internal class SerializationInfoWrite : IJsonWriter
    {
        public Type Type => typeof(SerializationInfo);

        public void Write(object obj, JsonWriterArgs args)
        {
            if (obj == null)
            {
                JsonWriterContainer.NullWriter.Write(null, args);
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
                JsonWriterContainer.StringWriter.Write(item.Name, args);
                writer.Write(':');

                args.WriteCheckLoop(item.Value, null);
            }


            throw new NotImplementedException();
        }
    }
}